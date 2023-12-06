using BE.Application.DependencyInjection.Extentions;
using BE.Persistence.DependencyInjection.Extensions.ServiceCollectionExtensions;
using BE.Persistance.DependencyInjection.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add configuration
builder.Services.AddConfigureMediatR();
//builder.Services.ConfigureSqlServerRetryOptions();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
