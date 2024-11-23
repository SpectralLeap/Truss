using System;
using System.Linq;

namespace Truss.Monads.Results.Protobuf;

/// <summary>
/// Represents a factory class for creating result objects.
/// </summary>
/// <typeparam name="T">The type of the success value.</typeparam>
public sealed class ResultFactory<T>
{
    private readonly Result<T> _result;

    public ResultFactory(Result<T> result)
    {
        _result = result;
    }

    /// <summary>
    /// Used to map the result to a gRPC message.
    ///
    /// Ignore the result property since it will already be mapped.
    ///
    /// The mapping only occurs on success.
    /// </summary>
    /// <param name="mapping"></param>
    /// <typeparam name="TMsg"></typeparam>
    /// <returns></returns>
    public TMsg ToGrpcMessageType<TMsg>(Action<T, TMsg>? mapping = null)
        where TMsg : new()
    {
        var msg = new TMsg();

        var resultProperty = typeof(TMsg).GetProperties()
            .FirstOrDefault(p => p.PropertyType == typeof(global::Results.Protobuf.Result));

        if (resultProperty is null)
            throw new InvalidOperationException("The gRpc type does not contain a result object");

        resultProperty.SetValue(msg, _result.MapToGrpc());

        if (_result.Succeeded)
        {
            mapping?.Invoke(_result.SuccessValue, msg);
        }

        return msg;
    }

}