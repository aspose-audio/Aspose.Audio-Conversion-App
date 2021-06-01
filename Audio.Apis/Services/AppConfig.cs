using Microsoft.AspNetCore.Hosting;
using System;
using System.Security;

namespace AudioVedio.Apis.Services
{
    public class AppConfig
    {
        public static string TempRootDirectory;
        public static string FontRootDirectory;

        public static string ForumKey;
        public static string ForumUrl;
        public static string ForumCategoryId;

        public static string AwsAccessKey;
        public static string AwsSecretKey;
        public static string S3Region;
        public static string S3BucketName;
        
        internal static string MailServer;
        internal static int MailServerPort;
        internal static int MailServerTimeOut;
        internal static string MailServerUserId;
        internal static string MailServerUserPassword;
        internal static string FromEmailAddress;
        internal static string FeedbackEmail;

        internal static string EsUrl;
        internal static string EsLogin;
        internal static string EsPassword;

        internal static string RedisKey;
        internal static string RedisConnectionString;
        internal static string FFmpegPath;
        static AppConfig()
        {
            TempRootDirectory = GetConfigValue("TempRootDirectory");
            FontRootDirectory = GetConfigValue("FontRootDirectory");

            ForumKey = GetConfigValue("ForumKey");
            ForumUrl = GetConfigValue("ForumUrl");
            ForumCategoryId = GetConfigValue("ForumCategoryId").ToString();

            AwsAccessKey = GetConfigValue("AwsAccessKey").ToString();
            AwsSecretKey = GetConfigValue("AwsSecretKey").ToString();
            S3Region = GetConfigValue("S3Region").ToString();
            S3BucketName = GetConfigValue("S3BucketName").ToString();

            FeedbackEmail = "william.shen@aspose.com";
            FromEmailAddress = "support@aspose.app";
            MailServer = "exchange.aspose.com";
            MailServerPort = 587;
            MailServerTimeOut = 6000000;
            MailServerUserId = GetConfigValue("MailServerUserId").ToString();
            MailServerUserPassword = GetConfigValue("MailServerUserPassword").ToString();

            EsUrl = GetConfigValue("EsUrl").ToString();
            EsLogin = GetConfigValue("EsLogin").ToString();
            EsPassword = GetConfigValue("EsPassword").ToString();

            RedisKey = GetConfigValue("RedisKey").ToString();
            RedisConnectionString = GetConfigValue("RedisConnectionString").ToString();
            FFmpegPath = GetConfigValue("FFmpegPath").ToString();
        }

        private static string GetConfigValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}