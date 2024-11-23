using System;
using System.Linq;

namespace Truss.Monads.Results.Protobuf;

/// <summary>
/// Extensions for mapping messages from grpc to Result types
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// Maps from grpc to a nil result
    /// </summary>
    /// <param name="resultGrpcMessage"></param>
    /// <typeparam name="TGrpc"></typeparam>
    /// <returns></returns>
    public static Result<Nil> MapFromGrpc<TGrpc>(
        this global::Results.Protobuf.Result resultGrpcMessage
    )
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
    public static Result<TOut> MapFromGrpc<TGrpc, TOut>(
        this global::Results.Protobuf.Result resultGrpcMessage,
        TGrpc grpcMessage,
        Func<TGrpc, TOut> responseMapper
    )
    {
        if (resultGrpcMessage.Succeeded)
        {
            return Result.Success(responseMapper(grpcMessage));
        }

        return Result.Fail(resultGrpcMessage.FailureReasons.ToArray());
    }

    /// <summary>
    /// Maps a generic Result object to a global::Results.Protobuf.Result object.
    /// </summary>
    /// <typeparam name="T">The type of the Result object.</typeparam>
    /// <param name="result">The Result object to be mapped.</param>
    /// <returns>A global::Results.Protobuf.Result object representing the mapped Result object.</returns>
    internal static global::Results.Protobuf.Result MapToGrpc<T>(
        this Result<T> result
    )
    {
        var resultMessage = new global::Results.Protobuf.Result()
        {
            Succeeded = result.Succeeded,
        };

        if (result.Failed)
        {
            resultMessage.FailureReasons.AddRange(
                result.FailureDetails.FailureReasons
            );
        }

        return resultMessage;
    }

    /// <summary>
    /// Maps a generic Result object to a global::Results.Protobuf.Result object.
    /// </summary>
    /// <typeparam name="T">The type of the Result object.</typeparam>
    /// <param name="result">The Result object to be mapped.</param>
    /// <returns>A global::Results.Protobuf.Result object representing the mapped Result object.</returns>
    public static ResultFactory<T> Map<T>(this Result<T> result)
    {
        var resultMessage = new global::Results.Protobuf.Result()
        {
            Succeeded = result.Succeeded,
        };

        if (result.Failed)
        {
            resultMessage.FailureReasons.AddRange(
                result.FailureDetails.FailureReasons
            );
        }

        return new ResultFactory<T>(result);
    }
}