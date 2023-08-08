using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Areas.Administrator.ReportGenerator;

namespace Infrastructure
{
    public static class Utility
    {
        static Utility()
        { }

        public static string CultureName
        {
            get
            {
                return (System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            }
        }

        public static string GetAreaName()
        {
            if ((System.Web.HttpContext.Current == null) ||
                (System.Web.HttpContext.Current.Request == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData.DataTokens == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData.DataTokens.Count == 0))
            {
                return (string.Empty);
            }

            object objAreaName =
                System.Web.HttpContext.Current.Request
                .RequestContext.RouteData.DataTokens["area"];

            string strAreaName = string.Empty;
            if (objAreaName != null)
            {
                strAreaName = objAreaName.ToString().Replace(" ", string.Empty);
            }

            return (strAreaName);
        }

        public static string GetControllerName()
        {
            if ((System.Web.HttpContext.Current == null) ||
                (System.Web.HttpContext.Current.Request == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values.Count == 0))
            {
                return (string.Empty);
            }

            object objControllerName =
                System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"];

            string strControllerName = string.Empty;
            if (objControllerName != null)
            {
                strControllerName = objControllerName.ToString().Replace(" ", string.Empty);
            }

            return (strControllerName);
        }

        public static string GetActionName()
        {
            if ((System.Web.HttpContext.Current == null) ||
                (System.Web.HttpContext.Current.Request == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values == null) ||
                (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values.Count == 0))
            {
                return (string.Empty);
            }

            object objActionName =
                System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["Action"];

            string strActionName = string.Empty;
            if (objActionName != null)
            {
                strActionName = objActionName.ToString().Replace(" ", string.Empty);
            }

            return (strActionName);
        }

        public static string EnumValue(Enums.EnumTypes enumType, int enumValue)
        {
            string retValue = "[No Text]";
            string helperValue = string.Empty;
            switch (enumType)
            {
                case Enums.EnumTypes.AccessTypes:
                    {
                        helperValue = ((Enums.AccessTypes)enumValue).ToString();
                        retValue = Resources.Enum.AccessTypes.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.CurrencyUnits:
                    {
                        helperValue = ((Enums.CurrencyUnits)enumValue).ToString();
                        retValue = Resources.Enum.CurrencyUnits.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.EnumTypes:
                    {
                        helperValue = ((Enums.EnumTypes)enumValue).ToString();
                        retValue = Resources.Enum.EnumTypesList.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.FileTypes:
                    {
                        helperValue = ((Enums.FileTypes)enumValue).ToString();
                        retValue = Resources.Enum.FileTypes.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.LogTypes:
                    {
                        helperValue = ((Enums.LogTypes)enumValue).ToString();
                        retValue = Resources.Enum.LogTypes.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.PageMessageTypes:
                    {
                        helperValue = ((Enums.PageMessageTypes)enumValue).ToString();
                        retValue = Resources.Enum.PageMessageTypes.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.RequestStates:
                    {
                        helperValue = ((Enums.RequestStates)enumValue).ToString();
                        retValue = Resources.Enum.RequestStates.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.Subsystems:
                    {
                        helperValue = ((Enums.SubSystems)enumValue).ToString();
                        retValue = Resources.Enum.SubSystems.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.Roles:
                    {
                        helperValue = ((Enums.Roles)enumValue).ToString();
                        retValue = Resources.Enum.Roles.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.ServicesResponse:
                    {
                        helperValue = ((Enums.ServicesResponse)enumValue).ToString();
                        retValue = Resources.Enum.ServicesResponse.ResourceManager.GetString(helperValue);
                        break;
                    }

                default:
                    break;

            }

            return retValue;
        }

        public static string UserLoginByWebService(string userName, string password)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            var oUser =
                oUnitOfWork.UserRepository
                .GetByUserName(userName)
                ;

            if (oUser == null)
                return string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.User.UserName);

            string strHashOfPassword = Utilities.Security.Hashing.GetSha1(password);
            if (string.Compare(strHashOfPassword, oUser.Password, ignoreCase: true) != 0)
                return string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.User.Password);

            if ((oUser.IsDeleted) || (oUser.Role.IsDeleted))
                return Resources.Message.APIMessage.AccountIsDeleted;

            if ((oUser.IsActived == false) || (oUser.Role.IsActived == false))
                return Resources.Message.APIMessage.AccountNotActived;

            return string.Empty;
        }

        public static void InsertErrorLog(string userName, string errorMessage, string description1, string description2)
        {
            DAL.UnitOfWork oUnitOfWoek = new DAL.UnitOfWork();

            oUnitOfWoek.ErrorLogRepository.InsertErrorLog(userName, errorMessage, description1, description2);
            oUnitOfWoek.Save();
        }

        public static List<Infrastructure.EnumRow> EnumList(Enums.EnumTypes enumType)
        {
            List<Infrastructure.EnumRow> EnumRowList = new List<EnumRow>();
            switch (enumType)
            {
                case Enums.EnumTypes.AccessTypes:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.AccessTypes))
                            .Cast<Enums.AccessTypes>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.AccessTypes.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.CurrencyUnits:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.CurrencyUnits))
                            .Cast<Enums.CurrencyUnits>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.CurrencyUnits.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.EnumTypes:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.EnumTypes))
                            .Cast<Enums.EnumTypes>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.EnumTypesList.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.FileTypes:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.FileTypes))
                            .Cast<Enums.FileTypes>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.FileTypes.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.LogTypes:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.LogTypes))
                            .Cast<Enums.LogTypes>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.LogTypes.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.PageMessageTypes:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.PageMessageTypes))
                            .Cast<Enums.PageMessageTypes>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.PageMessageTypes.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.RequestStates:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.RequestStates))
                            .Cast<Enums.RequestStates>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.RequestStates.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.Subsystems:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.SubSystems))
                            .Cast<Enums.SubSystems>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.SubSystems.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.Roles:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.Roles))
                            .Cast<Enums.Roles>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.Roles.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                case Enums.EnumTypes.ServicesResponse:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.ServicesResponse))
                            .Cast<Enums.ServicesResponse>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.ServicesResponse.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }
                    
                case Enums.EnumTypes.FinalApprove:
                    {
                        EnumRowList =
                            Enum.GetValues(typeof(Enums.FinalApprove))
                            .Cast<Enums.FinalApprove>().ToList()
                            .Select(current => new Infrastructure.EnumRow()
                            {
                                Id = current.GetHashCode(),
                                Name = Resources.Enum.FinalApprove.ResourceManager.GetString(current.ToString())
                            })
                            .ToList();
                        break;
                    }

                default:
                    break;
            }
            return EnumRowList;
        }

        public static string ConvertToShamciDate(string date)
        {
            try
            {
                string newDate =
                    date.Substring(0, 4) + "/" + date.Substring(4, 2) + "/" + date.Substring(6, 2);

                return newDate;
            }

            catch (Exception ex)
            {
                return date;
            }
        }

        public static string ConvertToCardNumber(string customerCardNumber)
        {
            try
            {
                string newCustomerCardNumber =
                    customerCardNumber.Substring(0, 4)
                    + "-"
                    + customerCardNumber.Substring(4, 4)
                    + "-"
                    + customerCardNumber.Substring(8, 4)
                    + "-"
                    + customerCardNumber.Substring(12, 4);

                return newCustomerCardNumber;
            }

            catch (Exception ex)
            {
                return customerCardNumber;
            }
        }

        public class MarksCard
        {
            public int RollNo { get; set; }
            public string Subject { get; set; }
            public int FullMarks { get; set; }
            public int Obtained { get; set; }
        }

        public static string Persion(DateTime datetime)
        {
            System.Globalization.PersianCalendar persionDate = new System.Globalization.PersianCalendar();

            string strYear = persionDate.GetYear(datetime).ToString();
            string strMonth = (persionDate.GetMonth(datetime).ToString().Length == 1) ? ("0" + persionDate.GetMonth(datetime)) : (persionDate.GetMonth(datetime).ToString());
            string strDay = (persionDate.GetDayOfMonth(datetime).ToString().Length == 1) ? ("0" + persionDate.GetDayOfMonth(datetime)) : (persionDate.GetDayOfMonth(datetime).ToString());
            return strYear + "/" + strMonth + "/" + strDay;
        }

        public static DateTime? ToDate(string persianDate)
        {
            var pattern = @"^(?<year>\d{4})(?<space>[-,/])(?<month>\d{1,2})(\k<space>)(?<day>\d{1,2})$";

            if (System.Text.RegularExpressions.Regex.IsMatch(persianDate, pattern))
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                System.Text.RegularExpressions.GroupCollection groups = regex.Match(persianDate).Groups;
                int year = Convert.ToInt32(groups["year"].Value);
                int month = Convert.ToInt32(groups["month"].Value);
                int day = Convert.ToInt32(groups["day"].Value);

                System.Globalization.PersianCalendar calendar = new System.Globalization.PersianCalendar();
                return calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }

            return null;
        }

        public static System.Web.IHtmlString DisplayDateTime
                    (this System.Web.Mvc.HtmlHelper helper, System.DateTime? dateTime)
        {
            return (DisplayDateTime(helper, dateTime, false));
        }

        public static System.Web.IHtmlString DisplayDateTime
            (this System.Web.Mvc.HtmlHelper helper, System.DateTime? dateTime, bool displayTime)
        {
            if (dateTime == null)
            {
                return (helper.Raw("-----"));
            }

            System.Globalization.PersianCalendar oPersianCalendar =
                new System.Globalization.PersianCalendar();

            int intY = oPersianCalendar.GetYear(dateTime.Value);
            int intM = oPersianCalendar.GetMonth(dateTime.Value);
            int intD = oPersianCalendar.GetDayOfMonth(dateTime.Value);

            string strShamsiDate =
                string.Format("{0}/{1}/{2}", intY, intM, intD);

            if (displayTime)
            {
                strShamsiDate =
                    string.Format("[{0}] -[{1}:{2}:{3}]",
                    strShamsiDate, dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second);
            }

            return (helper.Raw(strShamsiDate));
        }

        public static string DisplayDateTime
            (System.DateTime? dateTime, bool displayTime)
        {
            if (dateTime == null)
            {
                return ("-----");
            }

            System.Globalization.PersianCalendar oPersianCalendar =
                new System.Globalization.PersianCalendar();

            int intY = oPersianCalendar.GetYear(dateTime.Value);
            int intM = oPersianCalendar.GetMonth(dateTime.Value);
            int intD = oPersianCalendar.GetDayOfMonth(dateTime.Value);

            string strShamsiDate =
                string.Format("{0}/{1}/{2}", intY, intM, intD);

            if (displayTime)
            {
                strShamsiDate =
                    string.Format("[{0}] -[{1}:{2}:{3}]",
                    strShamsiDate, dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second);
            }

            return (strShamsiDate);
        }


        internal static List<ReportParameterValue> GetReportParameterValues(IList<ReportParameter> reportParameters,
     FormCollection formCollection, bool isConvert = true)
        {
            var reportParamValues = new List<ReportParameterValue>();

            if (reportParameters != null)
            {
                foreach (var reportParam in reportParameters)
                {
                    if (reportParam.DataType == Enums.DataType.Date)
                    {
                        if (isConvert)
                        {
                            var date = formCollection[reportParam.Name];
                            if (formCollection[reportParam.Name].Length > 0)
                            {
                                reportParamValues.Add(new ReportParameterValue
                                {
                                    ParameterName = reportParam.Name,
                                    Value = ToDate(formCollection[reportParam.Name])
                                });
                            }

                        }
                        else
                        {
                            System.DateTime date;
                            if (System.DateTime.TryParse(formCollection[reportParam.Name], out date))
                            {
                                reportParamValues.Add(new ReportParameterValue
                                {
                                    ParameterName = reportParam.Name,
                                    Value = date
                                });
                            }
                            else
                            {
                                reportParamValues.Add(new ReportParameterValue
                                {
                                    ParameterName = reportParam.Name,
                                    Value = formCollection[reportParam.Name]
                                });
                            }
                        }

                    }
                    else
                    {
                        reportParamValues.Add(new ReportParameterValue
                        {
                            ParameterName = reportParam.Name,
                            Value = formCollection[reportParam.Name]
                        });
                    }
                }
            }

            return reportParamValues;
        }


        internal static Dictionary<string, string> GetFieldColumns(DataTable dataTable)
        {
            var result = new Dictionary<string, string>();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                result.Add(dataTable.Columns[i].ColumnName, "Field" + i);
            }

            return result;
        }
        internal static List<object> DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            var fieldcolumns = Infrastructure.Utility.GetFieldColumns(table);
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            var result = new List<object>();

            var JSONString = new System.Text.StringBuilder();
            var post = new Common.KendoGridPost();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString = new System.Text.StringBuilder();
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        string tablerow = table.Rows[i][j].ToString();
                        tablerow = tablerow.Replace("\"", string.Empty);

                        if (j < table.Columns.Count - 1)
                        {

                            JSONString.Append("\"" + fieldcolumns[table.Columns[j].ColumnName.ToString()] + "\":" + "\"" + tablerow + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + fieldcolumns[table.Columns[j].ColumnName.ToString()] + "\":" + "\"" + tablerow + "\"");
                        }
                    }
                    JSONString.Append("}");

                    result.Add(jsSerializer.DeserializeObject(JSONString.ToString()));
                }
            }

            return result;

            //var strList = jsSerializer.Serialize(parentRow);

            //var ppp = jsSerializer.DeserializeObject(strList);
            //return jsSerializer.DeserializeObject(strList) as List<object>;
        }

      internal static List<string> GetFieldColumnsINfo(DataTable dataTable)
      {
         var result = new List<string>();

         for (int i = 0; i < dataTable.Columns.Count; i++)
         {
            result.Add("Field" + i);
         }

         return result;
      }

      internal static List<string> GetresourcesName(DataTable dataTable)
      {
         var result = new List<string>();

         for (int i = 0; i < dataTable.Columns.Count; i++)
         {
            result.Add(dataTable.Columns[i].ColumnName);
         }

         return result;
      }



   }
}