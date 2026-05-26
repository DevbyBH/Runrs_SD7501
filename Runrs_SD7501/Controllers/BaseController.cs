using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Runrs_SD7501.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                context.Result = new RedirectToActionResult(
                    "Index",
                    "Login",
                    null);
            }

            base.OnActionExecuting(context);
        }
    }
}