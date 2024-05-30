namespace Truss.Ipc.Abstractions;

public interface IIpcClient
{
    public Task<IResponse> Send(IRequest request, CancellationToken ct);
}