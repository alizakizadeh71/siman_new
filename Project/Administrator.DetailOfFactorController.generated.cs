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
namespace OPS.Areas.Administrator.Controllers
{
    public partial class DetailOfFactorController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DetailOfFactorController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected DetailOfFactorController(Dummy d) { }

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
        public System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.JsonResult GetDetailOfFactors()
        {
            return new T4MVC_JsonResult(Area, Name, ActionNames.GetDetailOfFactors);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Create()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Create);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Details()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Details);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Edit()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Delete()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult DeleteConfirmed()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.DeleteConfirmed);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult PrintFactor()
        {
            return new T4MVC_ActionResult(Area, Name, ActionNames.PrintFactor);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DetailOfFactorController Actions { get { return MVC.Administrator.DetailOfFactor; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Administrator";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "DetailOfFactor";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "DetailOfFactor";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string GetDetailOfFactors = "GetDetailOfFactors";
            public readonly string Create = "Create";
            public readonly string Details = "Details";
            public readonly string Edit = "Edit";
            public readonly string Delete = "Delete";
            public readonly string DeleteConfirmed = "Delete";
            public readonly string PrintFactor = "PrintFactor";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string GetDetailOfFactors = "GetDetailOfFactors";
            public const string Create = "Create";
            public const string Details = "Details";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
            public const string DeleteConfirmed = "Delete";
            public const string PrintFactor = "PrintFactor";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string headoffactorid = "headoffactorid";
        }
        static readonly ActionParamsClass_GetDetailOfFactors s_params_GetDetailOfFactors = new ActionParamsClass_GetDetailOfFactors();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetDetailOfFactors GetDetailOfFactorsParams { get { return s_params_GetDetailOfFactors; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetDetailOfFactors
        {
            public readonly string headoffactorid = "headoffactorid";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string headoffactorid = "headoffactorid";
            public readonly string detailOfFactor = "detailOfFactor";
        }
        static readonly ActionParamsClass_Details s_params_Details = new ActionParamsClass_Details();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Details DetailsParams { get { return s_params_Details; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Details
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string id = "id";
            public readonly string detailOfFactor = "detailOfFactor";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_DeleteConfirmed s_params_DeleteConfirmed = new ActionParamsClass_DeleteConfirmed();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteConfirmed DeleteConfirmedParams { get { return s_params_DeleteConfirmed; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteConfirmed
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_PrintFactor s_params_PrintFactor = new ActionParamsClass_PrintFactor();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_PrintFactor PrintFactorParams { get { return s_params_PrintFactor; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_PrintFactor
        {
            public readonly string headoffactorid = "headoffactorid";
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
                public readonly string Create = "Create";
                public readonly string Delete = "Delete";
                public readonly string Details = "Details";
                public readonly string Edit = "Edit";
                public readonly string Index = "Index";
                public readonly string PrintFactor = "PrintFactor";
            }
            public readonly string Create = "~/Areas/Administrator/Views/DetailOfFactor/Create.cshtml";
            public readonly string Delete = "~/Areas/Administrator/Views/DetailOfFactor/Delete.cshtml";
            public readonly string Details = "~/Areas/Administrator/Views/DetailOfFactor/Details.cshtml";
            public readonly string Edit = "~/Areas/Administrator/Views/DetailOfFactor/Edit.cshtml";
            public readonly string Index = "~/Areas/Administrator/Views/DetailOfFactor/Index.cshtml";
            public readonly string PrintFactor = "~/Areas/Administrator/Views/DetailOfFactor/PrintFactor.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_DetailOfFactorController : OPS.Areas.Administrator.Controllers.DetailOfFactorController
    {
        public T4MVC_DetailOfFactorController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index(System.Guid headoffactorid)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "headoffactorid", headoffactorid);
            return callInfo;
        }

        public override System.Web.Mvc.JsonResult GetDetailOfFactors(System.Guid headoffactorid)
        {
            var callInfo = new T4MVC_JsonResult(Area, Name, ActionNames.GetDetailOfFactors);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "headoffactorid", headoffactorid);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Create(System.Guid headoffactorid)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "headoffactorid", headoffactorid);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.DetailOfFactor.CreateViewModel detailOfFactor)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "detailOfFactor", detailOfFactor);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Details(System.Guid id)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Details);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.DetailOfFactor.EditViewModel detailOfFactor)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "detailOfFactor", detailOfFactor);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Delete(System.Guid id)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.DeleteConfirmed);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult PrintFactor(System.Guid headoffactorid)
        {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.PrintFactor);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "headoffactorid", headoffactorid);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
