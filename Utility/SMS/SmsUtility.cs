using System;
using System.ServiceModel;

namespace Utilities.SMS
{
    public static class SmsUtility
    {
        /// <summary>
        /// ارسال پیامک اطلاع‌رسانی بارگیری شدن بار به مشتری
        /// </summary>
        public static bool SendLoadedNotification(
            string buyerMobile,
            string factorNumber, // برای {0}
            string productName,  // برای {1} (ترکیب نام محصول، کارخانه، بسته‌بندی و تناژ)
            string driverName,   // برای {2}
            string driverMobile, // برای {3}
            string licensePlate  // برای {4}
            )
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";

                // مقادیر باید دقیقاً بر اساس شماره ایندکس‌ها (از 0 تا 4) در آرایه چیده شوند
                string[] texts = new string[]
                {
                    factorNumber,  // {0} - فاکتور
                    productName,   // {1} - محصول
                    driverName,    // {2} - نام راننده
                    driverMobile,  // {3} - شماره تماس راننده
                    licensePlate   // {4} - پلاک
                };

                var binding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport
                    }
                };

                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");

                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                // شناسه الگوی ثبت شده در پنل پیامکی
                int bodyId = 448729;

                soapClient.SendByBaseNumber(username, password, texts, buyerMobile, bodyId);

                return true;
            }
            catch (Exception ex)
            {
                // لاگ ارور
                Console.WriteLine($"خطا در ارسال پیامک بارگیری: {ex.Message}");
                return false;
            }
        }
    }
}
