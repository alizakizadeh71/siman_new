//using CAPICOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Data.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WCFServiceLibrary
{
    public class OPSServices : IOPSServices
    {
        DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

        #region IVO_Import New Version 95-05-02 Started
        public int IVO_Import_Insert_Base
            (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode
            , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            , string SecNumber, string SecDate, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue
            , string PerformNumber, string PerformDate, decimal BaseCurrencyValue, List<string> FileList
            , out string message, bool? IsEdit = false)
        {
            string InputData = string.Empty;
            try
            {
                // اگرپرونده ویرایشی بود رکورد جدبد حساب شود
                if (IsEdit == true)
                {
                    if (!string.IsNullOrEmpty(RecordNumber))
                    {
                        RecordNumber = RecordNumber + "_Edited";
                    }
                }
                var oProvince = oUnitOfWork.ProvinceRepository.GetByCode("97");

                var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);

                int SubSystemCode;
                if (ProvinceCode == "97") // اگر 40 بود کارشناسی است - اگر کد 97 بود یعنی پرداخت فوب است
                {
                    SubSystemCode = (int)Enums.SubSystems.Drug_Fob;
                }
                else if (ProvinceCode == "40")
                {
                    SubSystemCode = (int)Enums.SubSystems.Drug_Import;
                }
                else
                {
                    message = "خطا ناشناخته در ops";
                    return -100;
                }

                var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                    .FirstOrDefault(current => current.Code == SubSystemCode);

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .FirstOrDefault();

                InputData
                    = "  *****  InputData *****"
                    + "||UserName: " + UserName
                    + "||CompanyName: " + CompanyName
                    + "||CompanyNationalCode: " + CompanyNationalCode
                    + "||ProvinceCode: " + ProvinceCode
                    + "||CommodityType: " + CommodityType
                    + "||TotalValue: " + TotalValue
                    + "||CommodityUnit: " + CommodityUnit
                    + "||RecordNumber: " + RecordNumber
                    + "||RecordDate: " + RecordDate
                    + "||SecNumber: " + SecNumber
                    + "||SecDate: " + SecDate
                    + "||CellPhoneNumber: " + CellPhoneNumber
                    + "||CurrencyCode: " + CurrencyCode
                    + "||CurrencyValue: " + CurrencyValue
                    + "||PerformNumber: " + PerformNumber
                    + "||PerformDate: " + PerformDate
                    + "||BaseCurrencyValue: " + BaseCurrencyValue
                    + "||IsEdit: " + IsEdit
                    + "||TotalValue: " + TotalValue
                    + "||SubSystem: " + oSubSystem.Id
                    + "||ServiceTariffInSubSystem: " + oServiceTariffInSubSystem.Id;

                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CellPhoneNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

                List<Models.Request> requestList
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .Where(current => current.RecordDate == RecordDate)
                    .Where(current => Math.Round(current.CurrencyValue) == Math.Round(CurrencyValue)) /// برای اینکه اگر مبلغ فوب تغییر کرد درخواست جدید حساب شود
                    .ToList()
                    ;

                Models.Request requestRow = requestList
                        .OrderByDescending(current => current.InsertDateTime)
                        .FirstOrDefault();

                long AmountPaid;
                if (oCurrency.Code == (int)Enums.CurrencyUnits.Rails)
                {
                    AmountPaid = Convert.ToInt64(CurrencyValue);
                }
                else
                {
                    AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio); /// شاید نرخ ارز تغییر کرده باشد
                }


                /// اگر وجود داشت
                if (requestRow != null)
                {
                    /// برای حذف موارد تکراری و حذف فوبی که دستور پرداخت ندارد
                    List<Models.Request> oDeleteRequest = requestList
                        //.Where(curren => curren.Id != requestRow.Id)
                        .Where(curren => curren.RequestState < (int)Enums.RequestStates.PaymentOrder)
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();

                        var allFileRow = requestRow.Files.ToList();
                        foreach (var file in allFileRow)
                        {
                            file.IsDeleted = true;
                            file.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }

                    ///دارای دستور پرداخت - پرداخت یا تایید پرداخت هست
                    if (requestRow.RequestState >= (int)Enums.RequestStates.PaymentOrder)
                    {
                        var InvoiceNumberr = oUnitOfWork.RequestRepository.GetById(requestRow.Id).InvoiceNumber;
                        message = InvoiceNumberr.ToString();
                        return InvoiceNumberr;
                    }
                }

                #region Insert New Request
                Models.Request oRequest = new Models.Request();
                oRequest.UserId = oUser.Id;
                oRequest.SubSystemId = oSubSystem.Id;
                oRequest.SubSystem = oSubSystem;
                oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                oRequest.ProvinceId = oProvince.Id;
                oRequest.Province = oProvince;
                oRequest.CommodityType = CommodityType;
                oRequest.TotalValue = TotalValue;
                oRequest.CommodityUnit = CommodityUnit;
                oRequest.SecDate = SecDate;
                oRequest.SecNumber = SecNumber;
                oRequest.PerformNumber = PerformNumber;
                oRequest.PerformDate = PerformDate;
                oRequest.CompanyName = CompanyName;
                oRequest.CompanyNationalCode = CompanyNationalCode;
                oRequest.RecordNumber = RecordNumber;
                oRequest.RecordDate = RecordDate;
                oRequest.InvoiceDate = DateTime.Now;
                oRequest.CellPhoneNumber = CellPhoneNumber;
                oRequest.AmountPaid = AmountPaid;
                oRequest.RequestState = SubSystemCode == (int)Enums.SubSystems.Drug_Import ? 1 : 0;
                oRequest.CurrencyCode = oCurrency.Code;
                oRequest.CurrencyRation = oCurrency.Ratio;
                oRequest.CurrencyValue = CurrencyValue;
                oRequest.BaseCurrencyValue = BaseCurrencyValue;
                oRequest.IsActived = true;
                oRequest.IsDeleted = false;
                oRequest.IsSystem = false;
                oRequest.IsVerified = true;
                oRequest.Tariffs = 0;
                oRequest.LicenseNumber = string.Empty;
                oRequest.LicenseDate = DateTime.Now;

                oUnitOfWork.RequestRepository.Insert(oRequest);
                oUnitOfWork.Save();
                #endregion Insert New Request

                #region Insert New Message
                Models.Message oMessage = new Models.Message();
                oMessage.UserId = oUser.Id;
                oMessage.LastState = oRequest.RequestState;
                oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                oMessage.NewState = oRequest.RequestState;
                oMessage.RequestId = oRequest.Id;
                oUnitOfWork.MessageRepository.Insert(oMessage);
                oUnitOfWork.Save();
                #endregion Insert New Message

                #region Create File
                Models.File newFile;
                foreach (var fileAddress in FileList)
                {
                    newFile = new Models.File();
                    newFile.Id = Guid.NewGuid();
                    newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                    newFile.InsertDateTime = DateTime.Now;
                    newFile.IsActived = true;
                    newFile.IsDeleted = false;
                    newFile.IsSystem = true;
                    newFile.IsVerified = true;
                    newFile.Name = fileAddress.Split('&')[0].ToString();
                    newFile.RequestId = oRequest.Id;
                    newFile.UpdateDateTime = DateTime.Now;
                    oUnitOfWork.FileRepository.Insert(newFile);
                    oUnitOfWork.Save();
                }
                #endregion Create File

                var InvoiceNumber = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                message = InvoiceNumber.ToString();
                return InvoiceNumber;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = ex.Message;
                throw ex;
            }
        }
        public int IVO_Import_State_Base(string UserName, string Password, int InvoiceNumber, out string message)
        {
            #region Return Value Details
            // intRetValue=-4 -- UnDefind Error
            // intRetValue=-3 -- LoginFailed
            // intRetValue=-2 -- NotFound
            // intRetValue=-1 -- Request Incomplate
            // intRetValue= 0 -- Request Initialy
            // intRetValue= 1 -- Request PaymentOrder
            // intRetValue= 2 -- Request Payment
            // intRetValue= 3 -- Request PaymentConfirmation
            #endregion

            Models.Request oRequest = null;

            string InputData
                = "  *****  InputData *****"
                + "||UserName: " + UserName
                + "||Password: " + Password
                + "||InvoiceNumber: " + InvoiceNumber;

            #region Login Failed
            message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

            if (!string.IsNullOrEmpty(message))
            {
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -3;
            }
            #endregion

            try
            {
                oRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .FirstOrDefault()
                    ;

                //Not Found
                if (oRequest == null)
                {
                    message = string.Format(Resources.Message.Global.RecordNotFound, InvoiceNumber);
                    return -2;
                }

                else if (oRequest.RequestState == (int)Enums.RequestStates.PaymentConfirmation)
                {
                    message = oRequest.Bank_BankReciptNumber;
                    return (int)Enums.RequestStates.PaymentConfirmation;
                }

                else
                {
                    message =
                        oUnitOfWork.MessageRepository.Get()
                        .Where(current => current.RequestId == oRequest.Id)
                        .OrderByDescending(current => current.InsertDateTime).Select(x=>x.MessageText).FirstOrDefault();
                    return oRequest.RequestState;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return -1;
            }
        }
        #endregion
        #region IVO_Clearance


        public int IVO_Clearance_New
           (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode
           , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate, string ImportRecordNumber
           , string SecNumber, string SecDate, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, long CustomsValue, string PerformNumber
           , string PerformDate, decimal BaseCurrencyValue, string Description, List<string> FileList, out string message)
        {
            var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceCode);
            var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);

            var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                .ToList()
                .Where(current => SecNumber != "00000"
                    ? current.Code == (int)Enums.SubSystems.Drug_Clearance
                    : current.Code == (int)Enums.SubSystems.Drug_Clearance23)
                .FirstOrDefault();

            var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                .ToList()
                .Where(current => current.SubSystemId == oSubSystem.Id)
                .FirstOrDefault();

            string InputData
                = "  *****  InputData *****"
                + "||SubSystem:InsertClearance "
                + "||UserName: " + UserName
                + "||CompanyName: " + CompanyName
                + "||SubSystem: " + oSubSystem.Id
                + "||CompanyNationalCode: " + CompanyNationalCode
                + "||ImportRecordNumber: " + ImportRecordNumber
                + "||RecordNumber: " + RecordNumber;

            try
            {
                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CellPhoneNumber) || CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

                Models.Request oRequest = null;

                Models.Request oFindRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.SecNumber == SecNumber)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .Where(current => current.RecordDate == RecordDate)
                    .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                    .OrderByDescending(current => current.InsertDateTime)
                    .FirstOrDefault()
                    ;

                if (oFindRequest != null && oFindRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                {
                    #region Delete Last Record
                    oFindRequest.IsDeleted = true;
                    oFindRequest.IsActived = false;
                    oUnitOfWork.Save();
                    var allFileRow = oFindRequest.Files.ToList();

                    foreach (var file in allFileRow)
                    {
                        file.IsDeleted = true;
                        file.IsActived = false;
                    }
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = SecDate;
                    oRequest.SecNumber = SecNumber;
                    oRequest.PerformNumber = PerformNumber;
                    oRequest.PerformDate = PerformDate;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.ImportRecordNumber = ImportRecordNumber;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = CustomsValue != 0 ? CustomsValue : (Convert.ToInt64(CurrencyValue * oCurrency.Ratio));
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyValue = CurrencyValue;
                    oRequest.BaseCurrencyValue = BaseCurrencyValue;
                    oRequest.Description = Description;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.Tariffs = 0;
                    oRequest.LicenseNumber = string.Empty;
                    oRequest.LicenseDate = DateTime.Now;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oFindRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion

                    #region Create File
                    Models.File newFile;
                    foreach (var fileAddress in FileList)
                    {
                        newFile = new Models.File();
                        newFile.Id = Guid.NewGuid();
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = oRequest.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                    #endregion
                }

                else if (oFindRequest == null)
                {
                    #region Insert New Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = SecDate;
                    oRequest.SecNumber = SecNumber;
                    oRequest.PerformNumber = PerformNumber;
                    oRequest.PerformDate = PerformDate;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.ImportRecordNumber = ImportRecordNumber;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = CustomsValue != 0 ? CustomsValue : (Convert.ToInt64(CurrencyValue * oCurrency.Ratio));
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyValue = CurrencyValue;
                    oRequest.BaseCurrencyValue = BaseCurrencyValue;
                    oRequest.Description = Description;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.Tariffs = 0;
                    oRequest.LicenseNumber = string.Empty;
                    oRequest.LicenseDate = DateTime.Now;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion

                    #region Create File
                    Models.File newFile;
                    foreach (var fileAddress in FileList)
                    {
                        newFile = new Models.File();
                        newFile.Id = Guid.NewGuid();
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = oRequest.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                    #endregion
                }

                if (oRequest != null)
                {
                    message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();

                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(current => current.RecordDate == RecordDate)
                        .Where(curren => curren.SecNumber == SecNumber)
                        .Where(curren => curren.Id != oRequest.Id)
                        .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }
                    //

                    return oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                }

                else if (oFindRequest != null)
                {
                    message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(current => current.RecordDate == RecordDate)
                        .Where(curren => curren.SecNumber == SecNumber)
                        .Where(curren => curren.Id != oFindRequest.Id)
                        .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }
                    //
                    return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                }

                else
                {
                    message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();
                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(current => current.RecordDate == RecordDate)
                        .Where(curren => curren.SecNumber == SecNumber)
                        .Where(curren => curren.Id != oRequest.Id)
                        .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }
                    //
                    return oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = Resources.Message.Global.UnknownError;
                return -1;
            }
        }



        public int IVO_Clearance_Insert_Base
           (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode
           , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate, string ImportRecordNumber
           , string SecNumber, string SecDate, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, string PerformNumber
           , string PerformDate, decimal BaseCurrencyValue, string Description, List<string> FileList, out string message)
        {
            var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceCode);
            var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);

            var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                .ToList()
                .Where(current => SecNumber != "00000"
                    ? current.Code == (int)Enums.SubSystems.Drug_Clearance
                    : current.Code == (int)Enums.SubSystems.Drug_Clearance23)
                .FirstOrDefault();

            var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                .ToList()
                .Where(current => current.SubSystemId == oSubSystem.Id)
                .FirstOrDefault();

            string InputData
                = "  *****  InputData *****"
                + "||SubSystem:InsertClearance "
                + "||UserName: " + UserName
                + "||CompanyName: " + CompanyName
                + "||SubSystem: " + oSubSystem.Id
                + "||CompanyNationalCode: " + CompanyNationalCode
                + "||ImportRecordNumber: " + ImportRecordNumber
                + "||RecordNumber: " + RecordNumber;

            try
            {
                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CellPhoneNumber) || CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

                Models.Request oRequest = null;

                Models.Request oFindRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.SecNumber == SecNumber)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .Where(current => current.RecordDate == RecordDate)
                    .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                    .OrderByDescending(current => current.InsertDateTime)
                    .FirstOrDefault()
                    ;

                if (oFindRequest != null && oFindRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                {
                    #region Delete Last Record
                    oFindRequest.IsDeleted = true;
                    oFindRequest.IsActived = false;
                    oUnitOfWork.Save();
                    var allFileRow = oFindRequest.Files.ToList();

                    foreach (var file in allFileRow)
                    {
                        file.IsDeleted = true;
                        file.IsActived = false;
                    }
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = SecDate;
                    oRequest.SecNumber = SecNumber;
                    oRequest.PerformNumber = PerformNumber;
                    oRequest.PerformDate = PerformDate;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.ImportRecordNumber = ImportRecordNumber;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = oSubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23
                        ? (Convert.ToInt64(CurrencyValue * oCurrency.Ratio))
                        : (Convert.ToInt64(CurrencyValue * oCurrency.Ratio) + 33000);
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyValue = CurrencyValue;
                    oRequest.BaseCurrencyValue = BaseCurrencyValue;
                    oRequest.Description = Description;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.Tariffs = 0;
                    oRequest.LicenseNumber = string.Empty;
                    oRequest.LicenseDate = DateTime.Now;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oFindRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion

                    #region Create File
                    Models.File newFile;
                    foreach (var fileAddress in FileList)
                    {
                        newFile = new Models.File();
                        newFile.Id = Guid.NewGuid();
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = oRequest.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                    #endregion
                }

                else if (oFindRequest == null)
                {
                    #region Insert New Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = SecDate;
                    oRequest.SecNumber = SecNumber;
                    oRequest.PerformNumber = PerformNumber;
                    oRequest.PerformDate = PerformDate;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.ImportRecordNumber = ImportRecordNumber;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = oSubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23
                        ? (Convert.ToInt64(CurrencyValue * oCurrency.Ratio))
                        : (Convert.ToInt64(CurrencyValue * oCurrency.Ratio) + 33000);
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyValue = CurrencyValue;
                    oRequest.BaseCurrencyValue = BaseCurrencyValue;
                    oRequest.Description = Description;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.Tariffs = 0;
                    oRequest.LicenseNumber = string.Empty;
                    oRequest.LicenseDate = DateTime.Now;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion

                    #region Create File
                    Models.File newFile;
                    foreach (var fileAddress in FileList)
                    {
                        newFile = new Models.File();
                        newFile.Id = Guid.NewGuid();
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = oRequest.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                    #endregion
                }

                if (oRequest != null)
                {
                    message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();

                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(current => current.RecordDate == RecordDate)
                        .Where(curren => curren.SecNumber == SecNumber)
                        .Where(curren => curren.Id != oRequest.Id)
                        .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }
                    //

                    return oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                }

                else if (oFindRequest != null)
                {
                    message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(current => current.RecordDate == RecordDate)
                        .Where(curren => curren.SecNumber == SecNumber)
                        .Where(curren => curren.Id != oFindRequest.Id)
                        .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }
                    //
                    return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                }

                else
                {
                    message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();
                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(current => current.RecordDate == RecordDate)
                        .Where(curren => curren.SecNumber == SecNumber)
                        .Where(curren => curren.Id != oRequest.Id)
                        .Where(curren => (curren.RequestState != 3 || curren.RequestState != 2))
                        .ToList()
                        ;
                    if (oDeleteRequest.Count() > 0)
                    {
                        foreach (var item in oDeleteRequest)
                        {
                            item.IsDeleted = true;
                            item.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }
                    //
                    return oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = Resources.Message.Global.UnknownError;
                return -1;
            }
        }

        public int ClearanceRequestState(string UserName, string Password, int InvoiceNumber, out string message)
        {
            #region Return Value Details
            // intRetValue=-4 -- UnDefind Error
            // intRetValue=-3 -- LoginFailed
            // intRetValue=-2 -- NotFound
            // intRetValue=-1 -- Request Incomplate
            // intRetValue= 0 -- Request Initialy
            // intRetValue= 1 -- Request PaymentOrder
            // intRetValue= 2 -- Request Payment
            // intRetValue= 3 -- Request PaymentConfirmation
            #endregion

            Models.Request oRequest = null;

            string InputData
                = "  *****  InputData *****"
                + "||UserName: " + UserName
                + "||Password: " + Password
                + "||InvoiceNumber: " + InvoiceNumber;

            #region Login Failed
            message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

            if (!string.IsNullOrEmpty(message))
            {
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -3;
            }
            #endregion

            try
            {
                oRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault()
                    ;

                //Not Found
                if (oRequest == null)
                {
                    message = string.Format(Resources.Message.Global.RecordNotFound, InvoiceNumber);
                    return -2;
                }

                else if (oRequest.RequestState == (int)Enums.RequestStates.PaymentConfirmation)
                {
                    message = oRequest.Bank_BankReciptNumber;
                    return (int)Enums.RequestStates.PaymentConfirmation;
                }

                else
                {
                    message = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, oRequest.RequestState);
                    return oRequest.RequestState;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return -1;
            }
        }
        public int GetDepositNumber(string UserName, string Password, string RecordNumber, out string message)
        {
            #region Return Value Details
            // intRetValue=-4 -- UnDefind Error
            // intRetValue=-3 -- LoginFailed
            // intRetValue=-2 -- NotFound
            // intRetValue=-1 -- Request Incomplate
            // intRetValue= 0 -- Request Initialy
            // intRetValue= 1 -- Request PaymentOrder
            // intRetValue= 2 -- Request Payment
            // intRetValue= 3 -- Request PaymentConfirmation
            #endregion

            Models.Request oRequest = null;

            string InputData
                = "  *****  InputData *****"
                + "||UserName: " + UserName
                + "||Password: " + Password
                + "||RecordNumber: " + RecordNumber;

            #region Login Failed
            message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

            if (!string.IsNullOrEmpty(message))
            {
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -3;
            }
            #endregion

            try
            {
                oRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.RecordNumber == RecordNumber)
                    .FirstOrDefault()
                    ;

                if (oRequest == null)
                {
                    message = string.Format(Resources.Message.Global.RecordNotFound, RecordNumber);
                    return -2;
                }
                else if (oRequest.RequestState == 1 || oRequest.RequestState == 2 || oRequest.RequestState == 3)
                {
                    message = oRequest.DepositNumber;
                    return 0;
                }
                else
                {
                    message = oRequest.DepositNumber;
                    return -2;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return -1;
            }
        }
        #endregion

        public bool Delete_Request(string UserName, string Password, int SubSystemCode, int InvoiceNumber, out string message)
        {
            Models.Request oRequest = null;

            string InputData
                = "  *****  Delete *****"
                + "||UserName: " + UserName
                + "||Password: " + Password
                + "||SubSystemCode: " + SubSystemCode
                + "||InvoiceNumber: " + InvoiceNumber;

            #region Login Failed
            message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

            if (!string.IsNullOrEmpty(message))
            {
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return false;
            }
            #endregion

            try
            {
                oRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .Where(x => x.SubSystem.Code == SubSystemCode)
                    .FirstOrDefault()
                    ;

                if (oRequest == null)
                {
                    message = string.Format(Resources.Message.Global.RecordNotFound, InvoiceNumber);
                    return false;
                }

                else if (oRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                {
                    oUnitOfWork.RequestRepository.DeleteById(oRequest.Id);
                    oUnitOfWork.Save();
                    return true;
                }

                else
                {
                    message = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, oRequest.RequestState);
                    return false;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return false;
            }
        }

        public bool Delete_Request_ByRecordNumber(
            string UserName, string Password, int SubSystemCode, string RecordNumber, out string message)
        {
            List<Models.Request> list = null;

            string InputData
                = "  *****  Delete *****"
                + "||UserName: " + UserName
                + "||Password: " + Password
                + "||SubSystemCode: " + SubSystemCode
                + "||RecordNumber: " + RecordNumber;

            #region Login Failed
            message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

            if (!string.IsNullOrEmpty(message))
            {
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return false;
            }
            #endregion

            try
            {
                list =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(x => x.IsActived)
                    .Where(x => !x.IsDeleted)
                    .Where(current => current.RecordNumber == RecordNumber)
                    .Where(x => x.SubSystem.Code == SubSystemCode)
                    .ToList()
                    ;

                if (list.Count <= 0)
                {
                    message = string.Format(Resources.Message.Global.RecordNotFound, RecordNumber);
                    return false;
                }

                foreach (var row in list)
                {
                    if (row.RequestState < (int)Enums.RequestStates.PaymentOrder)
                    {
                        oUnitOfWork.RequestRepository.DeleteById(row.Id);
                        oUnitOfWork.Save();
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return false;
            }
        }

        public int TestConnection(out string message)
        {
            try
            {
                message = "Connect Successful";
                return 1;
            }

            catch (Exception ex)
            {
                message = ex.Message;
                return -1;
            }
        }




        #region CertPayment


        public int Cert_Payment3
    (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
    string CityCode
    , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
    //, string SecNumber
    //, string SecDate
    , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, int SubSystem, int ServiceTariff
    , string Description
    //, string PerformNumber
    //, string PerformDate
    //, decimal BaseCurrencyValue
    , List<string> FileList
    , out string message)
        {
            string InputData = string.Empty;
            int codeee = -50;
            try
            {
                var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceCode);
                codeee = -49;
                var oCity = oUnitOfWork.CityRepository.GetByCode(CityCode, oProvince.Id);
                codeee = -48;

                var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);
                codeee = -47;

                if (!(SubSystem == 3001 || SubSystem == 3002))
                {
                    message = "کد زیر سیستم باید 3001 یا 3002 باشد";
                    return codeee;
                }
                codeee = -469;
                if (!(ServiceTariff == 108 || ServiceTariff == 109 || ServiceTariff == 117 || ServiceTariff == 0))
                {
                    message = "کد تعرفه خدمت باید 0 یا 108 یا 109 یا 117 باشد";
                    return codeee;
                }

                if (ServiceTariff == 0)
                    ServiceTariff = 999;

                //codeee = -468;
                //if (SubSystem == 3002 && FileList.Count == 0)
                //{
                //    message = "ارسال فایل برای زیر سیستم گواهی حق ثبت اجباری میباشد";
                //    return codeee;
                //}

                codeee = -467;


                var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                    .ToList()
                    .Where(current => current.Code == SubSystem)
                    .FirstOrDefault();
                codeee = -46;

                string ServiceTariffs = ServiceTariff.ToString();

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.IsActived && !current.IsDeleted)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.ServiceTariff.RCode == ServiceTariffs)
                    .SingleOrDefault();

                codeee = -45;

                InputData
                        = "  *****  InputData *****"
                        + "||SubSystem:InsertImport "
                        + "||UserName: " + UserName
                        + "||Password: " + Password
                        + "||CompanyName: " + CompanyName
                        + "||CompanyNationalCode: " + CompanyNationalCode
                        + "||ProvinceCode: " + ProvinceCode
                        + "||CityCode: " + CityCode
                        + "||CommodityType: " + CommodityType
                        + "||TotalValue: " + TotalValue
                        + "||CommodityUnit: " + CommodityUnit
                        + "||RecordNumber: " + RecordNumber
                        + "||RecordDate: " + RecordDate
                        + "||CellPhoneNumber: " + CellPhoneNumber
                        + "||CurrencyCode: " + CurrencyCode
                        + "||CurrencyValue: " + CurrencyValue
                        + "||SubSystem: " + SubSystem
                        + "||ServiceTariff: " + ServiceTariff
                        + "||Description: " + Description;



                //           *****InputData * ****|| SubSystem:InsertImport || UserName: mali - admin || Password: 1234512345 ||
                //           CompanyName: علی اصغر نوبهاری || CompanyNationalCode: 5749487008 || ProvinceCode: 30 || CityCode: 45 ||
                //           CommodityType: پروانه بهداشتی بهره برداری خودرو || TotalValue: 0 || CommodityUnit: مجوز ||
                //           RecordNumber: 279449 || RecordDate: 1400 / 06 / 31 || CellPhoneNumber: 09023525686 || CurrencyCode: 1000 ||
                //           CurrencyValue: 225000.00 || SubSystem: 3001 || ServiceTariff: 999 ||
                //           Description: نوع مجوز: پروانه بهداشتی بهره برداری خودرو(279449) //نوع درخواست:مجوز اولیه()




                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);
                codeee = -44;

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || (!((CompanyNationalCode.Trim().Length == 11) || (CompanyNationalCode.Trim().Length == 10))))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }
                codeee = -43;

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }
                codeee = -42;

                if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }
                codeee = -41;

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -1;
                }
                #endregion
                codeee = -40;

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Cert");
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);
                codeee = -39;

                int RilaCode = (int)Enums.CurrencyUnits.Rails;
                Models.Request oRequest = null;
                Models.Request oFindRequest = null;

                List<Models.Request> oList
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    //.Where(current => current.RecordDate == RecordDate)
                    .ToList()
                    ;
                codeee = -38;

                //if (oCurrency.Code == RilaCode)
                //    oFindRequest =
                //        oList
                //        .Where(current => current.CurrencyCode == RilaCode)
                //        .OrderByDescending(current => current.InvoiceNumber)
                //        .FirstOrDefault()
                //        ;

                //else
                //    oFindRequest =
                //        oList
                //       .Where(current => current.CurrencyCode != RilaCode)
                //       .OrderByDescending(current => current.InvoiceNumber)
                //       .FirstOrDefault()
                //       ;

                oFindRequest =
                 oList
                 .OrderByDescending(current => current.InvoiceNumber)
                 .FirstOrDefault()
                 ;

                codeee = -37;
                var MessageText = Resources.Message.Request.Message_InitialRequet;

                if (oFindRequest != null) // فاکتور از قبل وجود دارد
                {
                    if (oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder) // فاکتور هنوز پرداخت نشده است
                    {
                        #region Delete Last Record Files
                        // فاکتور های قبلی را حذف کند
                        foreach (var lastRow in oList)
                        {
                            lastRow.IsDeleted = true;
                            lastRow.IsActived = false;
                        }
                        oUnitOfWork.Save();

                        var allFileRow = oFindRequest.Files.ToList();
                        if (allFileRow != null)
                        {
                            foreach (var file in allFileRow)
                            {
                                file.IsDeleted = true;
                                file.IsActived = false;
                            }
                            oUnitOfWork.Save();
                        }

                        codeee = -36;
                        #endregion
                        MessageText = Resources.Message.Request.Message_Update;
                    }
                    else if (oFindRequest.RequestState > (int)Enums.RequestStates.PaymentOrder) // فاکتور پرداخت شده است
                    {
                        var amount = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                        if (amount != oFindRequest.AmountPaid)
                        {
                            return -100; // پرداخت انجام شده ولی مبلغ تغییر کرده است خطا بده
                        }

                        message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                        return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                    }
                }
                #region Insert New Record
                codeee = -27;
                oRequest = new Models.Request();
                codeee = -266;

                oRequest.UserId = oUser.Id;
                codeee = -267;

                oRequest.SubSystemId = oSubSystem.Id;
                codeee = -268;

                oRequest.SubSystem = oSubSystem;
                codeee = -269;
                oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                codeee = -270;
                oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                codeee = -271;
                oRequest.ProvinceId = oProvince.Id;
                codeee = -272;
                oRequest.Province = oProvince;
                codeee = -273;
                oRequest.CityId = oCity.Id;
                codeee = -274;
                oRequest.City = oCity;
                codeee = -275;
                oRequest.CommodityType = CommodityType;
                oRequest.TotalValue = TotalValue;
                oRequest.CommodityUnit = CommodityUnit;
                //oRequest.SecDate = SecDate;
                //oRequest.SecNumber = SecNumber;
                //oRequest.PerformNumber = PerformNumber;
                //oRequest.PerformDate = PerformDate;
                oRequest.CompanyName = CompanyName;
                oRequest.CompanyNationalCode = CompanyNationalCode;
                oRequest.RecordNumber = RecordNumber;
                oRequest.RecordDate = RecordDate;
                oRequest.InvoiceDate = DateTime.Now;
                oRequest.CellPhoneNumber = CellPhoneNumber;
                codeee = -276;

                oRequest.AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                codeee = -277;

                oRequest.CurrencyCode = oCurrency.Code;
                codeee = -278;

                oRequest.CurrencyValue = CurrencyValue;
                oRequest.BaseCurrencyValue = CurrencyValue;
                //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                codeee = -279;
                oRequest.CurrencyRation = oCurrency.Ratio;
                codeee = -280;

                oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                oRequest.IsActived = true;
                oRequest.IsDeleted = false;
                oRequest.IsSystem = false;
                oRequest.IsVerified = true;
                oRequest.Tariffs = 0;
                oRequest.LicenseNumber = string.Empty;
                oRequest.LicenseDate = DateTime.Now;
                oRequest.Description = Description;
                codeee = -24;

                oUnitOfWork.RequestRepository.Insert(oRequest);
                oUnitOfWork.Save();
                codeee = -23;

                #endregion
                #region Insert New Message
                Models.Message oMessage = new Models.Message();
                oMessage.UserId = oUser.Id;
                oMessage.LastState = oRequest.RequestState;
                oMessage.MessageText = MessageText;
                oMessage.NewState = oRequest.RequestState;
                oMessage.RequestId = oRequest.Id;
                codeee = -22;

                oUnitOfWork.MessageRepository.Insert(oMessage);
                oUnitOfWork.Save();
                #endregion
                #region Create File
                codeee = -21;
                Models.File newFile;
                if (FileList != null)
                {
                    foreach (var fileAddress in FileList)
                    {
                        newFile = new Models.File();
                        newFile.Id = Guid.NewGuid();
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = oRequest.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        codeee = -20;

                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                        codeee = -19;

                    }
                }
                #endregion
                codeee = -18;
                codeee = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                return codeee;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ex.Message, InputData, "Cert");
                message = ex.Message;
                return codeee;
            }
        }




        public int NewCert_Payment
            (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
            string CityCode
            , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            //, string SecNumber
            //, string SecDate
            , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, int SubSystem
            //, string PerformNumber
            //, string PerformDate
            //, decimal BaseCurrencyValue
            , List<string> FileList
            , out string message)
        {
            string InputData = string.Empty;
            int codeee = -50;
            try
            {
                var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceCode);
                codeee = -49;
                var oCity = oUnitOfWork.CityRepository.GetByCode(CityCode, oProvince.Id);
                codeee = -48;

                var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);
                codeee = -47;

                if (!(SubSystem == 3001 || SubSystem == 3002))
                {
                    message = "کد زیر سیستم باید 3001 یا 3002 باشد";
                    return codeee;
                }
                codeee = -475;


                var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                    .ToList()
                    .Where(current => current.Code == SubSystem)
                    .FirstOrDefault();
                codeee = -46;

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.ServiceTariff.RCode == "999")
                    .FirstOrDefault();
                codeee = -45;

                InputData
                    = "  *****  InputData *****"
                    + "||SubSystem:InsertImport "
                    + "||UserName: " + UserName
                    + "||CompanyName: " + CompanyName
                    + "||SubSystem: " + oSubSystem.Id
                    + "||CompanyNationalCode: " + CompanyNationalCode
                    + "||RecordNumber: " + RecordNumber;

                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);
                codeee = -44;

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || (!((CompanyNationalCode.Trim().Length == 11) || (CompanyNationalCode.Trim().Length == 10))))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                codeee = -43;

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                codeee = -42;

                if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                codeee = -41;

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion
                codeee = -40;

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);
                codeee = -39;

                int RilaCode = (int)Enums.CurrencyUnits.Rails;
                Models.Request oRequest = null;
                Models.Request oFindRequest = null;

                List<Models.Request> oList
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .Where(current => current.RecordDate == RecordDate)
                    .ToList()
                    ;
                codeee = -38;

                if (oCurrency.Code == RilaCode)
                    oFindRequest =
                        oList
                        .Where(current => current.CurrencyCode == RilaCode)
                        .OrderByDescending(current => current.InvoiceNumber)
                        .FirstOrDefault()
                        ;

                else
                    oFindRequest =
                        oList
                       .Where(current => current.CurrencyCode != RilaCode)
                       .OrderByDescending(current => current.InvoiceNumber)
                       .FirstOrDefault()
                       ;
                codeee = -37;

                if (oFindRequest != null &&
                    ((oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder && oCurrency.Code == RilaCode)
                    || (oFindRequest.RequestState < (int)Enums.RequestStates.PaymentOrder && oCurrency.Code != RilaCode)))
                {
                    #region Delete Last Record Files

                    foreach (var lastRow in oList)
                    {
                        lastRow.IsDeleted = true;
                        lastRow.IsActived = false;
                    }
                    oUnitOfWork.Save();

                    var allFileRow = oFindRequest.Files.ToList();
                    if (allFileRow != null)
                    {
                        foreach (var file in allFileRow)
                        {
                            file.IsDeleted = true;
                            file.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }

                    #endregion
                    codeee = -36;

                    #region Insert New Record
                    if (oCurrency.Code == RilaCode)
                    {
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -35;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                    }

                    else
                    {
                        codeee = -34;

                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -33;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                    }
                    #endregion
                    codeee = -32;

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oFindRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    codeee = -31;

                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion
                    codeee = -30;

                    #region Create File
                    Models.File newFile;
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = fileAddress.Split('&')[0].ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            codeee = -29;

                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                        }
                    }
                    #endregion
                }
                else if (oFindRequest != null &&
                    ((oFindRequest.RequestState < (int)Enums.RequestStates.PaymentOrder && oCurrency.Code == RilaCode)
                    || (oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder && oCurrency.Code != RilaCode)))
                {
                    message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                    codeee = -28;


                    return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                }
                else if (oFindRequest == null)
                {
                    codeee = -27;

                    #region Insert New Record
                    if (oCurrency.Code == RilaCode)
                    {
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -26;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                        codeee = -25;

                    }

                    else
                    {
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -24;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                        codeee = -23;

                    }
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    codeee = -22;

                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion
                    codeee = -21;

                    #region Create File
                    Models.File newFile;
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = fileAddress.Split('&')[0].ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            codeee = -20;

                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                            codeee = -19;

                        }
                    }
                    #endregion
                }

                //پرداخت انجام شده است
                else
                {
                    message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                    return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                }
                codeee = -18;

                var erererere = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                codeee = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                return codeee;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = ex.Message;
                return codeee;
            }
        }





        public int Cert_Payment
            (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
            string CityCode
            , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            //, string SecNumber
            //, string SecDate
            , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue
            //, string PerformNumber
            //, string PerformDate
            //, decimal BaseCurrencyValue
            , List<string> FileList
            , out string message)
        {
            string InputData = string.Empty;
            int codeee = -50;
            try
            {
                var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceCode);
                codeee = -49;
                var oCity = oUnitOfWork.CityRepository.GetByCode(CityCode, oProvince.Id);
                codeee = -48;

                var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);
                codeee = -47;

                var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                    .ToList()
                    .Where(current => current.Code == (int)Enums.SubSystems.Certificate)
                    .FirstOrDefault();
                codeee = -46;

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .FirstOrDefault();
                codeee = -45;

                InputData
                    = "  *****  InputData *****"
                    + "||SubSystem:InsertImport "
                    + "||UserName: " + UserName
                    + "||CompanyName: " + CompanyName
                    + "||SubSystem: " + oSubSystem.Id
                    + "||CompanyNationalCode: " + CompanyNationalCode
                    + "||RecordNumber: " + RecordNumber;

                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);
                codeee = -44;

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || (!((CompanyNationalCode.Trim().Length == 11) || (CompanyNationalCode.Trim().Length == 10))))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                codeee = -43;

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                codeee = -42;

                if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                codeee = -41;

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion
                codeee = -40;

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);
                codeee = -39;

                int RilaCode = (int)Enums.CurrencyUnits.Rails;
                Models.Request oRequest = null;
                Models.Request oFindRequest = null;

                List<Models.Request> oList
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .Where(current => current.RecordDate == RecordDate)
                    .ToList()
                    ;
                codeee = -38;

                if (oCurrency.Code == RilaCode)
                    oFindRequest =
                        oList
                        .Where(current => current.CurrencyCode == RilaCode)
                        .OrderByDescending(current => current.InvoiceNumber)
                        .FirstOrDefault()
                        ;

                else
                    oFindRequest =
                        oList
                       .Where(current => current.CurrencyCode != RilaCode)
                       .OrderByDescending(current => current.InvoiceNumber)
                       .FirstOrDefault()
                       ;
                codeee = -37;

                if (oFindRequest != null &&
                    ((oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder && oCurrency.Code == RilaCode)
                    || (oFindRequest.RequestState < (int)Enums.RequestStates.PaymentOrder && oCurrency.Code != RilaCode)))
                {
                    #region Delete Last Record Files

                    foreach (var lastRow in oList)
                    {
                        lastRow.IsDeleted = true;
                        lastRow.IsActived = false;
                    }
                    oUnitOfWork.Save();

                    var allFileRow = oFindRequest.Files.ToList();
                    if (allFileRow != null)
                    {
                        foreach (var file in allFileRow)
                        {
                            file.IsDeleted = true;
                            file.IsActived = false;
                        }
                        oUnitOfWork.Save();
                    }

                    #endregion
                    codeee = -36;

                    #region Insert New Record
                    if (oCurrency.Code == RilaCode)
                    {
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -35;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                    }

                    else
                    {
                        codeee = -34;

                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -33;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                    }
                    #endregion
                    codeee = -32;

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oFindRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    codeee = -31;

                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion
                    codeee = -30;

                    #region Create File
                    Models.File newFile;
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = fileAddress.Split('&')[0].ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            codeee = -29;

                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                        }
                    }
                    #endregion
                }
                else if (oFindRequest != null &&
                    ((oFindRequest.RequestState < (int)Enums.RequestStates.PaymentOrder && oCurrency.Code == RilaCode)
                    || (oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder && oCurrency.Code != RilaCode)))
                {
                    message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                    codeee = -28;


                    return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                }
                else if (oFindRequest == null)
                {
                    codeee = -27;

                    #region Insert New Record
                    if (oCurrency.Code == RilaCode)
                    {
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -26;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                        codeee = -25;

                    }

                    else
                    {
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        oRequest.SubSystemId = oSubSystem.Id;
                        oRequest.SubSystem = oSubSystem;
                        oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                        oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                        oRequest.ProvinceId = oProvince.Id;
                        oRequest.Province = oProvince;
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        //oRequest.SecDate = SecDate;
                        //oRequest.SecNumber = SecNumber;
                        //oRequest.PerformNumber = PerformNumber;
                        //oRequest.PerformDate = PerformDate;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyValue = CurrencyValue;
                        oRequest.BaseCurrencyValue = CurrencyValue;
                        //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.Tariffs = 0;
                        oRequest.LicenseNumber = string.Empty;
                        oRequest.LicenseDate = DateTime.Now;
                        codeee = -24;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                        codeee = -23;

                    }
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = oRequest.RequestState;
                    oMessage.RequestId = oRequest.Id;
                    codeee = -22;

                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion
                    codeee = -21;

                    #region Create File
                    Models.File newFile;
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = fileAddress.Split('&')[0].ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            codeee = -20;

                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                            codeee = -19;

                        }
                    }
                    #endregion
                }

                //پرداخت انجام شده است
                else
                {
                    message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                    return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                }
                codeee = -18;

                var erererere = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                codeee = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                return codeee;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = ex.Message;
                return codeee;
            }
        }

        public int Cert_Payment_State(string UserName, string Password, int InvoiceNumber, out string message)
        {
            #region Return Value Details
            // intRetValue=-4 -- UnDefind Error
            // intRetValue=-3 -- LoginFailed
            // intRetValue=-2 -- NotFound
            // intRetValue=-1 -- Request Incomplate
            // intRetValue= 0 -- Request Initialy
            // intRetValue= 1 -- Request PaymentOrder
            // intRetValue= 2 -- Request Payment
            // intRetValue= 3 -- Request PaymentConfirmation
            #endregion

            Models.Request oRequest = null;

            string InputData
                = "  *****  InputData *****"
                + "||UserName: " + UserName
                + "||Password: " + Password
                + "||InvoiceNumber: " + InvoiceNumber;

            #region Login Failed
            message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

            if (!string.IsNullOrEmpty(message))
            {
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -3;
            }
            #endregion

            try
            {
                oRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .FirstOrDefault()
                    ;

                //Not Found
                if (oRequest == null)
                {
                    message = string.Format(Resources.Message.Global.RecordNotFound, InvoiceNumber);
                    return -2;
                }

                else if (oRequest.RequestState == (int)Enums.RequestStates.PaymentConfirmation)
                {
                    message = oRequest.Bank_BankReciptNumber;
                    return (int)Enums.RequestStates.PaymentConfirmation;
                }

                else
                {
                    message =
                        oUnitOfWork.MessageRepository.Get()
                        .Where(current => current.RequestId == oRequest.Id)
                        .OrderByDescending(current => current.InsertDateTime).Select(x => x.MessageText).FirstOrDefault();
                    return oRequest.RequestState;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return -1;
            }
        }

        #endregion



        public int Lims_Payment
(string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
string CityCode, string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, string Description, List<string> FileList
, out string message)
        {
            string InputData = string.Empty;
            int codeee = -50;
            try
            {
                var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceCode);
                codeee = -49;
                var oCity = oUnitOfWork.CityRepository.GetByCode(CityCode, oProvince.Id);
                codeee = -48;

                var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode(CurrencyCode);
                codeee = -47;

                int SubSystem = (int)Enums.SubSystems.Lims; ///کد زیر سیستم باید 3003 باشد
                int ServiceTariff = 998;
                codeee = -469;

                if (ServiceTariff == 0)
                    ServiceTariff = 999;

                //codeee = -468;
                //if (SubSystem == 3002 && FileList.Count == 0)
                //{
                //    message = "ارسال فایل برای زیر سیستم گواهی حق ثبت اجباری میباشد";
                //    return codeee;
                //}

                codeee = -467;


                var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                    .ToList()
                    .Where(current => current.Code == SubSystem)
                    .FirstOrDefault();
                codeee = -46;

                string ServiceTariffs = ServiceTariff.ToString();

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.IsActived && !current.IsDeleted)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.ServiceTariff.RCode == ServiceTariffs)
                    .SingleOrDefault();

                codeee = -45;

                InputData
                        = "  *****  InputData *****"
                        + "||SubSystem:InsertImport "
                        + "||UserName: " + UserName
                        + "||Password: " + Password
                        + "||CompanyName: " + CompanyName
                        + "||CompanyNationalCode: " + CompanyNationalCode
                        + "||ProvinceCode: " + ProvinceCode
                        + "||CityCode: " + CityCode
                        + "||CommodityType: " + CommodityType
                        + "||TotalValue: " + TotalValue
                        + "||CommodityUnit: " + CommodityUnit
                        + "||RecordNumber: " + RecordNumber
                        + "||RecordDate: " + RecordDate
                        + "||CellPhoneNumber: " + CellPhoneNumber
                        + "||CurrencyCode: " + CurrencyCode
                        + "||CurrencyValue: " + CurrencyValue
                        + "||SubSystem: " + SubSystem
                        + "||ServiceTariff: " + ServiceTariff
                        + "||Description: " + Description;


                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);
                codeee = -44;

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || (!((CompanyNationalCode.Trim().Length == 11) || (CompanyNationalCode.Trim().Length == 10))))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }
                codeee = -43;

                if (string.IsNullOrEmpty(RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }

                if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }
                codeee = -42;

                if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }

                if (oCurrency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }
                codeee = -41;

                if (CurrencyValue < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -1;
                }
                #endregion
                codeee = -40;

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, "Lims");
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);
                codeee = -39;

                int RilaCode = (int)Enums.CurrencyUnits.Rails;
                Models.Request oRequest = null;
                Models.Request oFindRequest = null;

                List<Models.Request> oList
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .ToList()
                    ;
                codeee = -38;


                oFindRequest =
                 oList
                 .OrderByDescending(current => current.InvoiceNumber)
                 .FirstOrDefault()
                 ;

                codeee = -37;
                var MessageText = Resources.Message.Request.Message_InitialRequet;

                if (oFindRequest != null) // فاکتور از قبل وجود دارد
                {
                    if (oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder) // فاکتور هنوز پرداخت نشده است
                    {
                        #region Delete Last Record Files
                        // فاکتور های قبلی را حذف کند
                        foreach (var lastRow in oList)
                        {
                            lastRow.IsDeleted = true;
                            lastRow.IsActived = false;
                        }
                        oUnitOfWork.Save();

                        var allFileRow = oFindRequest.Files.ToList();
                        if (allFileRow != null)
                        {
                            foreach (var file in allFileRow)
                            {
                                file.IsDeleted = true;
                                file.IsActived = false;
                            }
                            oUnitOfWork.Save();
                        }

                        codeee = -36;
                        #endregion
                        MessageText = Resources.Message.Request.Message_Update;
                    }
                    else if (oFindRequest.RequestState > (int)Enums.RequestStates.PaymentOrder) // فاکتور پرداخت شده است
                    {
                        var amount = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                        if (amount != oFindRequest.AmountPaid)
                        {
                            return -100; // پرداخت انجام شده ولی مبلغ تغییر کرده است خطا بده
                        }

                        message = oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber.ToString();
                        return oUnitOfWork.RequestRepository.GetById(oFindRequest.Id).InvoiceNumber;
                    }
                }
                #region Insert New Record
                codeee = -27;
                oRequest = new Models.Request();
                codeee = -266;

                oRequest.UserId = oUser.Id;
                codeee = -267;

                oRequest.SubSystemId = oSubSystem.Id;
                codeee = -268;

                oRequest.SubSystem = oSubSystem;
                codeee = -269;
                oRequest.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                codeee = -270;
                oRequest.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                codeee = -271;
                oRequest.ProvinceId = oProvince.Id;
                codeee = -272;
                oRequest.Province = oProvince;
                codeee = -273;
                oRequest.CityId = oCity.Id;
                codeee = -274;
                oRequest.City = oCity;
                codeee = -275;
                oRequest.CommodityType = CommodityType;
                oRequest.TotalValue = TotalValue;
                oRequest.CommodityUnit = CommodityUnit;
                //oRequest.SecDate = SecDate;
                //oRequest.SecNumber = SecNumber;
                //oRequest.PerformNumber = PerformNumber;
                //oRequest.PerformDate = PerformDate;
                oRequest.CompanyName = CompanyName;
                oRequest.CompanyNationalCode = CompanyNationalCode;
                oRequest.RecordNumber = RecordNumber;
                oRequest.RecordDate = RecordDate;
                oRequest.InvoiceDate = DateTime.Now;
                oRequest.CellPhoneNumber = CellPhoneNumber;
                codeee = -276;

                oRequest.AmountPaid = Convert.ToInt64(CurrencyValue * oCurrency.Ratio);
                codeee = -277;

                oRequest.CurrencyCode = oCurrency.Code;
                codeee = -278;

                oRequest.CurrencyValue = CurrencyValue;
                oRequest.BaseCurrencyValue = CurrencyValue;
                //oRequest.BaseCurrencyValue = BaseCurrencyValue;
                codeee = -279;
                oRequest.CurrencyRation = oCurrency.Ratio;
                codeee = -280;

                oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                oRequest.IsActived = true;
                oRequest.IsDeleted = false;
                oRequest.IsSystem = false;
                oRequest.IsVerified = true;
                oRequest.Tariffs = 0;
                oRequest.LicenseNumber = string.Empty;
                oRequest.LicenseDate = DateTime.Now;
                oRequest.Description = Description;
                codeee = -24;

                oUnitOfWork.RequestRepository.Insert(oRequest);
                oUnitOfWork.Save();
                codeee = -23;

                #endregion
                #region Insert New Message
                Models.Message oMessage = new Models.Message();
                oMessage.UserId = oUser.Id;
                oMessage.LastState = oRequest.RequestState;
                oMessage.MessageText = MessageText;
                oMessage.NewState = oRequest.RequestState;
                oMessage.RequestId = oRequest.Id;
                codeee = -22;

                oUnitOfWork.MessageRepository.Insert(oMessage);
                oUnitOfWork.Save();
                #endregion
                #region Create File
                codeee = -21;
                Models.File newFile;
                if (FileList != null)
                {
                    foreach (var fileAddress in FileList)
                    {
                        newFile = new Models.File();
                        newFile.Id = Guid.NewGuid();
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = oRequest.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        codeee = -20;

                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                        codeee = -19;

                    }
                }
                #endregion
                codeee = -18;
                codeee = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
                return codeee;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ex.Message, InputData, "Cert");
                message = ex.Message;
                return codeee;
            }
        }





    }
}