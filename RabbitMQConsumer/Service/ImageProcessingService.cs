using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;

namespace  RabbitMQConsumer.Service
{
    public class ImageProcessingService
    {
        private readonly RestClient _client;
        private readonly string _BaseUrl="https://api.imagga.com/v2/";
        private readonly  string apiKey = "acc_f5e5627ba38057d";
        private readonly string apiSecret = "0a747e384150c22eaede9579c8e122bd";
        public ImageProcessingService()
        {
            _client = new RestClient(_BaseUrl);
            
            
        }

        public string Upload(byte[] file,string name)
        {
           
            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));
            
            var request = new RestRequest("uploads",method:Method.Post);
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));
            request.AddFile("image",file,name);
            var response = _client.Execute(request);
            var imageresponse= JsonConvert.DeserializeObject<ResponseImage<UploadImage>>(response.Content);
            return imageresponse.Result.Id;
        }


        public DetectImage Detect(byte[] data)
        {
            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));
            var request = new RestRequest("/faces/detections", method: Method.Post);
            request.Timeout = -1;
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));
            request.AddParameter("image_base64", Convert.ToBase64String(data));
            request.AddParameter("return_face_id",1);
            var response = _client.Execute(request);

            // var imageresponse = JsonConvert.DeserializeObject<ResponseImage<UploadImage>>(response.Content);
            if (response.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var jsonobj = JObject.Parse(response.Content);
                var r=JsonConvert.DeserializeObject<DetectResult>(response.Content);
                if (r.result.faces.Count()>0)
                {
                    return new DetectImage { Confidence =r.result.faces.First().Confidence, face_id = r.result.faces.First().face_id, Status = 1 };
                }
                
            }
            return new DetectImage { Confidence = null, face_id = null ,Status=0};
        }


        public double? GetSimilarity(string faceId1,string faceId2)
        {
            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));
            var request = new RestRequest("/faces/similarity", method: Method.Get);
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));
           request.AddParameter("face_id", faceId1);
            request.AddParameter("second_face_id", faceId2);

            var response = _client.Execute(request);
            // var imageresponse = JsonConvert.DeserializeObject<ResponseImage<UploadImage>>(response.Content);
            var jsonobj = JObject.Parse(response.Content);
            var score = (double?)jsonobj["result"]?["score"];
            return score;

        }





    }

    public class ResponseImage<T>
    {
        public T Result { get; set; }
        public Status status { get; set; }
    }
    public class Status
    {
        public string  Type { get; set; }
        public string Text { get; set; }
    }
    public class DetectImage
    {
        public string face_id { get; set; }
        public double? Confidence { get; set; }
        public int Status  { get; set; }
        // public IEnumerable<Faces> faces { get; set; }
    }
    public class Result
    {
        public IEnumerable<Face> faces { get; set; }
        
    }

    public class Face
    {
        public double Confidence { get; set; }
        public coordinates Coordinates { get; set; }
        public string face_id { get; set; }

    }

    public class coordinates
    {
        public int height { get; set; }
        public int width { get; set; }
        public int xmax { get; set; }
        public int xmin { get; set; }
        public int ymax { get; set; }
        public int ymin { get; set; }
    }

    public class DetectResult
    {
        public Result result { get; set; }
        public Status status { get; set; }

    }
    public class UploadImage
    {
        public string Id { get; set; }
    }
    
    
}
