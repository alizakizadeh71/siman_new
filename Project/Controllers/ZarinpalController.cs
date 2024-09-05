using OPS.ir.shaparak.sadad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using OPS.CBINasimService;
using Rotativa;
using OPS.Parsian;
using log4net;
using System.Text;
using System.Security.Cryptography;
using ViewModels.Areas.Administrator.ZarinPal;


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Models;

namespace OPS.Controllers
{
    public partial class ZarinpalController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();
        private Infrastructure.cbinasimService oCBINasim = new Infrastructure.cbinasimService();
        PGServiceClient oIPGServices = new PGServiceClient();
        //string testamount = "10017";

        public virtual ActionResult Payment(int invoiceNumber, string MahalTahvil)
        {
            try
            {
                var oFactorCement = UnitOfWork.FactorCementRepository.GetByinvoicenumber(invoiceNumber).FirstOrDefault();
                var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
                oFactorCement.MahalTahvil = MahalTahvil;
                var Tonnage = Convert.ToInt32(UnitOfWork.tonnageRepository.Get()
                .Where(x => x.Id == oFactorCement.TonnageId).FirstOrDefault().Code);
                var InventoryTonnage = UnitOfWork.InventoryamountRepository.Get()
                    .Where(x => x.IsDeleted == false && x.IsActived != false)
                    .Where(x => x.ProductNameId == oFactorCement.ProductNameId)
                    .Where(x => x.ProductTypeId == oFactorCement.ProductTypeId)
                    .Where(x => x.PackageTypeId == oFactorCement.PackageTypeId)
                    .Where(x => x.FactoryNameId == oFactorCement.FactoryNameId)
                    .FirstOrDefault();
                UnitOfWork.FactorCementRepository.Update(oFactorCement);

                string merchant = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";
                string amount = string.Empty;
                if (oFactorCement.MahalTahvil == "Karkhane")
                {
                    if (oFactorCement.AmountPaid > user.creditAmount && user.UserName != "Guest")
                    {
                        amount = Convert.ToString(oFactorCement.AmountPaid - user.creditAmount);
                        user.creditAmount = 0;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();
                    }
                    else if(user.UserName == "Guest")
                    {
                        amount = oFactorCement.AmountPaid.ToString();
                    }
                    else if (user.UserName != "Guest")
                    {
                        user.creditAmount = Convert.ToInt32(user.creditAmount - oFactorCement.AmountPaid);
                        UnitOfWork.UserRepository.Update(user);
                        oFactorCement.FinalApprove = true;
                        InventoryTonnage.Inventorytonnage = InventoryTonnage.Inventorytonnage - Tonnage;
                        UnitOfWork.InventoryamountRepository.Update(InventoryTonnage);
                        UnitOfWork.FactorCementRepository.Update(oFactorCement);
                        UnitOfWork.Save();
                        // Redirect به صفحه مقصد
                        return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = oFactorCement.InvoiceNumber });
                    }
                    //amount = oFactorCement.AmountPaid.ToString();
                }
                else if (oFactorCement.MahalTahvil == "Mahal")
                {
                    if (oFactorCement.AmountPaid > user.creditAmount || user.UserName != "Guest")
                    {
                        amount = Convert.ToString(oFactorCement.AmountPaid - user.creditAmount);
                        user.creditAmount = 0;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();
                    }
                    else if (user.UserName != "Guest")
                    {
                        user.creditAmount = Convert.ToInt32(user.creditAmount - oFactorCement.AmountPaid);
                        UnitOfWork.UserRepository.Update(user);
                        oFactorCement.FinalApprove = true;
                        UnitOfWork.FactorCementRepository.Update(oFactorCement);
                        UnitOfWork.Save();
                        // Redirect به صفحه مقصد
                        return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = oFactorCement.InvoiceNumber });
                    }
                }
                string authority;
                string description = oFactorCement.ProductName.Name + " - " + oFactorCement.PackageType.Name + " - " + oFactorCement.FactoryName.Name + " - " + oFactorCement.Tonnage.Name;
                string callbackurl = "https://masalehpakhsh.com/Zarinpal/VerifyPayment";
                string mobile = oFactorCement.BuyerMobile;
                if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                {
                    callbackurl = "http://localhost:6066/Zarinpal/VerifyPayment";
                }

                ViewModels.Areas.Administrator.ZarinPal.RequestParameters Parameters = new ViewModels.Areas.Administrator.ZarinPal.RequestParameters(merchant, amount, description, callbackurl, mobile, "ali_animax@yahoo.com");

                var client = new RestClient(URLs.requestUrl);

                Method method = Method.Post;

                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");

                request.AddJsonBody(Parameters);

                var requestresponse = client.ExecuteAsync(request);

                JObject jo = JObject.Parse(requestresponse.Result.Content); /// "authority": "A00000000000000000000000000433573885"

                string errorscode = jo["errors"].ToString();

                JObject jodata = JObject.Parse(requestresponse.Result.Content);

                string dataauth = jodata["data"].ToString();


                if (dataauth != "[]")
                {


                    authority = jodata["data"]["authority"].ToString();

                    oFactorCement.Authority = authority;
                    UnitOfWork.FactorCementRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    string gatewayUrl = URLs.gateWayUrl + authority;

                    return Redirect(gatewayUrl);

                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                //    throw new Exception(ex.Message);
            }
            return null;
        }

        public virtual ActionResult VerifyPayment()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            try
            {
                VerifyParameters parameters = new VerifyParameters();
                string authority = string.Empty;
                string Status = string.Empty;
                if (HttpContext.Request.QueryString["Authority"] != "")
                {
                    authority = HttpContext.Request.QueryString["Authority"];
                    Status = HttpContext.Request.QueryString["Status"];
                }
                var oFactorCement = UnitOfWork.FactorCementRepository.GetByAuthority(authority).FirstOrDefault();

                if (oFactorCement.FinalApprove) /// اگر دفعه دوم وارد این صفحه شد تایید نهایی بوده و وارد این بخش شود
                {
                    cementViewModel = ConvertCementViewModel(oFactorCement);
                    return View(cementViewModel);
                }

                parameters.authority = oFactorCement.Authority;
                if (oFactorCement.MahalTahvil == "Karkhane")
                {
                    parameters.amount = oFactorCement.AmountPaid.ToString();
                }
                else if (oFactorCement.MahalTahvil == "Mahal")
                {
                    parameters.amount = oFactorCement.DestinationAmountPaid.ToString();
                }
                parameters.merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";

                //if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                //{
                //    parameters.amount = testamount;
                //}

                var client = new RestClient(URLs.verifyUrl);
                Method method = Method.Post;
                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.ExecuteAsync(request);


                JObject jodata = JObject.Parse(response.Result.Content);

                string data = jodata["data"].ToString();

                JObject jo = JObject.Parse(response.Result.Content);

                string errors = jo["errors"].ToString();

                if (data != "[]")
                {
                    string refid = jodata["data"]["ref_id"].ToString();
                    string card_pan = jodata["data"]["card_pan"].ToString();
                    string code = jodata["data"]["code"].ToString();

                    oFactorCement.ref_id = Convert.ToInt64(refid);
                    oFactorCement.card_pan = card_pan;
                    oFactorCement.Bankcode = Convert.ToInt32(code);
                    oFactorCement.AmountPaidDate = DateTime.Now;
                    oFactorCement.Address = cementViewModel.Address;
                    UnitOfWork.FactorCementRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    ViewBag.code = refid;
                    if (Status == "OK") /// اگر با موفقییت پرداخت شده بود
                    {
                        oFactorCement.FinalApprove = true;
                        var InventoryTonnage = UnitOfWork.InventoryamountRepository.Get()
                        .Where(x => x.IsDeleted == false && x.IsActived != false)
                        .Where(x => x.ProductNameId == oFactorCement.ProductNameId)
                        .Where(x => x.ProductTypeId == oFactorCement.ProductTypeId)
                        .Where(x => x.PackageTypeId == oFactorCement.PackageTypeId)
                        .Where(x => x.FactoryNameId == oFactorCement.FactoryNameId)
                        .FirstOrDefault();

                        var Tonnage = Convert.ToInt32(UnitOfWork.tonnageRepository.Get()
                        .Where(x => x.Id == oFactorCement.TonnageId).FirstOrDefault().Code);

                        InventoryTonnage.Inventorytonnage = InventoryTonnage.Inventorytonnage - Tonnage;
                        UnitOfWork.InventoryamountRepository.Update(InventoryTonnage);
                        UnitOfWork.Save();

                        cementViewModel = ConvertCementViewModel(oFactorCement);
                    }
                }
                else if (errors != "[]")
                {
                    string errorscode = jo["errors"]["code"].ToString();
                    TempData["NotFoundMessage"] = "خطا در پرداخت، کد خطا: " + errorscode;
                    //return BadRequest($"error code {errorscode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cementViewModel);
        }

        public static ViewModels.Areas.Administrator.Cement.CementViewModel ConvertCementViewModel(Models.FactorCement oFactorCement)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            cementViewModel.Id = oFactorCement.Id;
            cementViewModel.InvoiceNumber = oFactorCement.InvoiceNumber;
            cementViewModel.StringInsertDateTime = new Infrastructure.Calander(oFactorCement.InsertDateTime).Persion();
            cementViewModel.BuyerMobile = oFactorCement.BuyerMobile;
            cementViewModel.BuyerName = oFactorCement.User.FullName;
            cementViewModel.BuyerNationalCode = oFactorCement.User.NationalCode;
            cementViewModel.StringProvince = oFactorCement.Province.Name;
            cementViewModel.StringCity = oFactorCement.City.Name;
            cementViewModel.StringProductName = oFactorCement.ProductName.Name;
            cementViewModel.StringProductType = oFactorCement.ProductType.Name;
            cementViewModel.StringPackageType = oFactorCement.PackageType.Name;
            cementViewModel.StringFactoryName = oFactorCement.FactoryName.Name;
            cementViewModel.StringTonnage = oFactorCement.Tonnage.Name;
            cementViewModel.AmountPaid = oFactorCement.AmountPaid;
            cementViewModel.DestinationAmountPaid = oFactorCement.DestinationAmountPaid != null ? oFactorCement.DestinationAmountPaid.Value : 0;
            cementViewModel.MahalTahvil = oFactorCement.MahalTahvil == "Karkhane" ? "درب کارخانه" : oFactorCement.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ";
            cementViewModel.ref_id = oFactorCement.ref_id.ToString();
            cementViewModel.card_pan = oFactorCement.card_pan;
            cementViewModel.Address = oFactorCement.Address;
            cementViewModel.RemittanceNumber = oFactorCement.RemittanceNumber;
            return cementViewModel;
        }
        public virtual ActionResult Paymentwallet(int Chargeamount, int invoiceNumber)
        {
            var oFactorCement = UnitOfWork.walletFactorRepository.GetByinvoicenumber(invoiceNumber).FirstOrDefault();
            var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
            string authority;
            string description = "شارژ کیف پول" + "-" + user.UserName;
            string callbackurl = "https://masalehpakhsh.com/Zarinpal/VerifyPaymentWallet";
            string merchant = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";
            string mobile = oFactorCement.BuyerMobile;
            if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
            {
                callbackurl = "http://localhost:6066/Zarinpal/VerifyPaymentWallet";
            }

            ViewModels.Areas.Administrator.ZarinPal.RequestParameters Parameters = new ViewModels.Areas.Administrator.ZarinPal.RequestParameters(merchant, Chargeamount.ToString(), description, callbackurl, user.BuyerMobile, "ali_animax@yahoo.com");

            var client = new RestClient(URLs.requestUrl);

            Method method = Method.Post;

            var request = new RestRequest("", method);

            request.AddHeader("accept", "application/json");

            request.AddHeader("content-type", "application/json");

            request.AddJsonBody(Parameters);

            var requestresponse = client.ExecuteAsync(request);

            JObject jo = JObject.Parse(requestresponse.Result.Content); /// "authority": "A00000000000000000000000000433573885"

            string errorscode = jo["errors"].ToString();

            JObject jodata = JObject.Parse(requestresponse.Result.Content);

            string dataauth = jodata["data"].ToString();


            if (dataauth != "[]")
            {


                authority = jodata["data"]["authority"].ToString();

                oFactorCement.Authority = authority;
                UnitOfWork.walletFactorRepository.Update(oFactorCement);
                UnitOfWork.Save();

                string gatewayUrl = URLs.gateWayUrl + authority;

                return Redirect(gatewayUrl);

            }
            else
            {
                return null;
            }

        }

        public virtual ActionResult VerifyPaymentWallet()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            try
            {
                VerifyParameters parameters = new VerifyParameters();
                string authority = string.Empty;
                string Status = string.Empty;
                if (HttpContext.Request.QueryString["Authority"] != "")
                {
                    authority = HttpContext.Request.QueryString["Authority"];
                    Status = HttpContext.Request.QueryString["Status"];
                }
                var oFactorCement = UnitOfWork.walletFactorRepository.GetByAuthority(authority).FirstOrDefault();

                if (oFactorCement.FinalApprove) /// اگر دفعه دوم وارد این صفحه شد تایید نهایی بوده و وارد این بخش شود
                {
                    cementViewModel = ConvertCementViewModel(oFactorCement);
                    return View(cementViewModel);
                }

                parameters.authority = oFactorCement.Authority;
                parameters.merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";

                if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                {
                    parameters.amount = oFactorCement.Chargeamount.ToString();
                }
                else
                {
                    parameters.amount = oFactorCement.Chargeamount.ToString();
                }

                var client = new RestClient(URLs.verifyUrl);
                Method method = Method.Post;
                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.ExecuteAsync(request);


                JObject jodata = JObject.Parse(response.Result.Content);

                string data = jodata["data"].ToString();

                JObject jo = JObject.Parse(response.Result.Content);

                string errors = jo["errors"].ToString();

                if (data != "[]")
                {
                    string refid = jodata["data"]["ref_id"].ToString();
                    string card_pan = jodata["data"]["card_pan"].ToString();
                    string code = jodata["data"]["code"].ToString();

                    oFactorCement.ref_id = Convert.ToInt64(refid);
                    oFactorCement.card_pan = card_pan;
                    oFactorCement.Bankcode = Convert.ToInt32(code);
                    oFactorCement.AmountPaidDate = DateTime.Now;
                    UnitOfWork.walletFactorRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    ViewBag.code = refid;
                    if (Status == "OK") /// اگر با موفقییت پرداخت شده بود
                    {
                        oFactorCement.FinalApprove = true;
                        UnitOfWork.walletFactorRepository.Update(oFactorCement);
                        var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
                        user.creditAmount  += oFactorCement.Chargeamount;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();

                        cementViewModel = ConvertCementViewModel(oFactorCement);
                    }
                }
                else if (errors != "[]")
                {
                    string errorscode = jo["errors"]["code"].ToString();
                    TempData["NotFoundMessage"] = "خطا در پرداخت، کد خطا: " + errorscode;
                    //return BadRequest($"error code {errorscode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cementViewModel);
        }

        public static ViewModels.Areas.Administrator.Cement.CementViewModel ConvertCementViewModel(Models.walletFactor oFactorCement)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            cementViewModel.Id = oFactorCement.Id;
            cementViewModel.BuyerMobile = oFactorCement.BuyerMobile;
            cementViewModel.AmountPaid = oFactorCement.Chargeamount;
            cementViewModel.ref_id = oFactorCement.ref_id.ToString();
            cementViewModel.InvoiceNumber = oFactorCement.InvoiceNumber;
            cementViewModel.BuyerName = oFactorCement.User.UserName;
            cementViewModel.PayEndDate = oFactorCement.AmountPaidDate;
            cementViewModel.card_pan = oFactorCement.card_pan;
            cementViewModel.BuyerNationalCode = oFactorCement.User.NationalCode;
            return cementViewModel;
        }
    }
}