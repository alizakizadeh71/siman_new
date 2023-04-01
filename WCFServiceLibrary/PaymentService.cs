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

namespace WCFServiceLibrary
{
    public class PaymentService : IPaymentService
    {
        DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

        /// <summary>
        /// وب سرویس پرداخت هزینه کارشناسی
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CompanyNationalCode"></param>
        /// <param name="ProvinceCode"></param>
        /// <param name="CommodityType"></param>
        /// <param name="TotalValue"></param>
        /// <param name="CommodityUnit"></param>
        /// <param name="RecordNumber"></param>
        /// <param name="RecordDate"></param>
        /// <param name="SecNumber"></param>
        /// <param name="SecDate"></param>
        /// <param name="CellPhoneNumber"></param>
        /// <param name="AmountPaid"></param>
        /// <param name="PerformNumber"></param>
        /// <param name="PerformDate"></param>
        /// <param name="FileList"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int BachelorsFeePayment
            (string UserName, string Password, InvoiceInfo info, List<string> FileList
            , out int messageCode, out string message)
        {
            string InputData = string.Empty;
            try
            {
                var province = oUnitOfWork.ProvinceRepository.GetByCode("97");

                var currency = oUnitOfWork.CurrencyUnitRepository.GetByCode((int)Enums.CurrencyUnits.Rails);

                var subsystem = oUnitOfWork.SubSystemRepository.Get()
                    .ToList()
                    .Where(current => current.Code == (int)Enums.SubSystems.Drug_Clearance)
                    .FirstOrDefault();

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .FirstOrDefault();

                InputData
                    = "  *****  InputData *****"
                    + "||SubSystem:InsertImport "
                    + "||UserName: " + UserName
                    + "||CompanyName: " + info.CompanyName
                    + "||SubSystem: " + subsystem.Id
                    + "||CompanyNationalCode: " + info.CompanyNationalCode
                    + "||RecordNumber: " + info.RecordNumber;

                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (string.IsNullOrEmpty(info.CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (string.IsNullOrEmpty(info.CompanyNationalCode) || info.CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (string.IsNullOrEmpty(info.RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (string.IsNullOrEmpty(info.RecordDate) || info.RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (string.IsNullOrEmpty(info.CellPhoneNumber) || info.CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (currency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }

                if (info.InvoiceGoods==null || info.InvoiceGoods.Count==0 
                    || info.InvoiceGoods.Sum(x=>x.Amount)<= 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }
                #endregion

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

                var lastRequest
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .Where(current => current.CompanyNationalCode == info.CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == info.RecordNumber)
                    .Where(current => current.RecordDate == info.RecordDate)
                    .FirstOrDefault()
                    ;

                if (lastRequest != null)
                {
                    message = Enums.RequestStates.Duplicate.ToString();
                    messageCode = (int)Enums.RequestStates.Duplicate;
                    return lastRequest.InvoiceNumber;
                }


                #region Insert New Record
                Models.Request request = new Models.Request()
                {
                    UserId = oUser.Id,
                    SubSystemId = subsystem.Id,
                    ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId,
                    ServiceTariff = oServiceTariffInSubSystem.ServiceTariff,
                    ProvinceId = province.Id,
                    CommodityType = info.InvoiceGoods.FirstOrDefault().CommodityType,
                    TotalValue = info.InvoiceGoods.FirstOrDefault().TotalValue,
                    CommodityUnit = info.InvoiceGoods.FirstOrDefault().CommodityUnit,
                    SecDate = info.SecDate,
                    SecNumber = info.SecNumber,
                    PerformNumber = info.PerformNumber,
                    PerformDate = info.PerformDate,
                    CompanyName = info.CompanyName,
                    CompanyNationalCode = info.CompanyNationalCode,
                    RecordNumber = info.RecordNumber,
                    RecordDate = info.RecordDate,
                    InvoiceDate = DateTime.Now,
                    CellPhoneNumber = info.CellPhoneNumber,
                    AmountPaid = (long)info.InvoiceGoods.Sum(y=>y.Amount),
                    CurrencyCode = currency.Code,
                    CurrencyRation = currency.Ratio,
                    CurrencyValue = (long)info.InvoiceGoods.Sum(y => y.Amount),
                    BaseCurrencyValue = (long)info.InvoiceGoods.Sum(y => y.BaseAmount),
                    RequestState = (int)Enums.RequestStates.PaymentOrder,
                    IsActived = true,
                    IsDeleted = false,
                    IsSystem = false,
                    IsVerified = true,
                    Tariffs = 0,
                    LicenseNumber = string.Empty,
                    LicenseDate = DateTime.Now,
                };
                oUnitOfWork.RequestRepository.Insert(request);
                oUnitOfWork.Save();
                #endregion

                #region Insert New Message
                Models.Message oMessage = new Models.Message();
                oMessage.UserId = oUser.Id;
                oMessage.LastState = request.RequestState;
                oMessage.MessageText = Resources.Message.Request.Message_Update;
                oMessage.NewState = request.RequestState;
                oMessage.RequestId = request.Id;
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
                        newFile.FileAddress = fileAddress.Split('&')[1].ToString();
                        newFile.InsertDateTime = DateTime.Now;
                        newFile.IsActived = true;
                        newFile.IsDeleted = false;
                        newFile.IsSystem = true;
                        newFile.IsVerified = true;
                        newFile.Name = fileAddress.Split('&')[0].ToString();
                        newFile.RequestId = request.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                }
                #endregion

                var getRequest = oUnitOfWork.RequestRepository.GetById(request.Id);

                message = Enums.RequestStates.InitialRequet.ToString();
                messageCode = (int)Enums.RequestStates.InitialRequet;
                return getRequest.InvoiceNumber;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                messageCode = (int)Enums.RequestStates.Error;
                return -1;
            }
        }

        /// <summary>
        /// پرداخت هزینه فوب
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CompanyNationalCode"></param>
        /// <param name="ProvinceCode"></param>
        /// <param name="CommodityType"></param>
        /// <param name="TotalValue"></param>
        /// <param name="CommodityUnit"></param>
        /// <param name="RecordNumber"></param>
        /// <param name="RecordDate"></param>
        /// <param name="SecNumber"></param>
        /// <param name="SecDate"></param>
        /// <param name="CellPhoneNumber"></param>
        /// <param name="CurrencyCode"></param>
        /// <param name="CurrencyValue"></param>
        /// <param name="PerformNumber"></param>
        /// <param name="PerformDate"></param>
        /// <param name="BaseCurrencyValue"></param>
        /// <param name="FileList"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int FobPayment
            (string UserName, string Password, InvoiceInfo info, List<string> FileList
            , out int messageCode, out string message)
        {
            string InputData = string.Empty;
            try
            {
                var province = oUnitOfWork.ProvinceRepository.GetByCode("97");

                var currency = oUnitOfWork.CurrencyUnitRepository.GetByCode(info.Currency);

                var subsystem = oUnitOfWork.SubSystemRepository.Get()
                    .ToList()
                    .Where(current => current.Code == (int)Enums.SubSystems.Drug_Clearance)
                    .FirstOrDefault();

                var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .FirstOrDefault();

                InputData
                    = "  *****  InputData *****"
                    + "||SubSystem:InsertImport "
                    + "||UserName: " + UserName
                    + "||CompanyName: " + info.CompanyName
                    + "||SubSystem: " + subsystem.Id
                    + "||CompanyNationalCode: " + info.CompanyNationalCode
                    + "||RecordNumber: " + info.RecordNumber;

                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(info.CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(info.CompanyNationalCode) || info.CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(info.RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(info.RecordDate) || info.RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (string.IsNullOrEmpty(info.CellPhoneNumber) || info.CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (currency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }

                if (info.InvoiceGoods == null || info.InvoiceGoods.Count == 0
                    || info.InvoiceGoods.Sum(x => x.Amount) <= 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    messageCode = (int)Enums.RequestStates.Error;
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    return -1;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

                Models.Request request
                    = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .Where(current => current.CompanyNationalCode == info.CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == info.RecordNumber)
                    .Where(current => current.RecordDate == info.RecordDate)
                    .FirstOrDefault()
                    ;

                if (request != null && request.RequestState <= (int)Enums.RequestStates.InitialRequet)
                {
                    request.BaseCurrencyValue = info.InvoiceGoods.Sum(y=>y.BaseAmount);
                    request.CurrencyCode = info.Currency;
                    request.CurrencyRation = currency.Ratio;
                    request.CurrencyValue = info.InvoiceGoods.Sum(y => y.Amount);

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.RequestId = request.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion

                    message = "بروزرسانی درخواست";
                    messageCode = (int)Enums.RequestStates.InitialRequet;
                    return request.InvoiceNumber;
                }

                else if (request != null && request.RequestState > (int)Enums.RequestStates.InitialRequet)
                {
                    message = Enums.RequestStates.Duplicate.ToString();
                    messageCode = (int)Enums.RequestStates.Duplicate;
                    return request.InvoiceNumber;
                }

                else if (request == null)
                {
                    #region Insert New Record
                    request = new Models.Request();
                    request.UserId = oUser.Id;
                    request.SubSystemId = subsystem.Id;
                    request.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                    request.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                    request.ProvinceId = province.Id;
                    request.CommodityType = info.InvoiceGoods.FirstOrDefault().CommodityType;
                    request.TotalValue = info.InvoiceGoods.Sum(x=>x.TotalValue);
                    request.CommodityUnit = info.InvoiceGoods.FirstOrDefault().CommodityUnit;
                    request.SecDate = info.SecDate;
                    request.SecNumber = info.SecNumber;
                    request.PerformNumber = info.PerformNumber;
                    request.PerformDate = info.PerformDate;
                    request.CompanyName = info.CompanyName;
                    request.CompanyNationalCode = info.CompanyNationalCode;
                    request.RecordNumber = info.RecordNumber;
                    request.RecordDate = info.RecordDate;
                    request.InvoiceDate = DateTime.Now;
                    request.CellPhoneNumber = info.CellPhoneNumber;
                    request.AmountPaid = (long)info.InvoiceGoods.Sum(x => x.Amount * currency.Ratio);
                    request.CurrencyCode = currency.Code;
                    request.CurrencyValue = info.InvoiceGoods.Sum(x => x.Amount);
                    request.BaseCurrencyValue = info.InvoiceGoods.Sum(x => x.BaseAmount);
                    request.CurrencyRation = currency.Ratio;
                    request.RequestState = (int)Enums.RequestStates.InitialRequet;
                    request.IsActived = true;
                    request.IsDeleted = false;
                    request.IsSystem = false;
                    request.IsVerified = true;
                    request.Tariffs = 0;
                    request.LicenseNumber = string.Empty;
                    request.LicenseDate = DateTime.Now;
                    oUnitOfWork.RequestRepository.Insert(request);
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = request.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = request.RequestState;
                    oMessage.RequestId = request.Id;
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
                        newFile.RequestId = request.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                    #endregion

                    var Inserted = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .Where(current => current.CompanyNationalCode == info.CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == info.RecordNumber)
                    .Where(current => current.RecordDate == info.RecordDate)
                    .FirstOrDefault()
                    ;

                    message = Enums.RequestStates.InitialRequet.ToString();
                    messageCode = (int)Enums.RequestStates.InitialRequet;
                    return Inserted.InvoiceNumber;

                }

                else
                {
                    message = Enums.RequestStates.Error.ToString();
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = ex.Message;
                messageCode = (int)Enums.RequestStates.Error;
                return -1;
            }
        }

        /// <summary>
        /// پرداخت هزینه تبصره 23
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CompanyNationalCode"></param>
        /// <param name="ProvinceCode"></param>
        /// <param name="CommodityType"></param>
        /// <param name="TotalValue"></param>
        /// <param name="CommodityUnit"></param>
        /// <param name="RecordNumber"></param>
        /// <param name="RecordDate"></param>
        /// <param name="ImportRecordNumber"></param>
        /// <param name="SecNumber"></param>
        /// <param name="SecDate"></param>
        /// <param name="CellPhoneNumber"></param>
        /// <param name="CurrencyCode"></param>
        /// <param name="CurrencyValue"></param>
        /// <param name="PerformNumber"></param>
        /// <param name="PerformDate"></param>
        /// <param name="BaseCurrencyValue"></param>
        /// <param name="Description"></param>
        /// <param name="FileList"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int NotePayment
           (string UserName, string Password, InvoiceInfo info, List<string> FileList, out int messageCode, out string message)
        {
            var province = oUnitOfWork.ProvinceRepository.GetByCode(info.Province);
            var currency = oUnitOfWork.CurrencyUnitRepository.GetByCode(info.Currency);

            var subsystem = oUnitOfWork.SubSystemRepository.Get()
                .Where(x=>x.Code == (int)Enums.SubSystems.Drug_Clearance23)
                .FirstOrDefault();

            var oServiceTariffInSubSystem = oUnitOfWork.ServiceTariffInSubSystemRepository.Get()
                .ToList()
                .Where(current => current.SubSystemId == subsystem.Id)
                .FirstOrDefault();

            string InputData
                = "  *****  InputData *****"
                + "||SubSystem:InsertClearance "
                + "||UserName: " + UserName
                + "||CompanyName: " + info.CompanyName
                + "||SubSystem: " + subsystem.Id
                + "||CompanyNationalCode: " + info.CompanyNationalCode
                + "||ImportRecordNumber: " + info.ImportRecordNumber
                + "||RecordNumber: " + info.RecordNumber;

            try
            {
                #region Input Data Verified
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (string.IsNullOrEmpty(info.CompanyName))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyName);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (string.IsNullOrEmpty(info.CompanyNationalCode) || info.CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CompanyNationalCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (string.IsNullOrEmpty(info.RecordNumber))
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (string.IsNullOrEmpty(info.RecordDate) || info.RecordDate.Trim().Length != 10)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.RecordDate);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (string.IsNullOrEmpty(info.CellPhoneNumber) || info.CompanyNationalCode.Trim().Length != 11)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CellPhoneNumber);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (currency == null)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.CurrencyCode);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }

                if (info.InvoiceGoods == null || info.InvoiceGoods.Count == 0
                    || info.InvoiceGoods.Sum(x => x.Amount) <= 0)
                {
                    message = string.Format(Resources.Message.Global.MissDataFormat, Resources.Model.Request.AmountPaid);
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }
                #endregion

                #region Login Failed
                message = Infrastructure.Utility.UserLoginByWebService(UserName, Password);

                if (!string.IsNullOrEmpty(message))
                {
                    Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                    messageCode = (int)Enums.RequestStates.Error;
                    return -3;
                }
                #endregion

                Models.User oUser = oUnitOfWork.UserRepository.GetByUserName(UserName);

                Models.Request request =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .Where(current => current.CompanyNationalCode == info.CompanyNationalCode)
                    .Where(curren => curren.SecNumber == info.SecNumber)
                    .Where(curren => curren.RecordNumber == info.RecordNumber)
                    .Where(current => current.RecordDate == info.RecordDate)
                    .OrderByDescending(current => current.InvoiceNumber)
                    .FirstOrDefault()
                    ;

                if (request != null && request.RequestState <= (int)Enums.RequestStates.InitialRequet)
                {
                    #region Update Record
                    var currencyValue = info.InvoiceGoods.Sum(y => y.Amount);
                    var currencyValueBase = info.InvoiceGoods.Sum(y => y.BaseAmount);

                    request.AmountPaid = (Convert.ToInt64(currencyValue * currency.Ratio));
                    request.CurrencyCode = currency.Code;
                    request.CurrencyValue = currencyValue;
                    request.BaseCurrencyValue = currencyValueBase;
                    request.CurrencyRation = currency.Ratio;
                    request.RequestState = (int)Enums.RequestStates.InitialRequet;
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = request.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.NewState = request.RequestState;
                    oMessage.RequestId = request.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    oUnitOfWork.Save();
                    #endregion

                    message = "بروزرسانی درخواست";
                    messageCode = (int)Enums.RequestStates.InitialRequet;
                    return request.InvoiceNumber;
                }

                else if (request != null && request.RequestState > (int)Enums.RequestStates.InitialRequet)
                {
                    message = Enums.RequestStates.Duplicate.ToString();
                    messageCode = (int)Enums.RequestStates.Duplicate;
                    return request.InvoiceNumber;
                }

                else if (request == null)
                {
                    var currencyValue = info.InvoiceGoods.Sum(y => y.Amount);
                    var currencyValueBase = info.InvoiceGoods.Sum(y => y.BaseAmount);

                    #region Insert New Record
                    request = new Models.Request();
                    request.UserId = oUser.Id;
                    request.SubSystemId = subsystem.Id;
                    request.ServiceTariffId = oServiceTariffInSubSystem.ServiceTariffId;
                    request.ServiceTariff = oServiceTariffInSubSystem.ServiceTariff;
                    request.ProvinceId = province.Id;
                    request.CommodityType = info.InvoiceGoods.FirstOrDefault().CommodityType;
                    request.TotalValue = info.InvoiceGoods.Sum(x => x.TotalValue);
                    request.CommodityUnit = info.InvoiceGoods.FirstOrDefault().CommodityUnit;
                    request.SecDate = info.SecDate;
                    request.SecNumber = info.SecNumber;
                    request.PerformNumber = info.PerformNumber;
                    request.PerformDate = info.PerformDate;
                    request.CompanyName = info.CompanyName;
                    request.CompanyNationalCode = info.CompanyNationalCode;
                    request.RecordNumber = info.RecordNumber;
                    request.RecordDate = info.RecordDate;
                    request.ImportRecordNumber = info.ImportRecordNumber;
                    request.InvoiceDate = DateTime.Now;
                    request.CellPhoneNumber = info.CellPhoneNumber;

                    request.AmountPaid = (Convert.ToInt64(currencyValue * currency.Ratio));

                    request.CurrencyCode = currency.Code;
                    request.CurrencyValue = currencyValue;
                    request.BaseCurrencyValue = currencyValueBase;
                    request.Description = info.Description;
                    request.CurrencyRation = currency.Ratio;
                    request.RequestState = (int)Enums.RequestStates.InitialRequet;
                    request.IsActived = true;
                    request.IsDeleted = false;
                    request.IsSystem = false;
                    request.IsVerified = true;
                    request.Tariffs = 0;
                    request.LicenseNumber = string.Empty;
                    request.LicenseDate = DateTime.Now;

                    oUnitOfWork.RequestRepository.Insert(request);
                    oUnitOfWork.Save();
                    #endregion

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oUser.Id;
                    oMessage.LastState = request.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = request.RequestState;
                    oMessage.RequestId = request.Id;
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
                        newFile.RequestId = request.Id;
                        newFile.UpdateDateTime = DateTime.Now;
                        oUnitOfWork.FileRepository.Insert(newFile);
                        oUnitOfWork.Save();
                    }
                    #endregion

                    var Inserted = oUnitOfWork.RequestRepository.Get()
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubSystemId == subsystem.Id)
                    .Where(current => current.CompanyNationalCode == info.CompanyNationalCode)
                    .Where(curren => curren.RecordNumber == info.RecordNumber)
                    .Where(current => current.RecordDate == info.RecordDate)
                    .FirstOrDefault()
                    ;

                    message = Enums.RequestStates.InitialRequet.ToString();
                    messageCode = (int)Enums.RequestStates.InitialRequet;
                    return Inserted.InvoiceNumber;
                }

                else
                {
                    message = Enums.RequestStates.Error.ToString();
                    messageCode = (int)Enums.RequestStates.Error;
                    return -1;
                }
            }

            catch (Exception ex)
            {
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                message = ex.Message;
                messageCode = (int)Enums.RequestStates.Error;
                return -1;
            }
        }

        /// <summary>
        /// وب سرویس استعلام وضعیت
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="InvoiceNumber"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int InquiryRequest(string UserName, string Password, int InvoiceNumber, out int messageCode, out string message)
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
                messageCode = (int)Enums.RequestStates.Error;
                Infrastructure.Utility.InsertErrorLog(UserName, message, InputData, string.Empty);
                return -1;
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
                    messageCode = (int)Enums.RequestStates.Error;
                    message = string.Format(Resources.Message.Global.RecordNotFound, InvoiceNumber);
                    return -1;
                }

                else if (oRequest.RequestState == (int)Enums.RequestStates.PaymentConfirmation)
                {
                    messageCode = (int)Enums.RequestStates.PaymentConfirmation;
                    message = oRequest.Bank_BankReciptNumber;
                    return (int)Enums.RequestStates.PaymentConfirmation;
                }

                else
                {
                    messageCode = (int)oRequest.RequestState;
                    message = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, oRequest.RequestState);
                    return oRequest.RequestState;
                }
            }

            catch (Exception ex)
            {
                message = ex.Message;
                messageCode = (int)Enums.RequestStates.Error;
                string ErrorDetails = Utilities.Net.LogHandler.Report(GetType(), null, ex);
                Infrastructure.Utility.InsertErrorLog(UserName, ErrorDetails, InputData, string.Empty);
                return -1;
            }
        }

    }
}