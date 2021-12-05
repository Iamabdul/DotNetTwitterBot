using TwitterBot.Core.Helpers;
using TwitterBot.Helpers;

var builder = WebApplication.CreateBuilder(args);

//Configuring services
builder.Services.ConfigureTwitterBotServices(); //extension

var app = builder.Build();

//Endpoints to expose
app.MapEndpoints(); //extension

app.Run();