using System.Text;
using _01_FrameWork.Infrastructure.Configuration;
using AccountManagement.Infrastructure.Configuration;
using AccountManagement.Infrastructure.EFCore.Context;
using MasterManagement.Infrastructure.Configuration;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddMemoryCache();


builder.Services.AddDbContext<MasterContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddDbContext<AccountContext>(x => x.UseSqlServer(connectionString));


MasterManagementBootstrapper.Configure(builder.Services, connectionString);
AccountManagementBootstrapper.Configure(builder.Services, connectionString);
FrameworkBootstrapper.Configure(builder.Services, connectionString);


builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });


builder.Services.AddAuthorization();


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

    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token **_only_**"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
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

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();




//using AccountManagement.Infrastructure.Configuration;
//using MasterManagement.Infrastructure.Configuration;
//using MasterManagement.Infrastructure.EFCore.Context;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);


//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


//builder.Services.AddControllers();


//MasterManagementBootstrapper.Configure(builder.Services, connectionString);
//AccountManagementBootstrapper.Configure(builder.Services, connectionString);


//builder.Services.AddDbContext<MasterContext>(x => x.UseSqlServer(connectionString));
//builder.Services.AddApiVersioning(options =>
//{
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
//    options.ReportApiVersions = true;
//});


//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Master Management API",
//        Version = "v1",
//        Description = "API documentation for Master Management project"
//    });
//});


//var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Master Management API V1");
//        c.RoutePrefix = string.Empty; 
//    });
//}


//app.UseHttpsRedirection();


//app.UseAuthorization();


//app.MapControllers();


//app.Run();
