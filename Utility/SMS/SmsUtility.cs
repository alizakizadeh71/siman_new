using System;
using System.ServiceModel;

namespace Utilities.SMS
{
    public static class SmsUtility
    {
        /// <summary>
        /// ارسال پیامک اطلاع‌رسانی بارگیری شدن بار به مشتری
        /// شامل: نام باربری و شماره تماس باربری
        /// </summary>
        public static bool SendLoadedNotification(
            string buyerMobile,
            string buyerName,
            string factorNumber,
            string productName,
            string price,
            string remainAmount,
            string remainAmountText,
            string carrierName,
            string carrierMobile)
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";

                string[] texts =
                {
                    buyerName,          // {0}
                    factorNumber,       // {1}
                    productName,        // {2}
                    price,              // {3}
                    remainAmount,       // {4}
                    remainAmountText,   // {5}
                    carrierName,        // {6}
                    carrierMobile       // {7}
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

                int bodyId = 477498;

                soapClient.SendByBaseNumber(
                    username,
                    password,
                    texts,
                    buyerMobile,
                    bodyId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // الگو: باربری {0} / فاکتور {1} / شرح {2} / نام خریدار {3} / شماره خریدار {4}
        public static bool SendCarrierNotification(
            string carrierMobile,
            string carrierName,
            string factorNumber,
            string productName,
            string buyerName,
            string buyerMobile)
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";

                string[] texts =
                {
                    carrierName,   // {0} نام باربری
                    factorNumber,  // {1} شماره فاکتور
                    productName,   // {2} شرح
                    buyerName,     // {3} نام خریدار
                    buyerMobile,   // {4} شماره خریدار
                };

                var binding = new BasicHttpBinding { Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport } };
                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                soapClient.SendByBaseNumber(username, password, texts, carrierMobile, 477495);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// ارسال پیامک برای کاربران مهمان
        /// (بدون مانده حساب)
        /// </summary>
        public static bool SendGuestLoadedNotification(
            string buyerMobile,
            string buyerName,
            string factorNumber,
            string productName,
            string price,
            string carrierName,
            string carrierMobile)
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";

                string[] texts =
                {
                    buyerName,      // {0}
                    factorNumber,   // {1}
                    productName,    // {2}
                    price,          // {3}
                    carrierName,    // {4}
                    carrierMobile   // {5}
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

                int bodyId = 477499; // شناسه الگوی پیامک کاربر مهمان

                soapClient.SendByBaseNumber(
                    username,
                    password,
                    texts,
                    buyerMobile,
                    bodyId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // الگو: باربری {0} / فاکتور {1} / شرح {2} / نام خریدار {3} / شماره خریدار {4} / نام راننده {5} / شماره راننده {6}
        public static bool SendCarrierNotificationWithDriver(
            string carrierMobile,
            string carrierName,
            string factorNumber,
            string productName,
            string buyerName,
            string buyerMobile,
            string driverName,
            string driverMobile)
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";

                string[] texts =
                {
                    carrierName,   // {0} نام باربری
                    factorNumber,  // {1} شماره فاکتور
                    productName,   // {2} شرح
                    buyerName,     // {3} نام خریدار
                    buyerMobile,   // {4} شماره خریدار
                    driverName,    // {5} نام راننده
                    driverMobile,  // {6} شماره راننده
                };

                var binding = new BasicHttpBinding { Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport } };
                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                soapClient.SendByBaseNumber(username, password, texts, carrierMobile, 477497);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// ارسال پیامک اطلاع‌رسانی شارژ/کسر کیف پول توسط ادمین
        /// شامل: نام کاربر، مبلغ تغییر، موجودی جدید، وضعیت حساب
        /// </summary>
        public static bool SendWalletChargeNotification(
            string buyerMobile,
            string buyerName,
            string chargeAmount,
            string newBalance,
            string remainAmountText)  // بدهکار / طلبکار / تسویه
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";
                string[] texts =
                {
                    buyerName,                                    // {0} نام کاربر
                    chargeAmount,                                 // {1} مبلغ شارژ یا کسر
                    $"{newBalance} ریال ({remainAmountText})"    // {2} موجودی + وضعیت
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
                int bodyId = 482326;
                soapClient.SendByBaseNumber(username, password, texts, buyerMobile, bodyId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}