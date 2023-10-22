using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConsumer.MessageHandlers;
using RabbitMQConsumer.Service;
using System.Text;

namespace RabbitMQConsumer
{
   
       
    public class Consumer
    {
        private IConnection _connection;
        private IModel _channel;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private static HttpClient _httpClient=new HttpClient();
        private readonly BankAuthHandler _authHandler;

        public Consumer(BankAuthHandler authHandler)
        {
            _authHandler=authHandler;
        }

        public async Task ConsumeQueue(string queueName)
        {
            // CloudAMQP URL in format amqp://user:pass@hostName:port/vhost
            string _url = "amqps://udtrvaan:1L2J8yTgZoX7-h-imbgWTm8JQ_6pL6Vh@beaver.rmq.cloudamqp.com/udtrvaan";
            // create a connection and open a channel, dispose them when done
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_url)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // ensure that the queue exists before we access it
            
            bool durable = false;
            bool exclusive = false;
            bool autoDelete = true;

            _channel.QueueDeclare(queueName, durable, exclusive, autoDelete, null);

            var consumer = new EventingBasicConsumer(_channel);
            var status = 0;
            // add the message receive event
            consumer.Received += async (model, deliveryEventArgs) =>
            {
                var body = deliveryEventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var user=await _authHandler.ChangeStatus(message);
                if (user is not null)
                {
                    string text="";
                    if (user.StatusCode==1)
                    {
                        text = "Your Authentication was Successful!!";
                    }
                    else if (user.StatusCode == -1)
                    {
                        text = "Your Authntication Failed.";
                    }
                    Task.Factory.StartNew(() => MailgunService.SendEmail(user.EmailAddress, "Bank Authentication", text));
                }
                Console.WriteLine("ok");
                _channel.BasicAck(deliveryEventArgs.DeliveryTag, false);
            };

                _ = _channel.BasicConsume(consumer, queueName);
                _resetEvent.WaitOne();
                _channel?.Close();
                _channel = null;
                _connection?.Close();
                _connection = null;
                
           
            
            
        }
    }
}

