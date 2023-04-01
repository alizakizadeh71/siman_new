using System.Web.Mvc;

namespace OPS.Controllers
{
    public partial class ErrorController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ErrorController()
        { }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult BadRequest()
        {
            return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual System.Web.Mvc.ActionResult AccessDenied()
        {
            return (View());
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual System.Web.Mvc.ActionResult InvalidRequest()
        {
            return (View());
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult Forbidden()
        {
            return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual System.Web.Mvc.ViewResult Display(System.Net.HttpStatusCode? code)
        {
            //if (code == null)
            //{
            //    code = System.Net.HttpStatusCode.BadRequest;
            //}

            switch (code.Value)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    {
                        ViewBag.Message = Resources.Message.Global.BadRequest;
                        break;
                    }

                case System.Net.HttpStatusCode.Forbidden:
                    {
                        ViewBag.Message = Resources.Message.Global.Forbidden;
                        break;
                    }

                case System.Net.HttpStatusCode.NotFound:
                    {
                        ViewBag.Message = Resources.Message.Global.NotFound;
                        break;
                    }

                case System.Net.HttpStatusCode.TemporaryRedirect:
                    {
                        ViewBag.Message = Resources.Message.Global.TemporaryRedirect;
                        break;
                    }

                case System.Net.HttpStatusCode.Unauthorized:
                    {
                        ViewBag.Message = Resources.Message.Global.Unauthorized;
                        break;
                    }

                default:
                    {
                        ViewBag.Message = code.ToString();

                        break;
                    }
            }

            return (View());
        }

        public virtual System.Web.Mvc.ViewResult Display(System.Net.HttpStatusCode? code, int lincCode)
        {
            switch (code.Value)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    {
                        ViewBag.Message = Resources.Message.Global.BadRequest + " ErrorCode " + lincCode.ToString();
                        break;
                    }

                case System.Net.HttpStatusCode.Forbidden:
                    {
                        ViewBag.Message = Resources.Message.Global.Forbidden + " ErrorCode " + lincCode.ToString();
                        break;
                    }

                case System.Net.HttpStatusCode.NotFound:
                    {
                        ViewBag.Message = Resources.Message.Global.NotFound + " ErrorCode " + lincCode.ToString();
                        break;
                    }

                case System.Net.HttpStatusCode.TemporaryRedirect:
                    {
                        ViewBag.Message = Resources.Message.Global.TemporaryRedirect + " ErrorCode " + lincCode.ToString();
                        break;
                    }

                case System.Net.HttpStatusCode.Unauthorized:
                    {
                        ViewBag.Message = Resources.Message.Global.Unauthorized + " ErrorCode " + lincCode.ToString();
                        break;
                    }

                default:
                    {
                        ViewBag.Message = code.ToString() + " ErrorCode " + lincCode.ToString();

                        break;
                    }
            }

            return (View());
        }

        public virtual System.Web.Mvc.ViewResult Display(string exsption, int lincCode)
        {
            ViewBag.Message = exsption + " ErrorCode " + lincCode.ToString();
            return (View());
        }
    }
}