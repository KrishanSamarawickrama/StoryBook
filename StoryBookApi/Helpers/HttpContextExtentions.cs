using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace StoryBookApi.Helpers
{
    public static class HttpContextExtentions
    {
        public static void InsertParametersPaginationInHeader<T>(this HttpContext httpContext, IEnumerable<T> list)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            double count = list.Count();
            httpContext.Response.Headers.Add("noOfRecords", count.ToString());
        }
    }
}
