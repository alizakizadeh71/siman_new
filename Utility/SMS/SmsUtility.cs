using System;
using System.ServiceModel;

namespace Utilities.SMS
{
    public static class SmsUtility
    {
        /// <summary>
        /// ارسال پیامک اطلاع‌رسانی بارگیری شدن بار به مشتری
        /// </summary>
        public static bool SendLoadedNotification(string buyerMobile, string driverName, string driverMobile, string licensePlate)
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";

                // مقادیری که جایگزین متغیرهای {0} و {1} و {2} در الگوی پنل پیامک می‌شوند
                string[] texts = { driverName, driverMobile, licensePlate };

                var binding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport
                    }
                };

                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");

                // دقت کنید که فضای نام MelipayamakService باید با پروژه شما همخوانی داشته باشد
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                // شناسه الگوی ثبت شده در پنل پیامکی (این عدد را باید از پنل ملی پیامک بگیرید)
                int bodyId = 448147; // <--- این عدد را با کد الگوی جدید جایگزین کنید

                soapClient.SendByBaseNumber(username, password, texts, buyerMobile, bodyId);

                return true;
            }
            catch (Exception ex)
            {
                // در اینجا می‌توانید لاگ ارور را ذخیره کنید
                Console.WriteLine($"خطا در ارسال پیامک بارگیری: {ex.Message}");
                return false;
            }
        }
    }
}