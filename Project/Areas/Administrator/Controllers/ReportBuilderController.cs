using Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    [Infrastructure.SyncPermission]
    public partial class ReportBuilderController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public virtual ActionResult Index()
        {
            try
            {
                var reports = GetServiceReport().GetReports()
               .ToList()
               .Select(x => new ViewModels.ComboboxItemGuid
               {
                   Id = x.Id,
                   Name = x.Name
               })
               .ToList()
               ;

                return View(reports);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static QueryBuilder.Service.Common.IService GetServiceReport()
        {
            try
            {



                // connect to report server
                QueryBuilder.Service.Common.LoginData LoginData = new QueryBuilder.Service.Common.LoginData
                {
                    Username = "admin",
                    Password = "@admin",
                    Identifier = "6AB71CE2-BA1A-44D3-AB81-C3573F4C5045",
                    Version = "1.0.0.1"
                };
                QueryBuilder.Service.Common.IService service = QueryBuilder.Service.Client.Factory.GetService();
                service.Login(LoginData);
                return service;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        [HttpPost]
        public virtual ActionResult GetParamReport(System.Guid reportId)
        {
            IList<ViewModels.Areas.Administrator.ReportGenerator.ReportParameter> reportParameters;

            reportParameters = GetServiceReport().GetReportParameters(reportId)
                   .Select(x => new ViewModels.Areas.Administrator.ReportGenerator.ReportParameter
                   {
                       DataType = GetDataType(x.DataType),
                       DisplayName = x.DisplayName,
                       Name = x.Name,
                       Tag = x.Tag
                   })
                   .ToList();
            ViewBag.ReportId = reportId;
            return PartialView(reportParameters);
        }

        internal static DataType GetDataType(QueryBuilder.Common.ParameterDataType dataType)
        {
            switch (dataType)
            {
                case QueryBuilder.Common.ParameterDataType.Number:
                    return DataType.Int;

                case QueryBuilder.Common.ParameterDataType.String:
                    return DataType.String;
                case QueryBuilder.Common.ParameterDataType.DateTime:
                    return DataType.Date;
                default:
                    return DataType.String;
            }
        }

        [HttpPost]
        public virtual ActionResult IndexDataReport(System.Guid reportId, FormCollection formCollection)
        {
            IList<ViewModels.Areas.Administrator.ReportGenerator.ReportParameter> reportParameters;

            reportParameters = GetServiceReport().GetReportParameters(reportId)
                   .Select(x => new ViewModels.Areas.Administrator.ReportGenerator.ReportParameter
                   {
                       DataType = GetDataType(x.DataType),
                       DisplayName = x.DisplayName,
                       Name = x.Name,
                       Tag = x.Tag
                   })
                   .ToList();

            var reportParameterValues =
                       Infrastructure.Utility.GetReportParameterValues(reportParameters, formCollection);

            ViewBag.ReportParameterValues = reportParameterValues;
            ViewBag.ReportId = reportId;

            int totalPage;

            var reportData = GetDataReports(reportId, reportParameterValues, out totalPage);

            TempData[reportId.ToString()] = reportData;

            var fieldcolumns = Infrastructure.Utility.GetFieldColumns(reportData.DataTable);

            ViewBag.TotalPage = totalPage;

            ViewBag.Fieldcolumns = fieldcolumns;
            ViewBag.FormCollection = formCollection;
            return PartialView(reportParameters);
        }
        public ViewModels.Areas.Administrator.ReportGenerator.ReportingResult GetDataReports(Guid reportId,
            List<ViewModels.Areas.Administrator.ReportGenerator.ReportParameterValue> viewParameterValues, out int totalPage, int mymaxRecord = 0)
        {
            using (var dbContext = new Models.DatabaseContext())
            {
                var post = new Common.KendoGridPost();
                if (mymaxRecord > 0) { post.PageSize = mymaxRecord; };
                List<QueryBuilder.Service.Common.ParameterValue> parameterValues =
                    new List<QueryBuilder.Service.Common.ParameterValue>();

                viewParameterValues.ForEach(x =>
                {
                    parameterValues.Add(new QueryBuilder.Service.Common.ParameterValue
                    {
                        ParameterName = x.ParameterName,
                        Value = x.Value
                    });
                });

                var startRecord = (post.Page - 1) * post.PageSize;
                try
                {

                    totalPage = GetServiceReport().ExecuteReportResultCount(reportId, parameterValues);
                    var maxRecord = post.PageSize;
                    if (mymaxRecord > 0) { maxRecord = totalPage; }

                    var reportResult = GetServiceReport().ExecuteReport(reportId, parameterValues, startRecord, maxRecord);

                    return new ViewModels.Areas.Administrator.ReportGenerator.ReportingResult
                    {
                        DataTable = reportResult.DataTable,
                        ColumnInfos = reportResult.ColumnInfos.Select(x => new ViewModels.Areas.Administrator.ReportGenerator.ReportColumnInfo
                        {
                            AggregateFunction = GetReportAggregateFunction(x.AggregateFunction),
                            Alias = x.Alias,
                            Column = x.Column,
                            FooterAggregateFunctions = x.FooterAggregateFunctions.Select(y => GetReportAggregateFunction(y)).ToList(),
                            Order = x.Order,
                            Table = x.Table,
                            UniqueName = x.UniqueName
                        }).ToList()
                    };
                }
                catch (Exception ex)
                {
                    totalPage = 0;
                    return null;
                }
            }
        }
        internal static AggregateFunction GetReportAggregateFunction(QueryBuilder.Common.AggregateFunction aggregateFunction)
        {
            switch (aggregateFunction)
            {
                case QueryBuilder.Common.AggregateFunction.Sum:
                    return AggregateFunction.Sum;
                case QueryBuilder.Common.AggregateFunction.Average:
                    return AggregateFunction.Average;
                case QueryBuilder.Common.AggregateFunction.Min:
                    return AggregateFunction.Min;
                case QueryBuilder.Common.AggregateFunction.Max:
                    return AggregateFunction.Max;
                case QueryBuilder.Common.AggregateFunction.Count:
                    return AggregateFunction.Count;
                case QueryBuilder.Common.AggregateFunction.DistinctCount:
                    return AggregateFunction.DistinctCount;
                default:
                    return AggregateFunction.None;
            }
        }
        [HttpPost]
        public virtual ActionResult GetDataReports(System.Guid reportId, FormCollection formCollection, int totalPage)
        {
            if (TempData[reportId.ToString()] != null)
            {
                var reportData = TempData[reportId.ToString()] as ViewModels.Areas.Administrator.ReportGenerator.ReportingResult;

                TempData.Remove(reportId.ToString());

                var items = Infrastructure.Utility.DataTableToJSONWithJavaScriptSerializer(reportData.DataTable);

                Infrastructure.Kendo.KendoGridResult<object> kendos =
                    new Infrastructure.Kendo.KendoGridResult<object>
                    {
                        Items = items,
                        TotalCount = totalPage
                    };

                return (Json(kendos, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }

            IList<ViewModels.Areas.Administrator.ReportGenerator.ReportParameter> reportParameters;
            if (GetParamReports(reportId, out reportParameters))
            {
                var reportParameterValues =
                    Infrastructure.Utility.GetReportParameterValues(reportParameters, formCollection, true);

                ViewBag.ReportId = reportId;

                var reportData = GetDataReports(reportId, reportParameterValues, out totalPage);

                var items = Infrastructure.Utility.DataTableToJSONWithJavaScriptSerializer(reportData.DataTable);

                Infrastructure.Kendo.KendoGridResult<object> kendos =
                    new Infrastructure.Kendo.KendoGridResult<object>
                    {
                        Items = items,
                        TotalCount = totalPage
                    };

                return (Json(kendos, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }

            return null;
        }
        public bool GetParamReports(Guid reportId, out IList<ViewModels.Areas.Administrator.ReportGenerator.ReportParameter> reportParameters)
        {

            reportParameters = GetServiceReport().GetReportParameters(reportId)
                   .Select(x => new ViewModels.Areas.Administrator.ReportGenerator.ReportParameter
                   {
                       DataType = GetDataType(x.DataType),
                       DisplayName = x.DisplayName,
                       Name = x.Name,
                       Tag = x.Tag
                   })
                   .ToList();
            return true;

        }
        [HttpGet]
        public virtual ActionResult GetDataReports()
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            var lst = new List<object>();
            for (int i = 0; i < 4; i++)
            {
                var str = "{\"Key\":\"Ali" + i + "\",\"Value\":\"50\"}";

                var rrr = js.DeserializeObject(str);

                lst.Add(rrr);
            }

            Infrastructure.Kendo.KendoGridResult<object> kendos =
                new Infrastructure.Kendo.KendoGridResult<object>
                {
                    Items = lst,
                    TotalCount = 100
                };

            return (Json(kendos, System.Web.Mvc.JsonRequestBehavior.AllowGet));
        }
        public virtual ActionResult DownloadReportFile()
        {
            try
            {
                if (Session["excelExport"] != null)
                {
                    var excel2 = Session["excelExport"] as OpenXml.ObjectsToXls;
                    Session.Remove("excelExport");
                    excel2.WriteToHttpResponse("Export.xlsx");
                    string fullPath = Path.Combine(Server.MapPath("~/temp/"), "Export.xlsx");
                    return File(fullPath, "application/vnd.ms-excel", "Export.xlsx");
                }
                else
                {
                    Response.End();
                    return null;
                    throw new Exception(string.Format("{0} not found"));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public virtual JsonResult ExportToExcel(System.Guid reportId, FormCollection formCollection)
        {

            IList<ViewModels.Areas.Administrator.ReportGenerator.ReportParameter> reportParameters;
            if (GetParamReports(reportId, out reportParameters))
            {
                var post = new Common.KendoGridPost();
                var reportParameterValue = GetServiceReport().GetReportParameters(reportId)
                    .Select(x => new ViewModels.Areas.Administrator.ReportGenerator.ReportParameter
                    {
                        DataType = GetDataType(x.DataType),
                        DisplayName = x.DisplayName,
                        Name = x.Name,
                        Tag = x.Tag
                    })
                    .ToList();
                var reportParameterValues =
                    Infrastructure.Utility.GetReportParameterValues(reportParameterValue, formCollection, true);

                ViewBag.ReportId = reportId;
                int totalPage;
                var reportData = GetDataReports(reportId, reportParameterValues, out totalPage, 5);
                OpenXml.ObjectsToXls excel = new OpenXml.ObjectsToXls();
                excel.DataTableToJSONWithJavaScriptSerializer(reportData.DataTable, "reportResult");
                excel.WriteToHttpResponse("Export.xlsx");
                return Json(true, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

            return Json(false, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }


    }
}