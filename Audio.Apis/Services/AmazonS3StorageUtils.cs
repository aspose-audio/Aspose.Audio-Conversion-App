using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AudioVedio.Apis.Services
{
    public class AmazonS3StorageUtils
    {
        static RegionEndpoint regionEndpoint;
        static BasicAWSCredentials credentials; 
        static AmazonS3Client client;

        static AmazonS3StorageUtils()
        {
            regionEndpoint = RegionEndpoint.GetBySystemName(AppConfig.S3Region);
            credentials = new BasicAWSCredentials(AppConfig.AwsAccessKey, AppConfig.AwsSecretKey);
            client = new AmazonS3Client(credentials, regionEndpoint);
        }

        public async static Task<string> Upload(string key,Stream fs)
        {
            var req = new PutObjectRequest();
            req.InputStream = fs;
            req.AutoCloseStream = false;
            req.BucketName = AppConfig.S3BucketName;
            req.Key = key;
            req.CannedACL = S3CannedACL.BucketOwnerRead;
            await client.PutObjectAsync(req);

            return key;
        }

        internal async static Task UploadList(Dictionary<string, Stream> documents)
        {
            foreach (var item in documents) {
                await Upload(item.Key,item.Value);
            }
        }

        internal async static Task CopyFolder(string sourceFolder, string targetFolder)
        {
           await client.CopyObjectAsync(AppConfig.S3BucketName,sourceFolder, AppConfig.S3BucketName, targetFolder);
        }

        public async static Task<Stream> Download(string key)
        {
            var response= await client.GetObjectAsync(AppConfig.S3BucketName,key);
            return response.ResponseStream;
        }

        public static string GetPreSignedURL(string filePath)
        {
            var expiration = DateTime.Now + TimeSpan.FromDays(1);
            var sign = new GetPreSignedUrlRequest()
            {
                BucketName = AppConfig.S3BucketName,
                Key = filePath,
                Expires = expiration,
            };
            sign.ResponseHeaderOverrides.ContentType = "application/octet-stream";
            sign.ResponseHeaderOverrides.ContentDisposition="attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath));
            var url = client.GetPreSignedURL(sign);
            return url;
        }
    }
}