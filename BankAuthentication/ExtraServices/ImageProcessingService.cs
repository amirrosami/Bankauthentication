using Newtonsoft.Json;
using RestSharp;

namespace BankAuthentication.ExtraServices
{
    public class ImageProcessingService
    {
        private readonly RestClient _client;
        private readonly string _BaseUrl="https://api.imagga.com/v2/";
        private readonly  string apiKey = "acc_2468058f6b2f577";
        private readonly string apiSecret = " 8a486dc89f98e44492515a109c2c7793 ";
        public ImageProcessingService(RestClient client)
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


        public void Detection(string name,string contnttype,byte[] data)
        {
            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));

            var request = new RestRequest("/faces/detections", method: Method.Post);
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));
            request.AddFile("image", data, name);
            var response = _client.Execute(request);
           // var imageresponse = JsonConvert.DeserializeObject<ResponseImage<UploadImage>>(response.Content);
           var jsonobj= JsonConvert.DeserializeObject(response.Content);
            
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
   
    
    public class UploadImage
    {
        public string Id { get; set; }
    }
    
    
}
