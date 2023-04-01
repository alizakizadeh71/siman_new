namespace Utilities.Net
{
    public static class MailMessage
    {
        public static string ConvertTextForEmailBody(string text)
        {
            if (text == null)
            {
                return (string.Empty);
            }

            text =
                text
                .Replace(System.Convert.ToChar(13).ToString(), "<br />") // Return Key.
                .Replace(System.Convert.ToChar(10).ToString(), string.Empty) // Return Key.
                .Replace(System.Convert.ToChar(9).ToString(), "&nbsp;&nbsp;&nbsp;&nbsp;"); // TAB Key.

            return (text);
        }


        public static void Send(string subject, string body)
        {
            Send(null, null, subject, body, System.Net.Mail.MailPriority.High, null, System.Net.Mail.DeliveryNotificationOptions.Never);
        }


        public static void Send(IMailSettings mailSettings, string subject, string body)
        {
            Send(mailSettings, null, null, subject, body, System.Net.Mail.MailPriority.High, null, System.Net.Mail.DeliveryNotificationOptions.Never);
        }


        public static void Send(System.Net.Mail.MailAddress recipient, string subject, string body, System.Net.Mail.MailPriority priority)
        {
            System.Net.Mail.MailAddressCollection oRecipients = new System.Net.Mail.MailAddressCollection();
            oRecipients.Add(recipient);
            Send(null, oRecipients, subject, body, priority, null, System.Net.Mail.DeliveryNotificationOptions.Never);
        }


        public static void Send(IMailSettings mailSettings, System.Net.Mail.MailAddress recipient, string subject, string body, System.Net.Mail.MailPriority priority)
        {
            System.Net.Mail.MailAddressCollection oRecipients = new System.Net.Mail.MailAddressCollection();
            oRecipients.Add(recipient);
            Send(mailSettings, null, oRecipients, subject, body, priority, null, System.Net.Mail.DeliveryNotificationOptions.Never);
        }


        public static void Send(
                System.Net.Mail.MailAddress sender,
                System.Net.Mail.MailAddressCollection recipients,
                string subject,
                string body,
                System.Net.Mail.MailPriority priority,
                System.Collections.Generic.List<string> attachmentPathNames,
                System.Net.Mail.DeliveryNotificationOptions deliveryNotification)
        {
            System.Net.Mail.MailAddress oSender = null;
            System.Net.Mail.SmtpClient oSmtpClient = null;
            System.Net.Mail.MailMessage oMailMessage = null;

            try
            {
                #region Mail Message Configuration
                oMailMessage = new System.Net.Mail.MailMessage();

                oMailMessage.To.Clear();
                oMailMessage.CC.Clear();
                oMailMessage.Bcc.Clear();
                oMailMessage.ReplyToList.Clear();
                oMailMessage.Attachments.Clear();

                if (sender != null)
                {
                    oSender = sender;
                }
                else
                {
                    string strDisplayName = Setting.ApplicationSettings.GetValue("SenderDisplayName");
                    string strEmailAddress = Setting.ApplicationSettings.GetValue("SenderEmailAddress");

                    if (string.IsNullOrEmpty(strDisplayName))
                    {
                        oSender = new System.Net.Mail.MailAddress(strEmailAddress, strEmailAddress, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        oSender = new System.Net.Mail.MailAddress(strEmailAddress, strDisplayName, System.Text.Encoding.UTF8);
                    }
                }

                oMailMessage.From = oSender;
                oMailMessage.Sender = oSender;

                oMailMessage.ReplyToList.Add(oSender);

                if (recipients == null)
                {
                    System.Net.Mail.MailAddress oMailAddress = null;

                    string strDisplayName = Setting.ApplicationSettings.GetValue("SupportDisplayName");

                    string strEmailAddress = Setting.ApplicationSettings.GetValue("SupportEmailAddress");

                    if (string.IsNullOrEmpty(strDisplayName))
                    {
                        oMailAddress = new System.Net.Mail.MailAddress(strEmailAddress, strEmailAddress, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        oMailAddress = new System.Net.Mail.MailAddress(strEmailAddress, strDisplayName, System.Text.Encoding.UTF8);
                    }

                    oMailMessage.To.Add(oMailAddress);
                }
                else
                {
                    foreach (System.Net.Mail.MailAddress oMailAddress in recipients)
                    {
                        oMailMessage.To.Add(oMailAddress);
                    }
                }

                string strBccAddresses = Setting.ApplicationSettings.GetValue("BccAddresses");

                if (string.IsNullOrEmpty(strBccAddresses) == false)
                {
                    oMailMessage.Bcc.Add(strBccAddresses);
                }

                string strEmailSubjectTemplate = Setting.ApplicationSettings.GetValue("EmailSubjectTemplate");

                if (string.IsNullOrEmpty(strEmailSubjectTemplate))
                {
                    oMailMessage.Subject = subject;
                }
                else
                {
                    oMailMessage.Subject = string.Format(strEmailSubjectTemplate, subject);
                }

                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                oMailMessage.Body = body;
                oMailMessage.IsBodyHtml = true;
                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                oMailMessage.Priority = priority;
                oMailMessage.DeliveryNotificationOptions = deliveryNotification;


                if ((attachmentPathNames != null) && (attachmentPathNames.Count > 0))
                {
                    foreach (string strAttachmentPathName in attachmentPathNames)
                    {
                        if (System.IO.File.Exists(strAttachmentPathName))
                        {
                            System.Net.Mail.Attachment oAttachment =
                                new System.Net.Mail.Attachment(strAttachmentPathName);

                            oMailMessage.Attachments.Add(oAttachment);
                        }
                    }
                }

                oMailMessage.Headers.Add("Mailer_Version", "4.4");
                oMailMessage.Headers.Add("Mailer_Date", "2015/04/15");
                oMailMessage.Headers.Add("Mailer_Author", "Mr. Hadi Salari");
                oMailMessage.Headers.Add("Mailer_Company", "www.pap-ict.ir");
                #endregion


                #region Smtp Client Configuration

                oSmtpClient = new System.Net.Mail.SmtpClient();

                int intSmtpClientEnableSsl = 0;

                try
                {
                    intSmtpClientEnableSsl =System.Convert.ToInt32(Setting.ApplicationSettings.GetValue("SmtpClientEnableSsl", intSmtpClientEnableSsl.ToString()));
                }
                catch
                {
                }

                if (intSmtpClientEnableSsl == 1)
                {
                    oSmtpClient.EnableSsl = true;
                }
                else
                {
                    oSmtpClient.EnableSsl = false;
                }

                int intSmtpClientTimeout = 100000;

                try
                {
                    intSmtpClientTimeout =System.Convert.ToInt32(Setting.ApplicationSettings.GetValue("SmtpClientTimeout", intSmtpClientTimeout.ToString()));
                }
                catch
                {
                }

                oSmtpClient.Timeout = intSmtpClientTimeout;


                oSmtpClient.DeliveryMethod =System.Net.Mail.SmtpDeliveryMethod.Network;

                oSmtpClient.Host =Setting.ApplicationSettings.GetValue("SmtpClientHostAddress");

                int intSmtpClientPortNumber = 25;

                try
                {
                    intSmtpClientPortNumber =System.Convert.ToInt32(Setting.ApplicationSettings.GetValue("SmtpClientPortNumber", intSmtpClientPortNumber.ToString()));
                }
                catch
                {
                }

                oSmtpClient.Port = intSmtpClientPortNumber;

                oSmtpClient.UseDefaultCredentials = false;

                string strSenderEmailAddress = Setting.ApplicationSettings.GetValue("SenderEmailAddress");

                string strSenderEmailPassword =Setting.ApplicationSettings.GetValue("SenderEmailPassword");

                System.Net.NetworkCredential oNetworkCredential =new System.Net.NetworkCredential(strSenderEmailAddress, strSenderEmailPassword);

                oSmtpClient.Credentials = oNetworkCredential;
                #endregion

                oSmtpClient.Send(oMailMessage);
            }
            
            catch (System.Exception ex)
            {
                System.Collections.Hashtable oHashtable =
                    new System.Collections.Hashtable();

                if (oSender != null)
                {
                    oHashtable.Add("Sender Email", oSender.Address);
                    oHashtable.Add("Sender Display Name", oSender.DisplayName);
                }

                oHashtable.Add("Subject", subject);
                oHashtable.Add("Body", body);

                LogHandler.Report(typeof(MailMessage), oHashtable, ex, Enums.LogTypes.LogToFile);
                string strErrorMessage =Resources.Utility.Net.MailMessage.ErrorOnSendingEmail;
                throw (new Setting.ApplicationException(strErrorMessage));
            }

            finally
            {
                if (oMailMessage != null)
                {
                    oMailMessage.Dispose();
                    oMailMessage = null;
                }

                if (oSmtpClient != null)
                {
                    oSmtpClient.Dispose();
                    oSmtpClient = null;
                }
            }
        }

        public static void Send
            (   IMailSettings mailSettings,
                System.Net.Mail.MailAddress sender,
                System.Net.Mail.MailAddressCollection recipients,
                string subject,
                string body,
                System.Net.Mail.MailPriority priority,
                System.Collections.Generic.List<string> attachmentPathNames,
                System.Net.Mail.DeliveryNotificationOptions deliveryNotification)
        {
            if (mailSettings == null)
            {
                Send(sender, recipients, subject, body, priority, attachmentPathNames, deliveryNotification);
                return;
            }

            // **************************************************
            System.Net.Mail.MailAddress oSender = null;
            System.Net.Mail.SmtpClient oSmtpClient = null;
            System.Net.Mail.MailMessage oMailMessage = null;
            // **************************************************

            try
            {
                // **************************************************
                // *** Mail Message Configuration *******************
                // **************************************************
                oMailMessage = new System.Net.Mail.MailMessage();

                // **************************************************
                oMailMessage.To.Clear();
                oMailMessage.CC.Clear();
                oMailMessage.Bcc.Clear();
                oMailMessage.ReplyToList.Clear();
                oMailMessage.Attachments.Clear();
                // **************************************************

                // **************************************************
                if (sender != null)
                {
                    oSender = sender;
                }
                else
                {
                    string strDisplayName = mailSettings.SenderDisplayName;
                    string strEmailAddress = mailSettings.SenderEmailAddress;

                    if (string.IsNullOrEmpty(strDisplayName))
                    {
                        oSender =
                            new System.Net.Mail.MailAddress
                                (strEmailAddress, strEmailAddress, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        oSender =
                            new System.Net.Mail.MailAddress
                                (strEmailAddress, strDisplayName, System.Text.Encoding.UTF8);
                    }
                }

                oMailMessage.From = oSender;
                oMailMessage.Sender = oSender;

                // Note: Below Code Obsoleted in .NET 4.0
                //oMailMessage.ReplyTo = oSender;

                oMailMessage.ReplyToList.Add(oSender);
                // **************************************************

                if (recipients == null)
                {
                    System.Net.Mail.MailAddress oMailAddress = null;

                    string strDisplayName = mailSettings.SupportDisplayName;
                    string strEmailAddress = mailSettings.SupportEmailAddress;

                    if (string.IsNullOrEmpty(strDisplayName))
                    {
                        oMailAddress =
                            new System.Net.Mail.MailAddress
                                (strEmailAddress, strEmailAddress, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        oMailAddress =
                            new System.Net.Mail.MailAddress
                                (strEmailAddress, strDisplayName, System.Text.Encoding.UTF8);
                    }

                    oMailMessage.To.Add(oMailAddress);
                }
                else
                {
                    // Note: Wrong Usage!
                    // oMailMessage.To = recipients;

                    foreach (System.Net.Mail.MailAddress oMailAddress in recipients)
                    {
                        oMailMessage.To.Add(oMailAddress);
                    }
                }

                // **************************************************
                string strBccAddresses = mailSettings.BccAddresses;

                if (string.IsNullOrEmpty(strBccAddresses) == false)
                {
                    // Note: [BccAddresses] must be separated with comma character (",")
                    oMailMessage.Bcc.Add(strBccAddresses);
                }

                //if (oMailMessage.Bcc.Contains
                //	(new System.Net.Mail.MailAddress("Dtx@IranianExperts.ir")) == false)
                //{
                //	oMailMessage.Bcc.Add("Dtx@IranianExperts.ir");
                //}
                // **************************************************

                // **************************************************
                string strEmailSubjectTemplate = mailSettings.EmailSubjectTemplate;

                if (string.IsNullOrEmpty(strEmailSubjectTemplate))
                {
                    oMailMessage.Subject = subject;
                }
                else
                {
                    oMailMessage.Subject =
                        string.Format(strEmailSubjectTemplate, subject);
                }

                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                // **************************************************

                // **************************************************
                oMailMessage.Body = body;
                oMailMessage.IsBodyHtml = true;
                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                // **************************************************

                // **************************************************
                oMailMessage.Priority = priority;
                oMailMessage.DeliveryNotificationOptions = deliveryNotification;
                // **************************************************

                if ((attachmentPathNames != null) && (attachmentPathNames.Count > 0))
                {
                    foreach (string strAttachmentPathName in attachmentPathNames)
                    {
                        if (System.IO.File.Exists(strAttachmentPathName))
                        {
                            System.Net.Mail.Attachment oAttachment =
                                new System.Net.Mail.Attachment(strAttachmentPathName);

                            oMailMessage.Attachments.Add(oAttachment);
                        }
                    }
                }

                // **************************************************
                oMailMessage.Headers.Add("Mailer_Version", "4.4");
                oMailMessage.Headers.Add("Mailer_Date", "2015/04/15");
                oMailMessage.Headers.Add("Mailer_Author", "Mr. Hadi Salari");
                oMailMessage.Headers.Add("Mailer_Company", "www.pap-ict.ir");
                // **************************************************
                // *** /Mail Message Configuration ******************
                // **************************************************

                // **************************************************
                // *** Smtp Client Configuration ********************
                // **************************************************
                oSmtpClient = new System.Net.Mail.SmtpClient();

                // **************************************************
                oSmtpClient.Port = mailSettings.SmtpClientPortNumber;
                oSmtpClient.Timeout = mailSettings.SmtpClientTimeout;
                oSmtpClient.EnableSsl = mailSettings.SmtpClientEnableSsl;
                // **************************************************

                oSmtpClient.DeliveryMethod =
                    System.Net.Mail.SmtpDeliveryMethod.Network;

                oSmtpClient.Host = mailSettings.SmtpClientHostAddress;

                // **************************************************
                // **************************************************

                // **************************************************
                oSmtpClient.UseDefaultCredentials = false;

                string strSenderEmailAddress = mailSettings.SenderEmailAddress;
                string strSenderEmailPassword = mailSettings.SenderEmailPassword;

                System.Net.NetworkCredential oNetworkCredential =
                    new System.Net.NetworkCredential
                        (strSenderEmailAddress, strSenderEmailPassword);

                oSmtpClient.Credentials = oNetworkCredential;
                // **************************************************
                // *** /Smtp Client Configuration *******************
                // **************************************************

                oSmtpClient.Send(oMailMessage);
            }
            catch (System.Exception ex)
            {
                System.Collections.Hashtable oHashtable =
                    new System.Collections.Hashtable();

                if (oSender != null)
                {
                    oHashtable.Add("Sender Email", oSender.Address);
                    oHashtable.Add("Sender Display Name", oSender.DisplayName);
                }

                // **************************************************
                oHashtable.Add("Subject", subject);
                oHashtable.Add("Body", body);
                // **************************************************

                LogHandler.Report(typeof(Net.MailMessage), oHashtable, ex, Enums.LogTypes.LogToFile);
                string strErrorMessage =Resources.Utility.Net.MailMessage.ErrorOnSendingEmail;
                throw (new Setting.ApplicationException(strErrorMessage));
            }

            finally
            {
                if (oMailMessage != null)
                {
                    oMailMessage.Dispose();
                    oMailMessage = null;
                }

                if (oSmtpClient != null)
                {
                    oSmtpClient.Dispose();
                    oSmtpClient = null;
                }
            }
        }
    }
}
