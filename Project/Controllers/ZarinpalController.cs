﻿using OPS.ir.shaparak.sadad;
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

namespace OPS.Controllers
{
    public partial class ZarinpalController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();
        private Infrastructure.cbinasimService oCBINasim = new Infrastructure.cbinasimService();
        PGServiceClient oIPGServices = new PGServiceClient();
        string testamount = "10017";

        public virtual ActionResult Payment(int invoiceNumber)
        {
            try
            {
                var oFactorCement = UnitOfWork.FactorCementRepository.GetByinvoicenumber(invoiceNumber).FirstOrDefault();

                string merchant = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";
                string amount = oFactorCement.AmountPaid.ToString();
                string authority;
                string description = oFactorCement.ProductName.Name + " - " + oFactorCement.PackageType.Name + " - " + oFactorCement.FactoryName.Name + " - " + oFactorCement.Tonnage.Name;
                string callbackurl = "https://masalehpakhsh.com/Zarinpal/VerifyPayment";
                string mobile = oFactorCement.BuyerMobile;
                if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                {
                    callbackurl = "http://localhost:6066/Zarinpal/VerifyPayment";
                    mobile = "09130445980";
                    amount = testamount;
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
                parameters.amount = oFactorCement.AmountPaid.ToString();
                parameters.merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";

                if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                {
                    parameters.amount = testamount;
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
                    UnitOfWork.FactorCementRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    ViewBag.code = refid;
                    if (Status == "OK") /// اگر با موفقییت پرداخت شده بود
                    {
                        oFactorCement.FinalApprove = true;
                        UnitOfWork.FactorCementRepository.Update(oFactorCement);
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

        private static ViewModels.Areas.Administrator.Cement.CementViewModel ConvertCementViewModel(Models.FactorCement oFactorCement)
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
            cementViewModel.ref_id = oFactorCement.ref_id.ToString();
            cementViewModel.card_pan = oFactorCement.card_pan;
            return cementViewModel;
        }
    }
}