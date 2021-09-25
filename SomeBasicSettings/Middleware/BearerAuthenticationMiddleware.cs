using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicSettings.Middleware
{
    /// <summary>
    /// BearerAuthentication Middleware.
    /// This is simple representation of bearer implementation but 
    /// for production it required actual code.
    /// </summary>
    public class BearerAuthenticationMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<BearerAuthenticationMiddleware> logger;

        public BearerAuthenticationMiddleware(ILogger<BearerAuthenticationMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            if (context.IsHttpTriggerFunction())
            {
                var headers = context.BindingContext.BindingData["Headers"]?.ToString();
                var httpHeaders = System.Text.Json.JsonSerializer.Deserialize<HttpHeaders>(headers);
                if (httpHeaders?.Authorization != null &&
                    httpHeaders.Authorization.StartsWith("Bearer"))
                {
                    //Validation logic for your token. Here If Bearer present I consider as Valid.
                    if (httpHeaders.Authorization.Contains("admin"))
                    {
                        context.Items.Add("UserRole", "Admin");
                    }
                    await next(context);
                }
                else
                {
                    await context.CreateJsonResponse(System.Net.HttpStatusCode.Unauthorized, new { Message = "Token is not valid." });
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
