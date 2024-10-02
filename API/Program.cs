
using API.Services;
using BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Helpers.LoggerService;
using Shared.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

// ................................................................ Starting
// .........................................................................

BLLInjector.BLLConfigureServices(builder.Services, builder.Configuration); 

string origin = builder.Configuration.GetSection("Clients").GetSection("AngularClient").GetSection("Url").Value;


//  ......................................... Cors
builder.Services.AddCors(options => {
    options.AddPolicy(name: "AngularClient",
        builder => {
            builder.WithOrigins(AppSettings.ClientOrigin).AllowAnyHeader().AllowAnyMethod();
        });
});


// JWT Authentication & Authorization

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = AppSettings.ApiValidIssuer,
        ValidAudience = AppSettings.ApiValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SymmetricSecurityKey))
    };
});


//JSON TimeSpan Converter
builder.Services.AddControllers().AddJsonOptions(opts => {
    opts.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
});

// ............................................... Authorize Button

// Add authentication to Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ReCom Consulting Limited", Version = "API v1" });

    options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
        
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT"
                }
            },
            new string[] { }
        }
    });
});



builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

// .................................................................. Ending
// .........................................................................




var app = builder.Build();


// Initialize WebEnvironmentHelper
WebEnvironmentHelper.Initialize(app.Environment);


// var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // ....................................................... UI Design
    // ....................................................... Starting
    // ..................................................................
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RCL API v1");

        // Custom styles for Swagger UI
        c.ConfigObject.DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None;
        c.HeadContent = @"
        <style>
            body {
                font-size: 12px;
                background-color: #f5f5f5;
            }
            .swagger-ui .topbar {
                background-color: #3b5998;
            }
            .swagger-ui .scheme-container .schemes {
                font-size: 12px;
                color: #333333;
            }
            .swagger-ui .topbar .wrapper .title span,
            .swagger-ui .topbar .wrapper .info .title,
            .swagger-ui .topbar .wrapper .info .description p,
            .swagger-ui .topbar .wrapper .info .version,
            .swagger-ui .topbar .wrapper .info .contact,
            .swagger-ui .topbar .wrapper .info .license,
            .swagger-ui .info .title h1,
            .swagger-ui .info .version p,
            .swagger-ui .info .contact h4,
            .swagger-ui .info .license h4,
            .swagger-ui .opblock-summary-description p,
            .swagger-ui .opblock-section-header,
            .swagger-ui .opblock .opblock-section .opblock-section-header h4,
            .swagger-ui .opblock .opblock-body .opblock-body pre,
            .swagger-ui .opblock .opblock-body .opblock-media-type,
            .swagger-ui .opblock .opblock-request .opblock-request-body pre,
            .swagger-ui .opblock .opblock-request .opblock-request-body .opblock-media-type,
            .swagger-ui .opblock .opblock-response .opblock-response-body pre,
            .swagger-ui .opblock .opblock-response .opblock-response-body .opblock-media-type,
            .swagger-ui .footer,
            .swagger-ui .info .description p {
                font-size: 12px;
                color: #555555;
            }
            .swagger-ui .wrapper .info .title {
                color: #3b5998;
                font-size: 30px;
            }
        </style>
    ";
    });

    //  ............................................................... Ending
    //  ......................................................................
}

app.UseHttpsRedirection();


// .................................................................. Starting
// ...........................................................................

app.UseStaticFiles();
app.UseRouting();


// ............................. Cors
app.UseCors("AngularClient");

// ............................. Authentication
app.UseAuthentication();

// .................................................................... Ending
// ...........................................................................

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    // Map the default route to the Home controller's Index action
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
