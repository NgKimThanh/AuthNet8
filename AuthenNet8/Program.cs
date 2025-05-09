﻿using AuthenNet8.Entities;
using AuthenNet8.Middleware;
using AuthenNet8.Repositories.Users;
using AuthenNet8.Services.Auth;
using AuthenNet8.Services.Email;
using AuthenNet8.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper: Tự động ánh xạ dữ liệu từ Model của ứng dụng sang Model của Entity
builder.Services.AddAutoMapper(typeof(Program));

// Life cycle DI: AddSingleton(), AddTransient(), AddScoped()
//builder.Services.AddScoped<IGroupOfProductService, GroupOfProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHttpContextAccessor();

// Khởi tạo conection string
builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBDemoAuthen"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Cấu hình xác thực bằng JWT (JSON Web Token) 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value ?? string.Empty)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Middleware
// Anti XSS
app.UseMiddleware<AntiXssMiddleware>();
// Exception Handler
app.UseMiddleware<ExceptionHandler>();
#endregion Middleware

app.UseAuthorization();

// Ánh xạ các route vào Controller
app.MapControllers();

app.Run();
