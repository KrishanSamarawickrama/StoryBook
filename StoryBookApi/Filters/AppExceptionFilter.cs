using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace StoryBookApi.Filters
{
    public class AppExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<AppExceptionFilter> logger;

        public AppExceptionFilter(ILogger<AppExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception,context.Exception.Message);

            base.OnException(context);
        }
    }
}
