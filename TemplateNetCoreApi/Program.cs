using Serilog;
using TemplateNetCoreApi.Extension;
using TemplateNetCoreApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region config log
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion

#region add CORS
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
      corsPolicyBuilder =>
      {
          corsPolicyBuilder
          .AllowAnyHeader()
          .AllowAnyMethod()
          .SetIsOriginAllowed(origin => true)
          .AllowCredentials();
      });
});
#endregion

#region config swagger document
// Add a Swagger generator and Automatic Request and Response annotations:
builder.Services.AddSwaggerSwashbuckleConfigured();

//builder.Services.ConfigureSwagger();
#endregion

// Add services to the container.
builder.Services.RegisterDependencies(); // đăng kí dependency
builder.Services.ConfigureMapping();

builder.Services.ConfigureSqlContext(builder.Configuration); // đăng ký db context
builder.Services.ConfigureRepositoryManager(); // đăng ký service manager

builder.Services.AddAuthentication(); // thông báo có sử dụng chức năng Authen
builder.Services.ConfigureIdentity(); // thông báo sử dụng Identity
builder.Services.ConfigureJWT(builder.Configuration); // sử dụng Jwt

builder.Services.ConfigureControllers(); // đăng ký cache 30s

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

#region using CORS config
app.UseCors(myAllowSpecificOrigins);
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin,DNT,X-Mx-ReqToken,Keep-Alive,User-Agent,X-Requested-With,If-Modified-Since,Cache-Control,Content-Type");
    await next();
});
#endregion

app.UseMiddleware<ExceptionMiddleware>(); // bọc ngoài để catch những exception ko lường trước

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching(); // sử dụng cache 30s

app.UseAuthentication(); // thông báo có sử dụng chức năng Authen
app.UseAuthorization();

app.MapControllers();

app.Run();
