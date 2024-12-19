namespace Infrastructure
{
    public static class NotificationService
    {
        private static class Email
        {
            static Email()
            {
            }

            private static string EmailsBodyPath
            {
                get
                {
                    return ("~/App_Data/LocalizedTemplates/Email");
                }
            }

            public static void SendNewPassword(Models.User user, string newPassword)
            {
                //System.Globalization.CultureInfo oCultureInfo =
                //    System.Threading.Thread.CurrentThread.CurrentCulture;

                //string strRootRelativePathName =
                //    string.Format("{0}/SendNewPassword.{1}.htm",
                //    EmailsBodyPath,
                //    oCultureInfo.Name);

                //string strPathName = System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

                //string strEmailBody = Utilities.IO.File.Read(strPathName);

                //strEmailBody =
                //    strEmailBody
                //    .Replace("[PASSWORD]", newPassword)
                //    .Replace("[EMAIL_ADDRESS]", user.EmailAddress)
                //    .Replace("[SITE_URL]", "d.OPS.ir")
                //    ;

                //System.Net.Mail.MailAddress oMailAddress =
                //    new System.Net.Mail.MailAddress
                //        (user.EmailAddress, user.EmailAddress, System.Text.Encoding.UTF8);


                //    Utilities.Net.MailMessage.Send
                //        (oMailAddress, "New Password!", strEmailBody,
                //        System.Net.Mail.MailPriority.High);

            }

            public static void NotifyUserAfterUpdatingProfile(Models.User user)
            {
                //System.Globalization.CultureInfo oCultureInfo =
                //    System.Threading.Thread.CurrentThread.CurrentCulture;

                //string strRootRelativePathName =
                //    string.Format("{0}/NotifyUserAfterUpdatingProfile.{1}.htm",
                //    EmailsBodyPath, oCultureInfo.Name);

                //string strPathName = System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

                //string strEmailBody =Utilities.IO.File.Read(strPathName);

                //System.Net.Mail.MailAddress oMailAddress =
                //    new System.Net.Mail.MailAddress
                //        (user.EmailAddress, user.FullName, System.Text.Encoding.UTF8);


                //Utilities.Net.MailMessage.Send
                //    (oMailAddress, "Notify User After Updating Profile!",
                //    strEmailBody, System.Net.Mail.MailPriority.High);
            }
        }

        static NotificationService()
        {
        }

        public static void NotifyUserAfterUpdatingProfile(Models.User user)
        {
            Email.NotifyUserAfterUpdatingProfile(user);
        }
    }
}
