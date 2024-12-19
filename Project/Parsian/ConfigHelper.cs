namespace OPS.Parsian
{
    public sealed class ConfigHelper
    {
        //public static string ReversalServiceUrl
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["ReversalServiceUrl"];
        //    }
        //}

        //public static string ReversalServiceSW2Url
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["ReversalServiceSW2Url"];
        //    }
        //}

        public static string LoginAccount
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["LoginAccount"];
            }
        }

        public static string ParsianPGWBillServiceUrl
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianPGWBillServiceUrl"];
            }
        }

        public static string ParsianPGWSaleServiceUrl
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianPGWSaleServiceUrl"];
            }
        }

        public static string ParsianPGWGovermentIdSaleServiceUrl
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianPGWGovermentIdSaleServiceUrl"];
            }
        }

        public static string ParsianPGWConfirmServiceUrl
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianPGWConfirmServiceUrl"];
            }
        }

        public static string ParsianPGWConfirmServiceSW2Url
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianPGWConfirmServiceSW2Url"];
            }
        }

        public static string ParsianIPGPageUrl
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianIPGPageUrl"];
            }
        }

        public static string ParsianIPGPageSW2Url
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ParsianIPGPageSW2Url"];
            }
        }

        public static string BillPaymentCallback
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["BillPaymentCallback"];
            }
        }

        public static string SalePaymentCallback
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["SalePaymentCallback"];
            }
        }
    }
}