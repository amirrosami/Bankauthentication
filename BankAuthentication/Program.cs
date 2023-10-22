using BankAuthentication.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBankService();
builder.Services.AddHttpClient("localhost").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.Use(async (context, next) =>
{
    Console.WriteLine("amir");
    await next.Invoke();
}
    );
*/
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
