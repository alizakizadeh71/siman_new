namespace Infrastructure
{
    public static class WebServiceSetting_Sadad
    {
        static WebServiceSetting_Sadad()
        {
            //ServiceURL = "https://sadad.shaparak.ir/MerchantUtility.asmx";
            //ReturnURL_Last = "http://localhost:42932/Payment/MerchantCommit";
            // ReturnURL_CBI = "http://localhost:42932/Payment/VerifyTransaction";


            ServiceURL = "https://sadad.shaparak.ir/MerchantUtility.asmx";
            ReturnURL_Last = "http://localhost:6014/Payment/MerchantCommit";
            //ReturnURL_Last = "https://ops.ivo.ir/Payment/MerchantCommit";
            //ReturnURL_CBI = "http://localhost:8085/Payment/VerifyTransaction";
            ReturnURL_CBI = "http://localhost:6014/Payment/VerifyTransaction";
        }


        public static string TranKey { get; set; }
        public static string MerchantId { get; set; }
        public static string Terminal { get; set; }
        public static string ServiceURL { get; set; }
        public static string ReturnURL_Last { get; set; }
        public static string ReturnURL_CBI { get; set; }
        public static string ImportReffererUrl { get; set; }
        public static string RequestKey { get; set; }
    }
}