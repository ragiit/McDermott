namespace McDermott.TelemedicianApi.Middleware
{
    public class ApiHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiHeaderMiddleware(RequestDelegate next)=> _next = next;

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.Add("X-cons-id", "743627386");
            context.Request.Headers.Add("X-timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            context.Request.Headers.Add("X-signature", "QXJnaSBQdXJ3YW50byBXYWh5dSBzYW5ha2kgRHdpIEx1Y2t5");
            context.Request.Headers.Add("user_key", "s3l3m84R<4!|V<3H!DLip4N");

            await _next(context);
        }
    }
}
