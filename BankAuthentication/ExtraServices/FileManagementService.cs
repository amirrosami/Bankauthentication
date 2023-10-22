using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace BankAuthentication.ExtraServices
{

    public class GetFileResponse
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
    public class FileManagementService
    {
        private readonly IAmazonS3 _amazonS3;

        public FileManagementService()
        {
           
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("615fa39c-ef90-484e-a983-e0e531c18b21", "d469de4123279e9ecb1357f8b8f69677c4befaf4b0dd5f4044cb54969f30bcd1");
            var config = new AmazonS3Config { ServiceURL = "https://s3.ir-thr-at1.arvanstorage.ir/" /*"s3.ir-thr-at1.arvanstorage.ir"*/ };
            Console.WriteLine("amir111");
            _amazonS3 = new AmazonS3Client(awsCredentials, config);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string bucketname = "userimages";
            string key = Guid.NewGuid().ToString();
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketname,
                Key = key,
                InputStream=file.OpenReadStream(),
                ContentType = file.ContentType,
            };
            PutObjectResponse response = await _amazonS3.PutObjectAsync(putRequest);
            foreach (PropertyInfo prop in response.GetType().GetProperties())
            {
                Console.WriteLine($"{prop.Name}: {prop.GetValue(response, null)}");
            }
            if (response.HttpStatusCode==System.Net.HttpStatusCode.OK)
            {
                return key;
            }
            return "";
        }


        public async Task<GetFileResponse> GetFile(string Key)
        {
            const string bucketName = "<BUCKET_NAME>";
            var keyName = "";

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            IAmazonS3 _s3Client = new AmazonS3Client(awsCredentials, config);
          var obj=  await ReadObjectDataAsync(_s3Client, bucketName, keyName);
            return obj;
        }

        static async Task<GetFileResponse> ReadObjectDataAsync(IAmazonS3 client, string bucketName, string keyName)
        {
            string responseBody = string.Empty;

           
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                };

            using GetObjectResponse response = await client.GetObjectAsync(request);
                Stream responseStream = response.ResponseStream;
                byte[] bytes;
                using (var reader = new StreamReader(responseStream))
                {
                    bytes = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd());
                }

            return new GetFileResponse {
                ContentType = response.Headers.ContentType,
                Data = bytes,
                Name = ""
                };
      

                
        }
           
        


    }

    
        // Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
        // SPDX-License-Identifier:  Apache - 2.0
        // The following example shows two different methods for uploading data to
        // an Amazon Simple Storage Service (Amazon S3) bucket. The method,
        // UploadObjectFromFileAsync, uploads an existing file to an Amazon S3
        // bucket. The method, UploadObjectFromContentAsync, creates a new
        // file containing the text supplied to the method. The application
        // was created using AWS SDK for .NET 3.5 and .NET 5.0.

        class UploadObject
        {
            private static IAmazonS3 _s3Client;

            private const string BUCKET_NAME = "<BUCKET_NAME>";
            private const string OBJECT_NAME = "<OBJECT_NAME>";

            private static string LOCAL_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            static async Task Main()
            {
                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
                var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
                _s3Client = new AmazonS3Client(awsCredentials, config);

                // The method expects the full path, including the file name.
                var path = $"{LOCAL_PATH}/{OBJECT_NAME}";

                await UploadObjectFromFileAsync(_s3Client, BUCKET_NAME, OBJECT_NAME, path);
              
            }

            /// <summary>
            /// This method uploads a file to an Amazon S3 bucket. This
            /// example method also adds metadata for the uploaded file.
            /// </summary>
            /// <param name="client">An initialized Amazon S3 client object.</param>
            /// <param name="bucketName">The name of the S3 bucket to upload the
            /// file to.</param>
            /// <param name="objectName">The destination file name.</param>
            /// <param name="filePath">The full path, including file name, to the
            /// file to upload. This doesn't necessarily have to be the same as the
            /// name of the destination file.</param>
            private static async Task UploadObjectFromFileAsync(
                IAmazonS3 client,
                string bucketName,
                string objectName,
                string filePath)
            {
                try
                {
                    var putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = objectName,
                        FilePath = filePath,
                        ContentType = "text/plain"
                    };

                    putRequest.Metadata.Add("x-amz-meta-title", "someTitle");

                    PutObjectResponse response = await client.PutObjectAsync(putRequest);
                    
                    foreach (PropertyInfo prop in response.GetType().GetProperties())
                    {
                        Console.WriteLine($"{prop.Name}: {prop.GetValue(response, null)}");
                    }

                    Console.WriteLine($"Object {OBJECT_NAME} added to {bucketName} bucket");
                }
                catch (AmazonS3Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }
    }


