using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer.Service
{
    public class MailgunService
    {
        private static readonly RestClient _client=new RestClient("https://api.mailgun.net/v3");
        private static readonly string _apikey = "c4e22e4116489f82004c414f82467db8-3750a53b-c79b1e18";

        public static async void SendEmail(string to,string subject,string body)
        {
            RestRequest request = new RestRequest();
            request.Authenticator= new HttpBasicAuthenticator("3750a53b-c79b1e18", _apikey);
            request.AddParameter("domain", "sandboxd3674f25defd4a6b81a05769670dd792.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "mailgun@sandboxd3674f25defd4a6b81a05769670dd792.mailgun.org");
            request.AddParameter("to", to);
           
            request.AddParameter("subject", subject);
            request.AddParameter("text", body);
            request.Method = Method.Post;
            var response=await _client.ExecuteAsync(request);
            Console.WriteLine(response.ToString());
        }
    }
}
