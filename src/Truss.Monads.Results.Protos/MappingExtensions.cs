using ResultProtos;

namespace Truss.Monads.Results.Protos;

public static class MappingExtensions
{
    public static Result<Nil> MapFromGrpc<TGrpc>(this ResultGrpcMessage resultGrpcMessage)
    {
        if (resultGrpcMessage.Succeeded)
        {
            return Result.Success();
        }
    
        return Result.Fail(resultGrpcMessage.FailureReasons.ToArray());
    }

    /// <summary>
    /// Maps a gRPC message that contains a Result to a custom object using a mapping function.
    /// The mapping function should only map the remaining components of the carrier type.
    /// </summary>
    /// <typeparam name="TGrpc">The type of the gRPC message.</typeparam>
    /// <typeparam name="TOut">The type of the mapped object.</typeparam>
    /// <param name="resultGrpcMessage">The result gRPC message.</param>
    /// <param name="grpcMessage">The gRPC message to map.</param>
    /// <param name="responseMapper">The mapping function.</param>
    /// <returns>A result object containing the mapped object if successful, or a failure object containing the failure reasons.</returns>
    public static Result<TOut> MapFromGrpc<TGrpc, TOut>(this ResultGrpcMessage resultGrpcMessage, TGrpc grpcMessage,
        Func<TGrpc, TOut> responseMapper)
    {
        if (resultGrpcMessage.Succeeded)
        {
            return Result.Success(responseMapper(grpcMessage));
        }

        return Result.Fail(resultGrpcMessage.FailureReasons.ToArray());
    }

    /// <summary>
    /// Maps a generic Result object to a ResultGrpcMessage object.
    /// </summary>
    /// <typeparam name="T">The type of the Result object.</typeparam>
    /// <param name="result">The Result object to be mapped.</param>
    /// <returns>A ResultGrpcMessage object representing the mapped Result object.</returns>
    internal static ResultGrpcMessage MapToGrpc<T>(this Result<T> result)
    {
        var resultMessage = new ResultGrpcMessage()
        {
            Succeeded = result.Succeeded
        };

        if (result.Failed) resultMessage.FailureReasons.AddRange(result.FailureDetails.FailureReasons);

        return resultMessage;
    }
    
    /// <summary>
    /// Maps a generic Result object to a ResultGrpcMessage object.
    /// </summary>
    /// <typeparam name="T">The type of the Result object.</typeparam>
    /// <param name="result">The Result object to be mapped.</param>
    /// <returns>A ResultGrpcMessage object representing the mapped Result object.</returns>
    public static ResultFactory<T> Map<T>(this Result<T> result)
    {
        var resultMessage = new ResultGrpcMessage()
        {
            Succeeded = result.Succeeded
        };
    
        if (result.Failed) resultMessage.FailureReasons.AddRange(result.FailureDetails.FailureReasons);

        return new ResultFactory<T>(result);
    }
}