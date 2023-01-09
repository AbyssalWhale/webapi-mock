using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace webapi_mock.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string _apiKeyNameInConfigFile = "ApiKey";
        private string _apiKeyInRequest;

        public ApiKeyAttribute(string headerName) 
        {
            _apiKeyInRequest = headerName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKeyInRequest, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Authorization key or header has not been provided"
                };

                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(_apiKeyNameInConfigFile);

            if (apiKey is null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api key is not found in solution"
                };

                return;
            }

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Authorization key or header has not been provided"
                };

                return;
            }

            await next();
        }
    }
}
