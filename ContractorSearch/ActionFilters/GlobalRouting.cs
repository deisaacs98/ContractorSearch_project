using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContractorSearch.ActionFilters
{

    public class GlobalRouting : IActionFilter
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        public GlobalRouting(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            if (controller.Equals("Home"))
            {
                if (_claimsPrincipal.IsInRole("Customer"))
                {
                    context.Result = new RedirectToActionResult("Index",
                    "Customer", null);
                }
                else if (_claimsPrincipal.IsInRole("Contractor"))
                {
                    context.Result = new RedirectToActionResult("Index",
                    "Contractor", null);
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
