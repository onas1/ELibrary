using ELibrary.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.MVC.Controllers
{
    public class ErrorController : Controller
    {

        [Route("/Error/{statusCode}")]
        public IActionResult PageNotFoundHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    var statusDetails = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    var path = statusDetails.OriginalPath;
                    var qString = statusDetails.OriginalQueryString;
                    // Todo: log error to file
                  
                    break;

                case 401:
                    var statusDetail = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    var paths = statusDetail.OriginalPath;
                    var currentPath = UrlHelper.CreateUrl("Account/Login", HttpContext);
                    var returnUrl = $"{currentPath}?returnUrl={paths}/{statusDetail.OriginalQueryString}";
                    return Redirect(returnUrl);




            }
            return View("NotFound");
        }


        [Route("/Error")]
        public IActionResult ExceptionHandler(int statusCode)
        {
            var errorDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var path = errorDetails.Path;
            var err = errorDetails.Error;
            // Todo: log to file
         
            return View("Error");
        }

        public IActionResult AccessDeniedHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    var statusDetails = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    var path = statusDetails.OriginalPath;
                    var qString = statusDetails.OriginalQueryString;
                    // Todo: log error to file
                    
                    break;
            }
            return View("AccessDenied");
        }
    }
}
