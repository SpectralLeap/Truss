using System.IO.Pipes;
using System.Text;
using Newtonsoft.Json;
using Truss.Ipc.Abstractions;

namespace Truss.Ipc.NamedPipes.Client;


public sealed class NamedPipeClient : IIpcClient
{
    private readonly NamedPipeClientStream _namedPipeClientStream;

    public NamedPipeClient(NamedPipeClientConfig namedPipeClientConfig)
    {
        _namedPipeClientStream = new NamedPipeClientStream(".", "", PipeDirection.InOut);
        _namedPipeClientStream.ReadMode = PipeTransmissionMode.Message;
    }
    
    public async Task<IResponse> Send(IRequest request, CancellationToken ct, TimeSpan? timeOut = null)
    {
        using var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var tokenLinkingSource = new CancellationTokenSource();
        
        await _namedPipeClientStream.ConnectAsync(ct);
        
        await SendRequest(request, ct);
        
        var responseLengthBuffer = new byte[sizeof(int)];

        var lengthResponse = await _namedPipeClientStream.ReadAsync(responseLengthBuffer, 0, responseLengthBuffer.Length, ct);
    }

    private async Task SendRequest(IRequest request, CancellationToken ct)
    {
         var requestObject = JsonConvert.SerializeObject(request);
         
         var requestHeader = nameof(request);
 
         var content = requestHeader + "|" + requestObject;
         
         var message = Encoding.UTF8.GetBytes(content);
         
         var length = BitConverter.GetBytes(message.Length);
 
         await SendBuffer(length, ct);
 
         await SendBuffer(message, ct);
         
         _namedPipeClientStream.WaitForPipeDrain();
    }

    private async Task SendBuffer(byte[] buffer, CancellationToken ct)
    {
        await _namedPipeClientStream.WriteAsync(buffer, 0, buffer.Length, ct);
    }

    public Task<IResponse> Send(IRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}

public class NamedPipeClientConfig
{
}