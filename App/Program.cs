using App;
using App.Extensions;
using Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.RegisterApplicationDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(ApplicationConstants.DevelopmentCorsPolicyName);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // "Why make trillions, when we can make... billions?" - Dr. Evil
    await app.Services.EnsureDatabaseUpdated();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCompression();

app.UseStaticFiles(StaticFileOptionsFactory.Create());

app.MapFallbackToFile("index.html");

app.MapControllers();

app.Run();
