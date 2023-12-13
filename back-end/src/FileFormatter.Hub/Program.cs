using FileFormatter.HubRunner.Abstractions;
using FileFormatter.HubRunner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICache, Cache>();
builder.Services.AddSignalR();

var app = builder.Build();
app.UseCors(x => x
        .AllowAnyOrigin()
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod());
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ClientHub>("/test");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
