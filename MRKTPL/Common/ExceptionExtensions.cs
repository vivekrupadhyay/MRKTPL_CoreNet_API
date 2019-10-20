using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using MRKTPL.Data.ViewModel;
using MRKTPL.LoggerService;

namespace MRKTPL.Common
{
    public static class ExceptionExtensions
    {
        //public static IApplicationBuilder CustomExceptionMiddleware(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<ExceptionMiddleware>();
        //}
        public static IApplicationBuilder CustomExceptionMiddleware(this IApplicationBuilder app, ErrorLoggerManager errorLogger, IOptions<SendMailSettings> emailSetting)
        {
            return app.UseMiddleware<ExceptionMiddleware>(errorLogger,emailSetting);
        }
    }
}
