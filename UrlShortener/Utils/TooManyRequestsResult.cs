using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Utils
{
    public class TooManyRequestsResult : ObjectResult
    {
        public TooManyRequestsResult(object? value) : base(value)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 429;
            return base.ExecuteResultAsync(context);
        }
    }
}
