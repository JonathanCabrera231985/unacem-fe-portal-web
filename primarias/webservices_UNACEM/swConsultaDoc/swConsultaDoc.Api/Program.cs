using ClibLogger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using swConsultaDoc.Api.Services;
using swConsultaDoc.Api.Services.Contracts;
using swConsultaDoc.Data;
using swConsultaDoc.Data.Interfaces;
using swConsultaDoc.Domain;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IConsultaDoc, ConsultaDoc>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "wConsultaDoc.API", Version = "v1" });


    c.AddSecurityDefinition(JwtAuthenticationDefaults.AuthenticationScheme,
    new OpenApiSecurityScheme
    {
        Description = "Añade el JWT Authorization con la palabra Bearer.",
        Name = JwtAuthenticationDefaults.HeaderName, // Authorization
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtAuthenticationDefaults.AuthenticationScheme
            }
        },
        new List<string>()
    }
});
});



builder.Services.AddAuthorization(options =>
    options.DefaultPolicy =
    new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build()
);



//estos valores los obtenemos de nuestro appsettings
var issuer = builder.Configuration["AuthenticationSettings:Issuer"];
var audience = builder.Configuration["AuthenticationSettings:Audience"];
var signinKey = builder.Configuration["AuthenticationSettings:SigningKey"];
//autenticacion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signinKey!))
    };
});


builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));//inyectando servicio
builder.Services.AddScoped(typeof(IDocumentoService), typeof(DocumentoService));//inyectando servicio


var app = builder.Build();

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
