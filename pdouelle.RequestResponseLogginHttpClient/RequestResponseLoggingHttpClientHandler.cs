using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace pdouelle.RequestResponseLoggingHttpClient
{
    public class RequestResponseLoggingHttpClientHandler : DelegatingHandler
    {
        private readonly ILogger<RequestResponseLoggingHttpClientHandler> _logger;

        public RequestResponseLoggingHttpClientHandler(ILogger<RequestResponseLoggingHttpClientHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await LogRequest(request);

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            await LogResponse(response);

            return response;
        }

        private async Task LogRequest(HttpRequestMessage request)
        {
            string content = null;

            if (request.Content != null)
                content = await request.Content.ReadAsStringAsync();

            _logger.LogDebug($"Http Request Information: {request} Response Body: {content}");
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            string content = null;

            if (response?.Content != null)
                content = await response.Content.ReadAsStringAsync();

            _logger.LogDebug($"Http Response Information: {response} Response Body: {content}");
        }
    }
}