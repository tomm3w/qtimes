using System;
using System.Drawing;
using System.Web.Configuration;

namespace Authentication.Helpers
{
    public class ConfigurationHelper
    {
        static ConfigurationHelper instance = null;
        public const string K_ENVIRONMENT_PRODUCTION = "Production";
        private double _voteAttachmentFileSize;
        public static readonly object padlock = new object();
        public string AppName { get; set; }
        public int UserTokenExpireInHours { get; set; }
        public Size UserProfileMaxResolution { get; set; }
        public Size UserProfileMinResolution { get; set; }
        public Size UserProfileThumbSize { get; set; }
        public int UserPasswordLength { get; set; }
        public double VoteAttachmentFileSize { get { return _voteAttachmentFileSize; } }
        public string ApplePushDevCertificate { get; set; }
        public string ApplePushLiveCertificate { get; set; }

        public string iPhoneAppURL { get; set; }
        public string iPadAppURL { get; set; }
        public string FacebookURL { get; set; }

        public string ProdAdminUserName { get; set; }
        public string ProdAdminPassword { get; set; }
        public string DevAdminUserName { get; set; }
        public string DevAdminPassword { get; set; }
        public string Environment { get; set; }

        public string MailChimpKey { get; set; }
        ConfigurationHelper()
        {
            refreshConfigs();
        }
        private void refreshConfigs()
        {
            #region AppSpecific
            AppName = WebConfigurationManager.AppSettings["AppName"];
            #endregion
            #region UserProfile
            int userTokenExpireInHours;
            if (int.TryParse(WebConfigurationManager.AppSettings["UserTokenExpireInHours"], out userTokenExpireInHours))
            {
                UserTokenExpireInHours = userTokenExpireInHours;
            }
            else
            {
                UserTokenExpireInHours = 2;
            }

            string[] temp = WebConfigurationManager.AppSettings["UserProfileMaxResolution"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string tempStr = string.Empty;
            int expectedMaxWidth = int.Parse(temp[0]);
            int expectedMaxHeight = int.Parse(temp[1]);
            UserProfileMaxResolution = new Size(expectedMaxWidth, expectedMaxHeight);

            temp = WebConfigurationManager.AppSettings["UserProfileMinResolution"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int expectedMinWidth = int.Parse(temp[0]);
            int expectedMinHeight = int.Parse(temp[1]);
            UserProfileMinResolution = new Size(expectedMinWidth, expectedMinHeight);

            temp = WebConfigurationManager.AppSettings["UserProfileThumbSize"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int expectedUserProfileThumbSizeMinWidth = int.Parse(temp[0]);
            int expectedUserProfileThumbSizeMinHeight = int.Parse(temp[1]);
            UserProfileThumbSize = new Size(expectedUserProfileThumbSizeMinWidth, expectedUserProfileThumbSizeMinHeight);

            int userpasswordlength;
            if (int.TryParse(WebConfigurationManager.AppSettings["UserPasswordLength"], out userpasswordlength))
            {
                UserPasswordLength = userpasswordlength;
            }
            else
            {
                UserPasswordLength = 6;
            }

            tempStr = WebConfigurationManager.AppSettings["VoteAttachmentFileSize"];
            double.TryParse(tempStr, out _voteAttachmentFileSize);

            #endregion
            #region apple push
            ApplePushDevCertificate = WebConfigurationManager.AppSettings["ApplePushDevCertificate"];
            ApplePushLiveCertificate = WebConfigurationManager.AppSettings["ApplePushLiveCertificate"];
            #endregion

            #region app
            iPhoneAppURL = WebConfigurationManager.AppSettings["iPhoneAppURL"];
            iPadAppURL = WebConfigurationManager.AppSettings["iPadAppURL"];
            FacebookURL = WebConfigurationManager.AppSettings["FacebookURL"];
            #endregion

            Environment = WebConfigurationManager.AppSettings["Environment"];

            #region authentication
            ProdAdminUserName = WebConfigurationManager.AppSettings["ProdAdminUserName"];
            ProdAdminPassword = WebConfigurationManager.AppSettings["ProdAdminPassword"];

            DevAdminUserName = WebConfigurationManager.AppSettings["DevAdminUserName"];
            DevAdminPassword = WebConfigurationManager.AppSettings["DevAdminPassword"];
            #endregion
            #region
            MailChimpKey = WebConfigurationManager.AppSettings["MailChimpKey"];
            #endregion
        }
        public static string GetValue(string appkey)
        {
            return WebConfigurationManager.AppSettings[appkey];
        }
        public static ConfigurationHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigurationHelper();
                    }
                    return instance;
                }
            }
        }

    }
}