using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Telegramma.Sender.Core.ExceptionHandling
{
    public class ExceptionHandlingInterceptor : Interceptor
    {
        private readonly IExceptionHandler[] _exceptionHandlers;

        public ExceptionHandlingInterceptor(IServiceProvider serviceProvider)
        {
            _exceptionHandlers = serviceProvider.GetServices<IExceptionHandler>().ToArray();
        }
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception e)
            {
                HandleException(e);
                throw;
            }
        }

        public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                await continuation(request, responseStream, context);
            }
            catch (Exception e)
            {
                HandleException(e);
                throw;
            }
        }

        public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            ServerCallContext context,
            ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(requestStream, context);
            }
            catch (Exception e)
            {
                HandleException(e);
                throw;
            }
        }

        public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                await continuation(requestStream, responseStream, context);
            }
            catch (Exception e)
            {
                HandleException(e);
                throw;
            }
        }

        private void HandleException(Exception e)
        {
            foreach (var exceptionHandler in _exceptionHandlers)
            {
                var handled = exceptionHandler.Handle(e);
                if (handled == null)
                    continue;
                throw new RpcException(new Status(handled.StatusCode, string.Join(",", handled.Errors)));
            }
        }
    }
}
