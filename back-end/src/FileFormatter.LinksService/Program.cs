using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using FileFormatter.StoreIntegrator.Extensions;
using FileFormatter.LinksService.Abstractions;
using FileFormatter.LinksService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>{});
builder.Services.AddMinioStore(builder.Configuration);
builder.Services.AddScoped<ILinkBuilderService, LinkBuilderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => { });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
