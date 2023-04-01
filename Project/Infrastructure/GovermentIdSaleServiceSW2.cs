using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure
{
	public static class GovermentIdSaleServiceSW2
	{
		static GovermentIdSaleServiceSW2()
		{
			//ServiceURL = "https://sadad.shaparak.ir/MerchantUtility.asmx";
			//ReturnURL_Last = "http://localhost:42932/Payment/MerchantCommit";
			// ReturnURL_CBI = "http://localhost:42932/Payment/VerifyTransaction";


			ServiceURL = "https://pec.shaparak.ir/NewIPGServices/Sale/GovermentIdSaleServiceSW2.asmx";
			//ReturnURL_Last = "https://ops.ivo.ir/Payment/MerchantCommit";
			ReturnURL_Last = "http://localhost:8085/Payment/MerchantCommit";
			ReturnURL_CBI = "http://localhost:8085/Payment/VerifyTransaction";
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