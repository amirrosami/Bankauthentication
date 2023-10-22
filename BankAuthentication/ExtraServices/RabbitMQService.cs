using RabbitMQ.Client;
using System.Text;

namespace BankAuthentication.ExtraServices
{
    public class RabbitMQPublisher:IRabbitMqPublisher
    {
        private static readonly string _url = "amqps://udtrvaan:1L2J8yTgZoX7-h-imbgWTm8JQ_6pL6Vh@beaver.rmq.cloudamqp.com/udtrvaan";
        public void Publish(string queueName, string message)
        {
        // Create a ConnectionFactory and set the Uri to the CloudAMQP url
        // the connectionfactory is stateless and can safetly be a static resource in your app
         var factory = new ConnectionFactory
            {
            Uri = new Uri(_url)
            };
            // create a connection and open a channel, dispose them when done
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            // ensure that the queue exists before we publish to it
            
            bool durable = false;
            bool exclusive = false;
            bool autoDelete = true;

            channel.QueueDeclare(queueName, durable, exclusive, autoDelete, null);

            // read message from input
            // the data put on the queue must be a byte array
            var data = Encoding.UTF8.GetBytes(message);
            // publish to the "default exchange", with the queue name as the routing key
            var exchangeName = "";
            var routingKey = queueName;
            channel.BasicPublish(exchangeName, routingKey, null, data);
        }


        


    }

    public interface IRabbitMqPublisher
    {
        void Publish(string queueName, string message);
    }
}
