using MasterManagement.Infrastructure.Configuration;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddControllers();


MasterManagementBootstrapper.Configure(builder.Services, connectionString);


builder.Services.AddDbContext<MasterContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Master Management API",
        Version = "v1",
        Description = "API documentation for Master Management project"
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Master Management API V1");
        c.RoutePrefix = string.Empty; 
    });
}


app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();


app.Run();
