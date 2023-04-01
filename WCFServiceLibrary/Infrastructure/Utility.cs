using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFServiceLibrary.Infrastructure
{
    public static class Utility
    {
        static Utility()
        { }

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

                case Enums.EnumTypes.EnumTypes:
                    {
                        helperValue = ((Enums.EnumTypes)enumValue).ToString();
                        retValue = Resources.Enum.EnumTypesList.ResourceManager.GetString(helperValue);
                        break;
                    }

                case Enums.EnumTypes.CurrencyUnits:
                    {
                        helperValue = ((Enums.CurrencyUnits)enumValue).ToString();
                        retValue = Resources.Enum.CurrencyUnits.ResourceManager.GetString(helperValue);
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
    }
}