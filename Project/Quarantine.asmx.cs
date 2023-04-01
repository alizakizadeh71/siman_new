using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace OPS
{
    /// <summary>
    /// Summary description for Quarantine
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Quarantine : System.Web.Services.WebService
    {
        DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

        //[WebMethod]
        //public int IVO_Quarantine_Insert
        //    (string UserName, string Password,string URLAddress, int SubSystemCode, string CompanyName, string CompanyNationalCode
        //    ,string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
        //    , string CellPhoneNumber, long AmountPaid,int Tariff,int Province, int City
        //    , List<string> FileList, out string message)
        //{

        //    #region Convert Code Qurantineh
        //    switch (SubSystemCode)
        //    {
        //        case 4:
        //        case 5:
        //        case 6:
        //        case 11:
        //            SubSystemCode = 2003;
        //            break;

        //        case 7:
        //            SubSystemCode = 2005;
        //            break;

        //        case 9:
        //            SubSystemCode = 2001;
        //            break;

        //        case 10:
        //            SubSystemCode = 2004;
        //            break;

        //        case 12:
        //            SubSystemCode = 2002;
        //            break;
        //    }
        //    #endregion

        //    string ProvinceString=string.Empty;
        //    if (Province < 10)
        //        ProvinceString = "0" + Province.ToString();

        //    else
        //        ProvinceString = Province.ToString();

        //    var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceString);

        //    var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode((int)Enums.CurrencyUnits.Rails);

        //    var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
        //        .ToList()
        //        .Where(current => current.Code == SubSystemCode)
        //        .FirstOrDefault();

        //    string InputData
        //        = "  *****  InputData *****"
        //        + "SubSystem:Quarantine " + Environment.NewLine
        //        + "UserName:" + UserName + Environment.NewLine
        //        + "URLAddress:" + URLAddress + Environment.NewLine
        //        + "SubSystemCode:" + SubSystemCode + Environment.NewLine
        //        + "CompanyName:" + CompanyName + Environment.NewLine
        //        + "CompanyNationalCode:" + CompanyNationalCode + Environment.NewLine
        //        + "CommodityType:" + CommodityType + Environment.NewLine
        //        + "TotalValue:" + TotalValue + Environment.NewLine
        //        + "RecordNumber:" + RecordNumber + Environment.NewLine
        //        + "RecordDate:" + RecordDate + Environment.NewLine
        //        + "CellPhoneNumber:" + CellPhoneNumber + Environment.NewLine
        //        + "AmountPaid:" + AmountPaid + Environment.NewLine
        //        + "Province:" + Province + Environment.NewLine
        //        + "City:" + City + Environment.NewLine
        //        + "FileList:" + FileList + Environment.NewLine
        //        ;

        //    try
        //    {
        //        // Return -1
        //        #region Input Data Verified
        //        message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

        //        if (oSubSystem == null)
        //        {
        //            message = "زیر سیستم وجود ندارد";
        //            Infrastructure.Utility.InsertErrorLog(SubSystemCode.ToString(), message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (!string.IsNullOrEmpty(message))
        //        {
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (string.IsNullOrEmpty(CompanyName))
        //        {
        //            message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (string.IsNullOrEmpty(CompanyNationalCode) || CompanyNationalCode.Trim().Length != 11)
        //        {
        //            message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (string.IsNullOrEmpty(RecordNumber))
        //        {
        //            message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
        //        {
        //            message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
        //        {
        //            message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }

        //        if (AmountPaid < 0)
        //        {
        //            message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -1;
        //        }
        //        #endregion

        //        // Return -3
        //        #region Login Failed
        //        message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

        //        if (!string.IsNullOrEmpty(message))
        //        {
        //            Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
        //            return -3;
        //        }
        //        #endregion

        //        Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

        //        Models.Request oRequest = null;

        //        Models.Request oFindRequest =
        //            oUnitOfWork.RequestRepository.Get()
        //            .Where(current => current.IsDeleted == false)
        //            .Where(current => current.IsActived == true)
        //            .Where(current => current.SubSystemId == oSubSystem.Id)
        //            .Where(current => current.CompanyNationalCode == CompanyNationalCode)
        //            .Where(curren => curren.RecordNumber == RecordNumber)
        //            //.Where(current => current.RecordDate == RecordDate)
        //            .OrderByDescending(current => current.InvoiceNumber)
        //            .FirstOrDefault()
        //            ;

        //        if (oFindRequest != null && oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder)
        //        {
        //            #region Delete Last Record
        //            oFindRequest.IsDeleted = true;
        //            oFindRequest.IsActived = false;

        //            var allFileRow = oFindRequest.Files.ToList();

        //            if (allFileRow != null)
        //            {
        //                foreach (var file in allFileRow)
        //                {
        //                    file.IsDeleted = true;
        //                    file.IsActived = false;
        //                }
        //            }
        //            #endregion

        //            #region Insert New Record
        //            oRequest = new Models.Request();
        //            oRequest.UserId = oUser.Id;
        //            oRequest.SubSystemId = oSubSystem.Id;
        //            oRequest.ProvinceId = oProvince.Id;
        //            oRequest.CommodityType = CommodityType;
        //            oRequest.TotalValue = TotalValue;
        //            oRequest.CommodityUnit = CommodityUnit;
        //            oRequest.SecDate = null;
        //            oRequest.SecNumber = null;
        //            oRequest.PerformNumber = null;
        //            oRequest.PerformDate = null;
        //            oRequest.CompanyName = CompanyName;
        //            oRequest.CompanyNationalCode = CompanyNationalCode;
        //            oRequest.RecordNumber = RecordNumber;
        //            oRequest.RecordDate = RecordDate;
        //            oRequest.InvoiceDate = DateTime.Now;
        //            oRequest.CellPhoneNumber = CellPhoneNumber;
        //            oRequest.AmountPaid = AmountPaid;
        //            oRequest.CurrencyCode = oCurrency.Code;
        //            oRequest.CurrencyRation = oCurrency.Ratio;
        //            oRequest.CurrencyValue = AmountPaid;
        //            oRequest.BaseCurrencyValue = AmountPaid;
        //            oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
        //            oRequest.IsActived = true;
        //            oRequest.IsDeleted = false;
        //            oRequest.IsSystem = false;
        //            oRequest.IsVerified = true;
        //            oRequest.URLAddress = URLAddress;

        //            oUnitOfWork.RequestRepository.Insert(oRequest);
        //            oUnitOfWork.Save();
        //            #endregion

        //            //#region Move Last Message
        //            //if (oFindRequest.Messages.ToList().Count > 0)
        //            //{
        //            //    foreach (var row in oFindRequest.Messages.ToList())
        //            //    {
        //            //        row.RequestId = oRequest.Id;
        //            //    }

        //            //}
        //            //#endregion

        //            #region Insert New Message
        //            Models.Message oMessage = new Models.Message();
        //            oMessage.UserId = oUser.Id;
        //            oMessage.LastState = oFindRequest.RequestState;
        //            oMessage.MessageText = Resources.Message.Request.Message_Update;
        //            oMessage.NewState=oRequest.RequestState;
        //            oMessage.RequestId=oRequest.Id;
        //            oUnitOfWork.MessageRepository.Insert(oMessage);
        //            oUnitOfWork.Save();
        //            #endregion

        //            #region Create File
        //            Models.File newFile;
        //            if (FileList != null)
        //            {
        //                foreach (var fileAddress in FileList)
        //                {
        //                    newFile = new Models.File();
        //                    newFile.Id = Guid.NewGuid();
        //                    newFile.FileAddress = fileAddress;
        //                    newFile.InsertDateTime = DateTime.Now;
        //                    newFile.IsActived = true;
        //                    newFile.IsDeleted = false;
        //                    newFile.IsSystem = true;
        //                    newFile.IsVerified = true;
        //                    newFile.Name = newFile.Id.ToString();
        //                    newFile.RequestId = oRequest.Id;
        //                    newFile.UpdateDateTime = DateTime.Now;
        //                    oUnitOfWork.FileRepository.Insert(newFile);
        //                    oUnitOfWork.Save();
        //                }
        //            }
        //            #endregion
        //        }

        //        if (oFindRequest != null && oFindRequest.RequestState > (int)Enums.RequestStates.PaymentOrder)
        //        {
        //            oRequest = oFindRequest;
        //        }

        //        else if (oFindRequest == null)
        //        {
        //            #region InsertNew Record
        //            oRequest = new Models.Request();
        //            oRequest.UserId = oUser.Id;
        //            oRequest.SubSystemId = oSubSystem.Id;
        //            oRequest.ProvinceId = oProvince.Id;
        //            oRequest.CommodityType = CommodityType;
        //            oRequest.TotalValue = TotalValue;
        //            oRequest.CommodityUnit = CommodityUnit;
        //            oRequest.SecDate = null;
        //            oRequest.SecNumber = null;
        //            oRequest.PerformNumber = null;
        //            oRequest.PerformDate = null;
        //            oRequest.CompanyName = CompanyName;
        //            oRequest.CompanyNationalCode = CompanyNationalCode;
        //            oRequest.RecordNumber = RecordNumber;
        //            oRequest.RecordDate = RecordDate;
        //            oRequest.InvoiceDate = DateTime.Now;
        //            oRequest.CellPhoneNumber = CellPhoneNumber;
        //            oRequest.AmountPaid = AmountPaid;
        //            oRequest.CurrencyCode = oCurrency.Code;
        //            oRequest.CurrencyRation = oCurrency.Ratio;
        //            oRequest.CurrencyValue = AmountPaid;
        //            oRequest.BaseCurrencyValue = AmountPaid;
        //            oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
        //            oRequest.IsActived = true;
        //            oRequest.IsDeleted = false;
        //            oRequest.IsSystem = false;
        //            oRequest.IsVerified = true;
        //            oRequest.URLAddress = URLAddress;

        //            oUnitOfWork.RequestRepository.Insert(oRequest);
        //            #endregion

        //            #region Insert New Message
        //            Models.Message oMessage = new Models.Message();
        //            oMessage.UserId = oUser.Id;
        //            oMessage.LastState = oRequest.RequestState;
        //            oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
        //            oMessage.NewState = oRequest.RequestState;
        //            oMessage.RequestId = oRequest.Id;
        //            oUnitOfWork.MessageRepository.Insert(oMessage);
        //            oUnitOfWork.Save();
        //            #endregion

        //            #region Create File
        //            Models.File newFile;
        //            if (FileList != null)
        //            {
        //                foreach (var fileAddress in FileList)
        //                {
        //                    newFile = new Models.File();
        //                    newFile.Id = Guid.NewGuid();
        //                    newFile.FileAddress = fileAddress;
        //                    newFile.InsertDateTime = DateTime.Now;
        //                    newFile.IsActived = true;
        //                    newFile.IsDeleted = false;
        //                    newFile.IsSystem = true;
        //                    newFile.IsVerified = true;
        //                    newFile.Name = newFile.Id.ToString();
        //                    newFile.RequestId = oRequest.Id;
        //                    newFile.UpdateDateTime = DateTime.Now;
        //                    oUnitOfWork.FileRepository.Insert(newFile);
        //                    oUnitOfWork.Save();
        //                }
        //            }
        //            #endregion
        //        }

        //        message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();
        //        return oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
        //    }

        //    catch (Exception ex)
        //    {
        //        string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
        //        Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
        //        message = Resources.Message.Global.UnknownError;
        //        return -4;
        //    }
        //}

        [WebMethod]
        public int IVO_Quarantine_Insert
            (string UserName, string Password, string URLAddress, int SubSystemCode, string CompanyName, string CompanyNationalCode
            , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            , string CellPhoneNumber, long AmountPaid, int Province, int City
            , List<string> FileList, out string message)
        {

            if (AmountPaid > 0 && AmountPaid < 10000)
                AmountPaid = 10000;

            #region Convert Code Qurantineh
            switch (SubSystemCode)
            {
                case 4:
                case 5:
                case 6:
                case 11:
                    SubSystemCode = 2003;
                    break;

                case 7:
                    SubSystemCode = 2005;
                    break;

                case 9:
                    SubSystemCode = 2001;
                    break;

                case 10:
                    SubSystemCode = 2004;
                    break;

                case 12:
                    SubSystemCode = 2002;
                    break;
            }
            #endregion

            string ProvinceString = string.Empty;
            if (Province < 10)
                ProvinceString = "0" + Province.ToString();

            else
                ProvinceString = Province.ToString();

            var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceString);

            var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode((int)Enums.CurrencyUnits.Rails);

            var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                .Where(current => current.Code == SubSystemCode)
                .FirstOrDefault();

            var oCommodity = oUnitOfWork.CommodityRepository.Get()
                .Where(current => current.Name == CommodityType)
                .FirstOrDefault();

            var oCommodityInSubSystem = oUnitOfWork.CommodityInSubSystemRepository.Get()
                .Where(current => oCommodity != null ? current.CommodityId == oCommodity.Id : true)
                .Where(current => current.SubSystemId == oSubSystem.Id)
                .FirstOrDefault();

            string InputData
                = "  *****  InputData *****"
                + "SubSystem:Quarantine " + Environment.NewLine
                + "UserName:" + UserName + Environment.NewLine
                + "URLAddress:" + URLAddress + Environment.NewLine
                + "SubSystemCode:" + SubSystemCode + Environment.NewLine
                + "CompanyName:" + CompanyName + Environment.NewLine
                + "CompanyNationalCode:" + CompanyNationalCode + Environment.NewLine
                + "CommodityCode:" + CommodityType + Environment.NewLine
                + "CommodityType:" + CommodityType + Environment.NewLine
                + "TotalValue:" + TotalValue + Environment.NewLine
                + "RecordNumber:" + RecordNumber + Environment.NewLine
                + "RecordDate:" + RecordDate + Environment.NewLine
                + "CellPhoneNumber:" + CellPhoneNumber + Environment.NewLine
                + "AmountPaid:" + AmountPaid + Environment.NewLine
                + "Province:" + Province + Environment.NewLine
                + "City:" + City + Environment.NewLine
                + "FileList:" + FileList + Environment.NewLine
                ;

            try
            {
                // Return -1
                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (oSubSystem == null)
                {
                    message = "زیر سیستم وجود ندارد";
                    Infrastructure.Utility.InsertErrorLog(SubSystemCode.ToString(), message, InputData, string.Empty);
                    return -1;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -3;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || (!(CompanyNationalCode.Trim().Length == 11 || CompanyNationalCode.Trim().Length == 8)))
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

                if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (AmountPaid < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
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
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    //.Where(current => current.RecordDate == RecordDate)
                    .OrderByDescending(current => current.InvoiceNumber)
                    .FirstOrDefault()
                    ;

                if (oFindRequest != null && oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder)
                {
                    #region Delete Last Record
                    oFindRequest.IsDeleted = true;
                    oFindRequest.IsActived = false;

                    var allFileRow = oFindRequest.Files.ToList();

                    if (allFileRow != null)
                    {
                        foreach (var file in allFileRow)
                        {
                            file.IsDeleted = true;
                            file.IsActived = false;
                        }
                    }
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oCommodityInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oCommodityInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = null;
                    oRequest.SecNumber = null;
                    oRequest.PerformNumber = null;
                    oRequest.PerformDate = null;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = AmountPaid;
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.CurrencyValue = AmountPaid;
                    oRequest.BaseCurrencyValue = AmountPaid;
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.URLAddress = URLAddress;

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
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress;
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = newFile.Id.ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                        }
                    }
                    #endregion
                }

                if (oFindRequest != null && oFindRequest.RequestState > (int)Enums.RequestStates.PaymentOrder)
                {
                    oRequest = oFindRequest;
                }

                else if (oFindRequest == null)
                {
                    #region InsertNew Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oCommodityInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oCommodityInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = null;
                    oRequest.SecNumber = null;
                    oRequest.PerformNumber = null;
                    oRequest.PerformDate = null;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = AmountPaid;
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.CurrencyValue = AmountPaid;
                    oRequest.BaseCurrencyValue = AmountPaid;
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.URLAddress = URLAddress;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
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
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress;
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = newFile.Id.ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                        }
                    }
                    #endregion
                }

                message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();
                return oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber;
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = Resources.Message.Global.UnknownError;
                return -4;
            }
        }


        [WebMethod]
        public int IVO_Quarantine_Insert_New
            (string UserName, string Password, string URLAddress, int SubSystemCode, string CompanyName, string CompanyNationalCode
            , int CommodityCode, string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            , string CellPhoneNumber, long AmountPaid, int Province, int City
            , List<string> FileList, out string message)
        {
            int ErrorCodeGharanrine = 0;
            try
            {
                ErrorCodeGharanrine = 1;
                if (AmountPaid >= 0 && AmountPaid < 10000)
                    AmountPaid = 10000;
                ErrorCodeGharanrine = 2;
                #region Convert Code Qurantineh
                switch (SubSystemCode)
                {
                    case 4:
                    case 5:
                    case 6:
                    case 11:
                        SubSystemCode = 2003;
                        break;

                    case 7:
                        SubSystemCode = 2005;
                        break;

                    case 9:
                        SubSystemCode = 2001;
                        break;

                    case 10:
                        SubSystemCode = 2004;
                        break;

                    case 12:
                        SubSystemCode = 2002;
                        break;
                }
                ErrorCodeGharanrine = 3;
                #endregion

                string ProvinceString = string.Empty;
                if (Province < 10)
                    ProvinceString = "0" + Province.ToString();

                else
                    ProvinceString = Province.ToString();
                ErrorCodeGharanrine = 4;
                string CityString = string.Empty;
                if (City < 10)
                    CityString = "0" + City.ToString();

                else
                    CityString = City.ToString();
                ErrorCodeGharanrine = 5;

                var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceString);
                ErrorCodeGharanrine = 6;

                var oCity = oUnitOfWork.CityRepository.GetByCode(CityString, oProvince.Id);
                ErrorCodeGharanrine = 7;

                var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode((int)Enums.CurrencyUnits.Rails);
                ErrorCodeGharanrine = 8;

                var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                    .Where(current => current.Code == SubSystemCode)
                    .FirstOrDefault();
                ErrorCodeGharanrine = 9;

                var oCommodity = oUnitOfWork.CommodityRepository.Get()
                    .Where(current => current.Code == CommodityCode)
                    .FirstOrDefault();
                ErrorCodeGharanrine = 10;

                var oCommodityInSubSystem = oUnitOfWork.CommodityInSubSystemRepository.Get()
                    .Where(current => current.CommodityId == oCommodity.Id)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .FirstOrDefault();
                ErrorCodeGharanrine = 11;

                string InputData
                    = "  *****  InputData *****"
                    + "SubSystem:Quarantine " + Environment.NewLine
                    + "UserName:" + UserName + Environment.NewLine
                    + "URLAddress:" + URLAddress + Environment.NewLine
                    + "SubSystemCode:" + SubSystemCode + Environment.NewLine
                    + "CompanyName:" + CompanyName + Environment.NewLine
                    + "CompanyNationalCode:" + CompanyNationalCode + Environment.NewLine
                    + "CommodityCode:" + CommodityCode + Environment.NewLine
                    + "CommodityType:" + CommodityType + Environment.NewLine
                    + "TotalValue:" + TotalValue + Environment.NewLine
                    + "CommodityUnit:" + CommodityUnit + Environment.NewLine
                    + "RecordNumber:" + RecordNumber + Environment.NewLine
                    + "RecordDate:" + RecordDate + Environment.NewLine
                    + "CellPhoneNumber:" + CellPhoneNumber + Environment.NewLine
                    + "AmountPaid:" + AmountPaid + Environment.NewLine
                    + "Province:" + Province + Environment.NewLine
                    + "City:" + City + Environment.NewLine
                    + "FileList:" + FileList + Environment.NewLine
                    ;
                ErrorCodeGharanrine = 12;

                try
                {
                    // Return -1
                    #region Input Data Verified
                    message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                    if (oSubSystem == null)
                    {
                        message = "زیر سیستم وجود ندارد";
                        Infrastructure.Utility.InsertErrorLog(SubSystemCode.ToString(), message, InputData, string.Empty);
                        return -1;
                    }
                    ErrorCodeGharanrine = 13;

                    if (!string.IsNullOrEmpty(message))
                    {
                        Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                        return -3;
                    }

                    if (string.IsNullOrEmpty(CompanyName))
                    {
                        message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                        Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                        return -1;
                    }

                    if (string.IsNullOrEmpty(CompanyNationalCode) || (!(CompanyNationalCode.Trim().Length == 11 || CompanyNationalCode.Trim().Length == 8)))
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
                    ErrorCodeGharanrine = 14;

                    if (string.IsNullOrEmpty(RecordDate) || RecordDate.Trim().Length != 10)
                    {
                        message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                        Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                        return -1;
                    }

                    if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                    {
                        message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                        Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                        return -1;
                    }

                    if (AmountPaid < 0)
                    {
                        message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                        Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                        return -1;
                    }
                    #endregion
                    ErrorCodeGharanrine = 15;

                    Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);
                    ErrorCodeGharanrine = 151;

                    Models.Request oRequest = null;

                    Models.Request oFindRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        //.Where(current => current.RecordDate == RecordDate)
                        .OrderByDescending(current => current.InsertDateTime)
                        .FirstOrDefault()
                        ;
                    ErrorCodeGharanrine = 152;

                    if (oFindRequest != null && oFindRequest.AmountPaid == AmountPaid && oFindRequest.CommodityUnit == CommodityUnit)
                    {
                        oRequest = oFindRequest;
                    }

                    else if (oFindRequest != null && oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder)
                    {
                        ErrorCodeGharanrine = 153;

                        #region Delete Last Record
                        oFindRequest.IsDeleted = true;
                        oFindRequest.IsActived = false;

                        var allFileRow = oFindRequest.Files.ToList();

                        if (allFileRow != null)
                        {
                            foreach (var file in allFileRow)
                            {
                                file.IsDeleted = true;
                                file.IsActived = false;
                            }
                        }
                        oUnitOfWork.Save();
                        #endregion
                        ErrorCodeGharanrine = 154;

                        #region Insert New Record
                        oRequest = new Models.Request();
                        ErrorCodeGharanrine = 155;

                        oRequest.UserId = oUser.Id;
                        ErrorCodeGharanrine = 156;

                        oRequest.SubSystemId = oSubSystem.Id;
                        ErrorCodeGharanrine = 157;

                        oRequest.SubSystem = oSubSystem;
                        ErrorCodeGharanrine = 158;
                        oRequest.ServiceTariffId = oCommodityInSubSystem.ServiceTariffId;
                        ErrorCodeGharanrine = 159;
                        oRequest.ServiceTariff = oCommodityInSubSystem.ServiceTariff;
                        ErrorCodeGharanrine = 160;
                        oRequest.ProvinceId = oProvince.Id;
                        ErrorCodeGharanrine = 161;
                        oRequest.Province = oProvince;
                        if (oCity != null)
                        {
                            oRequest.CityId = oCity.Id;
                            oRequest.City = oCity;
                        }
                        else
                        {
                            oRequest.Browser = City.ToString();
                        }
                        ErrorCodeGharanrine = 162;

                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        oRequest.SecDate = null;
                        oRequest.SecNumber = null;
                        oRequest.PerformNumber = null;
                        oRequest.PerformDate = null;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = AmountPaid;
                        ErrorCodeGharanrine = 163;

                        oRequest.CurrencyCode = oCurrency.Code;
                        ErrorCodeGharanrine = 164;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.CurrencyValue = AmountPaid;
                        oRequest.BaseCurrencyValue = AmountPaid;
                        oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.URLAddress = URLAddress;
                        ErrorCodeGharanrine = 165;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        oUnitOfWork.Save();
                        #endregion
                        ErrorCodeGharanrine = 166;

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
                        ErrorCodeGharanrine = 167;

                        #region Create File
                        Models.File newFile;
                        if (FileList != null)
                        {
                            foreach (var fileAddress in FileList)
                            {
                                newFile = new Models.File();
                                newFile.Id = Guid.NewGuid();
                                newFile.FileAddress = fileAddress;
                                newFile.InsertDateTime = DateTime.Now;
                                newFile.IsActived = true;
                                newFile.IsDeleted = false;
                                newFile.IsSystem = true;
                                newFile.IsVerified = true;
                                newFile.Name = newFile.Id.ToString();
                                newFile.RequestId = oRequest.Id;
                                newFile.UpdateDateTime = DateTime.Now;
                                oUnitOfWork.FileRepository.Insert(newFile);
                                oUnitOfWork.Save();
                            }
                        }
                        ErrorCodeGharanrine = 168;

                        #endregion
                    }

                    else if (oFindRequest != null && oFindRequest.RequestState > (int)Enums.RequestStates.PaymentOrder)
                    {
                        oRequest = oFindRequest;
                    }

                    else if (oFindRequest == null)
                    {
                    ErrorCodeGharanrine = 17;
                        #region InsertNew Record
                        oRequest = new Models.Request();
                        oRequest.UserId = oUser.Id;
                        ErrorCodeGharanrine = 171;

                        oRequest.SubSystemId = oSubSystem.Id;
                        ErrorCodeGharanrine = 172;

                        oRequest.SubSystem = oSubSystem;
                        ErrorCodeGharanrine = 173;

                        oRequest.ServiceTariffId = oCommodityInSubSystem.ServiceTariffId;
                        ErrorCodeGharanrine = 174;

                        oRequest.ServiceTariff = oCommodityInSubSystem.ServiceTariff;
                        ErrorCodeGharanrine = 175;

                        oRequest.ProvinceId = oProvince.Id;
                        ErrorCodeGharanrine = 176;

                        oRequest.Province = oProvince;
                        if (oCity != null)
                        {
                            oRequest.CityId = oCity.Id;
                            oRequest.City = oCity;
                        }
                        else
                        {
                            oRequest.Browser = City.ToString();
                        }
                        oRequest.CommodityType = CommodityType;
                        oRequest.TotalValue = TotalValue;
                        oRequest.CommodityUnit = CommodityUnit;
                        oRequest.SecDate = null;
                        oRequest.SecNumber = null;
                        oRequest.PerformNumber = null;
                        oRequest.PerformDate = null;
                        oRequest.CompanyName = CompanyName;
                        oRequest.CompanyNationalCode = CompanyNationalCode;
                        oRequest.RecordNumber = RecordNumber;
                        oRequest.RecordDate = RecordDate;
                        oRequest.InvoiceDate = DateTime.Now;
                        oRequest.CellPhoneNumber = CellPhoneNumber;
                        oRequest.AmountPaid = AmountPaid;
                        ErrorCodeGharanrine = 177;

                        oRequest.CurrencyCode = oCurrency.Code;
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        oRequest.CurrencyValue = AmountPaid;
                        oRequest.BaseCurrencyValue = AmountPaid;
                        oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                        oRequest.IsActived = true;
                        oRequest.IsDeleted = false;
                        oRequest.IsSystem = false;
                        oRequest.IsVerified = true;
                        oRequest.URLAddress = URLAddress;
                        ErrorCodeGharanrine = 178;

                        oUnitOfWork.RequestRepository.Insert(oRequest);
                        #endregion
                        ErrorCodeGharanrine = 179;

                        #region Insert New Message
                        Models.Message oMessage = new Models.Message();
                        oMessage.UserId = oUser.Id;
                        oMessage.LastState = oRequest.RequestState;
                        oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                        oMessage.NewState = oRequest.RequestState;
                        oMessage.RequestId = oRequest.Id;
                        ErrorCodeGharanrine = 180;

                        oUnitOfWork.MessageRepository.Insert(oMessage);
                        oUnitOfWork.Save();
                        #endregion
                        ErrorCodeGharanrine = 181;

                        #region Create File
                        Models.File newFile;
                        if (FileList != null)
                        {
                            foreach (var fileAddress in FileList)
                            {
                                newFile = new Models.File();
                                newFile.Id = Guid.NewGuid();
                                newFile.FileAddress = fileAddress;
                                newFile.InsertDateTime = DateTime.Now;
                                newFile.IsActived = true;
                                newFile.IsDeleted = false;
                                newFile.IsSystem = true;
                                newFile.IsVerified = true;
                                newFile.Name = newFile.Id.ToString();
                                newFile.RequestId = oRequest.Id;
                                newFile.UpdateDateTime = DateTime.Now;
                                oUnitOfWork.FileRepository.Insert(newFile);
                                oUnitOfWork.Save();
                            }
                        }
                        #endregion
                    }
                    ErrorCodeGharanrine = 182;

                    message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();

                    ErrorCodeGharanrine = 183;

                    // برای حذف موارد تکراری
                    List<Models.Request> oDeleteRequest =
                        oUnitOfWork.RequestRepository.Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.SubSystemId == oSubSystem.Id)
                        .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                        .Where(curren => curren.RecordNumber == RecordNumber)
                        .Where(curren => curren.Id != oRequest.Id)
                        .Where(curren => curren.RequestState == 1)
                        .ToList()
                        ;
                    ErrorCodeGharanrine = 19;

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

                catch (Exception ex)
                {
                    string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                    Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                    message = Resources.Message.Global.UnknownError;
                    return ErrorCodeGharanrine;
                }
            }
            catch (Exception ex)
            {
                message = "خطا - اطلاعات به سامانه پرداخت آنلاین ناقص ارسال شده است";
                return ErrorCodeGharanrine;
            }
        }


        [WebMethod]
        public int IVO_Quarantine_Insert_960612
            (string UserName, string Password, string URLAddress, int SubSystemCode, string CompanyName, string CompanyNationalCode
            , int CommodityCode, string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            , string CellPhoneNumber, long AmountPaid, int Province, int City, int Tariffs, string LicenseNumber, DateTime LicenseDate
            , List<string> FileList, out string message)
        {
            if (AmountPaid > 0 && AmountPaid < 10000)
                AmountPaid = 10000;

            #region Insert InputData
            string InputData
                = "  *****  InputData *****"
                + "SubSystem:Quarantine " + Environment.NewLine
                + "UserName:" + UserName + Environment.NewLine
                + "URLAddress:" + URLAddress + Environment.NewLine
                + "SubSystemCode:" + SubSystemCode + Environment.NewLine
                + "CompanyName:" + CompanyName + Environment.NewLine
                + "CompanyNationalCode:" + CompanyNationalCode + Environment.NewLine
                + "CommodityCode:" + CommodityCode + Environment.NewLine
                + "CommodityType:" + CommodityType + Environment.NewLine
                + "TotalValue:" + TotalValue + Environment.NewLine
                + "RecordNumber:" + RecordNumber + Environment.NewLine
                + "RecordDate:" + RecordDate + Environment.NewLine
                + "CellPhoneNumber:" + CellPhoneNumber + Environment.NewLine
                + "AmountPaid:" + AmountPaid + Environment.NewLine
                + "Province:" + Province + Environment.NewLine
                + "City:" + City + Environment.NewLine
                + "Tariffs:" + Tariffs + Environment.NewLine
                + "LicenseNumber:" + LicenseNumber + Environment.NewLine
                + "LicenseDate:" + LicenseDate + Environment.NewLine
                + "FileList:" + FileList + Environment.NewLine
                + "Tariffs:" + Tariffs + Environment.NewLine
                + "LicenseNumber:" + LicenseNumber + Environment.NewLine
                + "LicenseDate:" + LicenseDate + Environment.NewLine
                ;
            #endregion

            #region Convert Code Qurantineh
            switch (SubSystemCode)
            {
                case 4:
                case 5:
                case 6:
                case 11:
                    SubSystemCode = 2003;
                    break;

                case 7:
                    SubSystemCode = 2005;
                    break;

                case 9:
                    SubSystemCode = 2001;
                    break;

                case 10:
                    SubSystemCode = 2004;
                    break;

                case 12:
                    SubSystemCode = 2002;
                    break;
            }
            #endregion

            string ProvinceString = string.Empty;

            if (Province < 10)
                ProvinceString = "0" + Province.ToString();

            else
                ProvinceString = Province.ToString();

            var oProvince = oUnitOfWork.ProvinceRepository.GetByCode(ProvinceString);



            string CityString = string.Empty;
            if (City < 10)
                CityString = "0" + City.ToString();

            else
                CityString = City.ToString();

            var oCity = oUnitOfWork.CityRepository.GetByCode(CityString, oProvince.Id);


            var oCurrency = oUnitOfWork.CurrencyUnitRepository.GetByCode((int)Enums.CurrencyUnits.Rails);

            #region SubSystem
            var oSubSystem = oUnitOfWork.SubSystemRepository.Get()
                .Where(current => current.Code == SubSystemCode)
                .FirstOrDefault();


            if (oSubSystem == null)
            {
                message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.SubSystem);
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -1;
            }
            #endregion

            #region Commodity
            var oCommodity = oUnitOfWork.CommodityRepository.Get()
                .Where(current => current.Code == CommodityCode)
                .FirstOrDefault();


            if (oCommodity == null)
            {
                message = string.Format(Resources.Message.Global.MissDataFormat, "عنوان کالا");
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -1;
            }
            #endregion

            #region Commodity in subsystem

            var oCommodityInSubSystem = oUnitOfWork.CommodityInSubSystemRepository.Get()
                .Where(current => current.CommodityId == oCommodity.Id)
                .Where(current => current.SubSystemId == oSubSystem.Id)
                .FirstOrDefault();

            if (oCommodityInSubSystem == null)
            {
                message = "تعرفه کالا در سیستم پرداخت ثبت نشده است";
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -1;
            }
            #endregion

            try
            {
                // Return -1
                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (oSubSystem == null)
                {
                    message = "زیر سیستم وجود ندارد";
                    Infrastructure.Utility.InsertErrorLog(SubSystemCode.ToString(), message, InputData, string.Empty);
                    return -1;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -3;
                }

                if (string.IsNullOrEmpty(CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(CompanyNationalCode) || (!(CompanyNationalCode.Trim().Length == 11 || CompanyNationalCode.Trim().Length == 8)))
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

                if (string.IsNullOrEmpty(CellPhoneNumber) || CellPhoneNumber.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (AmountPaid < 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
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
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .OrderByDescending(current => current.InsertDateTime)
                    .FirstOrDefault()
                    ;

                if (oFindRequest != null && oFindRequest.AmountPaid == AmountPaid && oFindRequest.CommodityUnit == CommodityUnit)
                {
                    oRequest = oFindRequest;
                }

                else if (oFindRequest == null)
                {
                    #region InsertNew Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oCommodityInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oCommodityInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    if (oCity != null)
                    {
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                    }
                    else
                    {
                        oRequest.Browser = City.ToString();
                    }
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = null;
                    oRequest.SecNumber = null;
                    oRequest.PerformNumber = null;
                    oRequest.PerformDate = null;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = AmountPaid;
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.CurrencyValue = AmountPaid;
                    oRequest.BaseCurrencyValue = AmountPaid;
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.URLAddress = URLAddress;
                    oRequest.Tariffs = Tariffs;
                    oRequest.LicenseNumber = LicenseNumber;
                    oRequest.LicenseDate = LicenseDate;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
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
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress;
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = newFile.Id.ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                        }
                    }
                    #endregion
                }

                else if (oFindRequest != null && oFindRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder)
                {
                    #region Delete Last Record
                    oFindRequest.IsDeleted = true;
                    oFindRequest.IsActived = false;

                    var allFileRow = oFindRequest.Files.ToList();

                    if (allFileRow != null)
                    {
                        foreach (var file in allFileRow)
                        {
                            file.IsDeleted = true;
                            file.IsActived = false;
                        }
                    }
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Record
                    oRequest = new Models.Request();
                    oRequest.UserId = oUser.Id;
                    oRequest.SubSystemId = oSubSystem.Id;
                    oRequest.SubSystem = oSubSystem;
                    oRequest.ServiceTariffId = oCommodityInSubSystem.ServiceTariffId;
                    oRequest.ServiceTariff = oCommodityInSubSystem.ServiceTariff;
                    oRequest.ProvinceId = oProvince.Id;
                    oRequest.Province = oProvince;
                    if (oCity != null)
                    {
                        oRequest.CityId = oCity.Id;
                        oRequest.City = oCity;
                    }
                    else
                    {
                        oRequest.Browser = City.ToString();
                    }
                    oRequest.CommodityType = CommodityType;
                    oRequest.TotalValue = TotalValue;
                    oRequest.CommodityUnit = CommodityUnit;
                    oRequest.SecDate = null;
                    oRequest.SecNumber = null;
                    oRequest.PerformNumber = null;
                    oRequest.PerformDate = null;
                    oRequest.CompanyName = CompanyName;
                    oRequest.CompanyNationalCode = CompanyNationalCode;
                    oRequest.RecordNumber = RecordNumber;
                    oRequest.RecordDate = RecordDate;
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.CellPhoneNumber = CellPhoneNumber;
                    oRequest.AmountPaid = AmountPaid;
                    oRequest.CurrencyCode = oCurrency.Code;
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.CurrencyValue = AmountPaid;
                    oRequest.BaseCurrencyValue = AmountPaid;
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = false;
                    oRequest.IsVerified = true;
                    oRequest.URLAddress = URLAddress;
                    oRequest.Tariffs = Tariffs;
                    oRequest.LicenseNumber = LicenseNumber;
                    oRequest.LicenseDate = LicenseDate;

                    oUnitOfWork.RequestRepository.Insert(oRequest);
                    oUnitOfWork.Save();
                    #endregion

                    #region Move Last Message
                    //if (oFindRequest.Messages.ToList().Count > 0)
                    //{
                    //    foreach (var row in oFindRequest.Messages.ToList())
                    //    {
                    //        row.RequestId = oRequest.Id;
                    //    }

                    //}
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
                    if (FileList != null)
                    {
                        foreach (var fileAddress in FileList)
                        {
                            newFile = new Models.File();
                            newFile.Id = Guid.NewGuid();
                            newFile.FileAddress = fileAddress;
                            newFile.InsertDateTime = DateTime.Now;
                            newFile.IsActived = true;
                            newFile.IsDeleted = false;
                            newFile.IsSystem = true;
                            newFile.IsVerified = true;
                            newFile.Name = newFile.Id.ToString();
                            newFile.RequestId = oRequest.Id;
                            newFile.UpdateDateTime = DateTime.Now;
                            oUnitOfWork.FileRepository.Insert(newFile);
                            oUnitOfWork.Save();
                        }
                    }
                    #endregion
                }

                else if (oFindRequest != null && oFindRequest.RequestState > (int)Enums.RequestStates.PaymentOrder)
                {
                    oRequest = oFindRequest;
                }

                message = oUnitOfWork.RequestRepository.GetById(oRequest.Id).InvoiceNumber.ToString();


                // برای حذف موارد تکراری
                List<Models.Request> oDeleteRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == oSubSystem.Id)
                    .Where(current => current.CompanyNationalCode == CompanyNationalCode.Trim())
                    .Where(curren => curren.RecordNumber == RecordNumber)
                    .Where(curren => curren.Id != oRequest.Id)
                    .Where(curren => curren.RequestState == 1)
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

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = Resources.Message.Global.UnknownError;
                return -4;
            }
        }

        [WebMethod]
        public int QuarantineRequestState(string UserName, string Password, int SubSystemCode, int InvoiceNumber, out string message)
        {
            int sssss = 0;
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

            #region Convert Code Qurantineh
            switch (SubSystemCode)
            {
                case 4:
                case 5:
                case 6:
                case 11:
                    sssss = 2003;
                    break;

                case 7:
                    sssss = 2005;
                    break;

                case 9:
                    sssss = 2001;
                    break;

                case 10:
                    sssss = 2004;
                    break;

                case 12:
                    sssss = 2002;
                    break;
            }
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
                    .Where(current => current.SubSystem.Code == sssss)
                    .FirstOrDefault()
                    ;

                //Not Found
                if (oRequest == null)
                {
                    //message = UserName + "!" + Password + "!" + InvoiceNumber + "!" + SubSystemCode + "!" + sssss;
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
                return -4;
            }
        }

    }
}
