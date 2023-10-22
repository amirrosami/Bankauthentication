

using Microsoft.Extensions.DependencyInjection;
using RabbitMQConsumer;
using RabbitMQConsumer.MessageHandlers;
using RabbitMQConsumer.Repository;
using RabbitMQConsumer.Service;

var services = new ServiceCollection();
services.AddScoped<Consumer>();
services.AddDbContext<BankDbContext>();
services.AddScoped<BankAuthHandler>();
services.AddScoped<UserRepository>();
services.AddScoped<ImageProcessingService>();
services.AddScoped<FileManagementService>();
var serviceProvider = services.BuildServiceProvider();
var _consumer = serviceProvider.GetService<Consumer>();
var _file = serviceProvider.GetService<FileManagementService>();
var _Image = serviceProvider.GetService<ImageProcessingService>();
//_appDbContext = serviceProvider.GetService<ApplicationDbContext>();
await _consumer.ConsumeQueue("bank");
//MailgunService.SendEmail("ahrostami4@gmail.com");

    