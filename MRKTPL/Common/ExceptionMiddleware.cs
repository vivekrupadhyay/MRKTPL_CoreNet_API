using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MRKTPL.Data.ViewModel;
using MRKTPL.LoggerService;
using MRKTPL.Services.ErrorLoggerServices;

namespace MRKTPL.Common {
    public class ExceptionMiddleware {
        private readonly RequestDelegate _next;
        private ILogger logger;
        // private readonly ErrorLoggerManager _errorLogger;
        // private readonly IOptions<SendMailSettings> _emailSetting;

        public ExceptionMiddleware (RequestDelegate next, ILogger<ExceptionMiddleware> _logger) //ErrorLoggerManager errorLogger, IOptions<SendMailSettings> emailSetting)
        {
            logger = _logger;
            _next = next;
            // _emailSetting = emailSetting;

        }

        public async Task InvokeAsync (HttpContext httpContext) {
            try {
                await _next (httpContext);
            } catch (Exception ex) {
                logger.LogCritical ($"Unhandled exception: {ex.Message} {Environment.NewLine}Stacktrace: {ex.StackTrace}", ex);
                await HandleExceptionAsync (httpContext, ex);
            }
        }

        private Task HandleExceptionAsync (HttpContext context, Exception exception) {
            var code = HttpStatusCode.InternalServerError;
            var response = new InternalServerError {
                HttpStatusCode = (int) code,
                UserMessage = "Internal Server Error",
                MoreInfo = "An unexpected error has occured.",
                DeveloperMessage = "Detailed information about the error has been logged.",
                Errors = new List<ErrorResponse> (),
                IsError = true,
                RequestId = context.TraceIdentifier
            };
            var result = JsonSerializer.Serialize (response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync (result);
            // context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            // logger = new Logger {
            //     StracTrace = exception.StackTrace,
            //     ClassName = exception.Source,
            //     Message = exception.Message,
            //     StatusCode = context.Response.StatusCode
            // };
            // _errorLogger.LogError ();
            // logger.ErrorLog (exception);

            // return context.Response.WriteAsync (logger.ToString ());
        }
    }
}