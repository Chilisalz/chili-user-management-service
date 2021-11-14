﻿using Microsoft.AspNetCore.Http;
using System.Linq;

namespace UserManagementService.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                return string.Empty;
            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
