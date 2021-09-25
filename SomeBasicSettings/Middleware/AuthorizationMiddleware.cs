using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicSettings.Middleware
{
    public class AuthorizationMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<AuthorizationMiddleware> logger;

        public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            if (context.IsHttpTriggerFunction())
            {
                string functionEntryPoint = context.FunctionDefinition.EntryPoint;
                Type assemblyType = Type.GetType(functionEntryPoint.Substring(0, functionEntryPoint.LastIndexOf('.')));
                MethodInfo methodInfo = assemblyType.GetMethod(functionEntryPoint.Substring(functionEntryPoint.LastIndexOf('.') + 1));
                if (methodInfo.GetCustomAttribute(typeof(FunctionAuthorizeAttribute), false) is FunctionAuthorizeAttribute functionAuthorizeAttribute)
                {
                    if (context.Items.ContainsKey("UserRole") && context.Items["UserRole"] != null)
                    {
                        var role = context.Items["UserRole"].ToString();
                        if (!functionAuthorizeAttribute.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                        {
                            await context.CreateJsonResponse(System.Net.HttpStatusCode.Forbidden, new { Message = "Forbidden Access." });
                        }
                        else
                        {
                            await next(context);
                        }
                    }
                    else
                    {
                        await context.CreateJsonResponse(System.Net.HttpStatusCode.Forbidden, new { Message = "Forbidden Access." });
                    }
                }
            }
            else
            {
                await next(context);
            }
        }
    }

public class SimpleMiddleware :
    IFunctionsWorkerMiddleware
{
    public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        //TODO: logic goes here.
        return next(context);
    }
}
}
