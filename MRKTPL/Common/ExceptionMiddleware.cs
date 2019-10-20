using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MRKTPL.Data.ViewModel;
using MRKTPL.LoggerService;
using MRKTPL.Services.ErrorLoggerServices;
using System;
using System.Net;
using System.Threading.Tasks;


namespace MRKTPL.Common
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private Logger logger;
        private readonly ErrorLoggerManager _errorLogger;
        private readonly IOptions<SendMailSettings> _emailSetting;

        public ExceptionMiddleware(RequestDelegate next,  ErrorLoggerManager errorLogger, IOptions<SendMailSettings> emailSetting)
        {
            _errorLogger = errorLogger;
            _next = next;
            _emailSetting = emailSetting;
            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            logger = new Logger
            {
                StracTrace = exception.StackTrace,
                ClassName = exception.Source,
                Message = exception.Message,
                StatusCode = context.Response.StatusCode
            };
            _errorLogger.LogError();
            logger.ErrorLog(exception);

            return context.Response.WriteAsync(logger.ToString());
        }
    }
}
