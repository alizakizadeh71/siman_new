// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace OPS.Controllers
{
    public partial class ParsianPGWSalePaymentController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ParsianPGWSalePaymentController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ParsianPGWSalePaymentController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult PaymentCallback()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.PaymentCallback);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ParsianPGWSalePaymentController Actions { get { return MVC.ParsianPGWSalePayment; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ParsianPGWSalePayment";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ParsianPGWSalePayment";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string PaymentCallback = "PaymentCallback";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string PaymentCallback = "PaymentCallback";
        }


        static readonly ActionParamsClass_PaymentCallback s_params_PaymentCallback = new ActionParamsClass_PaymentCallback();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_PaymentCallback PaymentCallbackParams { get { return s_params_PaymentCallback; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_PaymentCallback
        {
            public readonly string model = "model";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_ParsianPGWSalePaymentController : OPS.Controllers.ParsianPGWSalePaymentController
    {
        public T4MVC_ParsianPGWSalePaymentController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult PaymentCallback(OPS.Parsian.PaymentCallbackModel model)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.PaymentCallback);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591