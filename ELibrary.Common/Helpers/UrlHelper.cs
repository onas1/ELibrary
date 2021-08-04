using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Common.Helpers
{
    public class UrlHelper
    {
        public static string BaseAddress(HttpContext context)
        {
            return context.Request.Scheme + "://" + context.Request.Host;
        }

        public static string BaseAddress(HttpContext context, string path)
        {
            return context.Request.Scheme + "://" + path;
        }

        //Create a url given the url Path
        public static string CreateUrl(string urlPath, HttpContext context)
        {
            return string.Join('/', BaseAddress(context), urlPath);
        }

        public static string CreateUrl(string urlPath, string url, HttpContext context)
        {
            return string.Join('/', BaseAddress(context, url), urlPath);
        }

        //generate link to be embeded in the emails
        public static string GetEmailLink(Dictionary<string, string> queryParams, string urlPath, HttpContext context)
        {
            var path = urlPath.StartsWith('/') ? urlPath.Substring(1) : urlPath;
            var baseUrl = CreateUrl(path, context);
            //construct the account confirmation link
            return QueryHelpers.AddQueryString(baseUrl, queryParams);
        }
    }
}
