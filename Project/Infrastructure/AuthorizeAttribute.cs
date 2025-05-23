using DAL;
using System.Linq;

namespace Infrastructure
{
    public class AuthorizeAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public AuthorizeAttribute()
        { }

        private void RedirectToRouteResult
            (System.Web.Mvc.ActionExecutingContext filterContext,
            string areaNameBase, string controllerNameBase, string actionNameBase,
            System.Collections.Generic.IDictionary<string, object> actionParameters = null)
        {
            return;
            if (actionParameters != null)
            {
                foreach (var varItem in actionParameters)
                {
                    filterContext.ActionParameters.Add(varItem);
                }
            }

            filterContext.Result =
                new System.Web.Mvc.RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                    {
                        { "area", areaNameBase },
                        { "controller", controllerNameBase },
                        { "action", actionNameBase }
                    });
        }

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            return;
            base.OnActionExecuting(filterContext);

            string strAreaName = string.Empty;
            string strControllerName = string.Empty;
            string strActionName = string.Empty;
            string strReturnUrl = string.Empty;

            object objArea = filterContext.RouteData.DataTokens["area"];
            if (objArea != null)
            {
                strAreaName = objArea.ToString().Trim();
            }

            object objController = filterContext.RouteData.Values["controller"];
            if ((objController == null) || (objController.ToString().Trim() == string.Empty))
            {
                RedirectToRouteResult
                    (
                    areaNameBase: string.Empty,
                    controllerNameBase: "Error",
                    actionNameBase: "BadRequest",
                    filterContext: filterContext
                    );

                return;
            }
            strControllerName = objController.ToString().Trim();


            object objAction = filterContext.RouteData.Values["action"];
            if ((objAction == null) || (objAction.ToString().Trim() == string.Empty))
            {
                RedirectToRouteResult
                    (
                    areaNameBase: string.Empty,
                    controllerNameBase: "Error",
                    actionNameBase: "BadRequest",
                    filterContext: filterContext
                    );

                return;
            }
            strActionName = objAction.ToString().Trim();

            if (string.IsNullOrEmpty(strAreaName))
            {
                strReturnUrl = string.Format("/{0}/{1}", strControllerName, strActionName);
            }
            else
            {
                strReturnUrl = string.Format("/{0}/{1}/{2}", strAreaName, strControllerName, strActionName);
            }

            // **************************************************
            // **************************************************
            // **************************************************

            if (Infrastructure.AuthenticatedUser.IsAuthenticated)
            {
                switch (Infrastructure.Sessions.AuthenticatedUser.Role)
                {
                    case Enums.Roles.Programmer:
                        {
                            return;
                        }
                }
            }


            using (var oUnitOfWork = new UnitOfWork())
            {
                Models.ProjectAction oAction
                    = oUnitOfWork
                    .ProjectActionRepository
                    .GetAction(strAreaName, strControllerName, strActionName)
                    ;

                if (oAction == null)
                {
                    RedirectToRouteResult
                        (
                            areaNameBase: string.Empty,
                            controllerNameBase: "Error",
                            actionNameBase: "InvalidRequest",
                            filterContext: filterContext
                        );

                    return;
                }

                if (oAction.IsPublic)
                {
                    return;
                }

                if (Infrastructure.AuthenticatedUser.IsAuthenticated == false)
                {
                    RedirectToRouteResult
                        (
                            areaNameBase: string.Empty,
                            controllerNameBase: "Account",
                            actionNameBase: "Login",
                            filterContext: filterContext
                        );

                    return;
                }

                if ((Infrastructure.AuthenticatedUser.IsAuthenticated) &&
                    (Infrastructure.Sessions.AuthenticatedUser.Role == Enums.Roles.Programmer))
                {
                    return;
                }

                Models.Role oUserRole = Infrastructure.Sessions.AuthenticatedUser.User.Role; 
                var asasas = oAction.Roles.Select(current => current.Id).ToList();
                if (!oAction.Roles.Select(current => current.Id).Contains(oUserRole.Id))
                {
                    RedirectToRouteResult
                        (
                            areaNameBase: string.Empty,
                            controllerNameBase: "Error",
                            actionNameBase: "AccessDenied",
                            filterContext: filterContext
                        );

                    return;
                }
            }
        }

        public override void OnActionExecuted
            (System.Web.Mvc.ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting
            (System.Web.Mvc.ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted
            (System.Web.Mvc.ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

    }
}