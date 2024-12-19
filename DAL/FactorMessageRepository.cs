using System;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class FactorMessageRepository : Repository<Models.FactorMessage>, IFactorMessageRepository
    {
        public FactorMessageRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public string MetMessageById(Guid headoffactorid)
        {
            var varMessages =
                Get()
                .Include(x => x.User)
                .Where(current => current.HeadOfFactorId == headoffactorid)
                .OrderBy(current => current.InsertDateTime)
                .ToList()
                ;

            string RequestMessage = "[پیامی برای این فاکتور در سیستم ثبت نشده است]";

            if (varMessages.Count > 0)
            {
                RequestMessage = string.Empty;
                int Counter = 0;
                foreach (var message in varMessages)
                {
                    Counter++;
                    RequestMessage += "<div><span class='badge'>" + Counter + "</span> ";
                    RequestMessage += "<span style='font-weight:bold'>" + Persion(message.InsertDateTime) + " ";
                    RequestMessage += message.User.FullName + " : </span>";
                    RequestMessage += message.MessageText;
                    RequestMessage += "</div><br/>";
                }
            }

            return RequestMessage;
        }

        public string Persion(DateTime mydate)
        {
            System.Globalization.PersianCalendar mycal = new System.Globalization.PersianCalendar();

            string strYear = mycal.GetYear(mydate).ToString();
            string strMonth = (mycal.GetMonth(mydate).ToString().Length == 1) ? ("0" + mycal.GetMonth(mydate)) : (mycal.GetMonth(mydate).ToString());
            string strDay = (mycal.GetDayOfMonth(mydate).ToString().Length == 1) ? ("0" + mycal.GetDayOfMonth(mydate)) : (mycal.GetDayOfMonth(mydate).ToString());
            return "[" + strYear + "/" + strMonth + "/" + strDay + "]";
        }

        public string EnumValue(Enums.EnumTypes enumType, int enumValue)
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
    }
}
