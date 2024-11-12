using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using TemplateNetCoreApi.Extension.MultiExample;
using TemplateNetCoreApi.Extension.Swagger.OptionalRouteParameter;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace TemplateNetCoreApi.Extension
{
    /// <summary>
    /// Configure the Swagger generator.
    /// </summary>
    public static class ConfigureSwaggerSwashbuckle
    {
        /// <summary>
        /// Configure the Swagger generator with XML comments, bearer authentication, etc.
        /// Additional configuration files:
        /// <list type="bullet">
        ///     <item>ConfigureSwaggerSwashbuckleOptions.cs</item>
        /// </list>
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerSwashbuckleConfigured(this IServiceCollection services)
        {
            // Configures ApiExplorer (needed from ASP.NET Core 6.0).
            services.AddEndpointsApiExplorer();

            // Register the Swagger generator, defining one or more Swagger documents.
            // Read more here: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

            services.AddSwaggerGen(options =>
            {
                // If we would like to provide request and response examples (Part 1/2)
                // Enable the Automatic (or Manual) annotation of the [SwaggerRequestExample] and [SwaggerResponseExample].
                // Read more here: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters
                options.ExampleFilters();

                // If we would like to include documentation comments in the OpenAPI definition file and SwaggerUI.
                // Set the comments path for the XmlComments file.
                // Read more here: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio#xml-comments
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);

                // If we would like to provide security information about the authorization scheme that we are using (e.g. Bearer).
                // Add Security information to each operation for bearer tokens and define the scheme.
                options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (JWT). Example: \"bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // If we use the [Authorize] attribute to specify which endpoints require Authorization, then we can
                // Show an "(Auth)" info to the summary so that we can easily see which endpoints require Authorization.
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();

                // Multi Example
                options.ParameterFilter<CustomParameterFilter>();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Scheduling Tool (Phase II)",
                    Description = "Danh sách APIs cung cấp cho project Template <br> " 
                      //"Design: <a href=\"https://www.figma.com/design/VXzRdIT3L22KX0Y8NOe7ng/08T45R.CST.NKE-Construction-Scheduling-Tool?node-id=121-8524&t=TgkH0yhrpwHNheGP-0\">Figma</a><br><br>" 
                      //"Config Hub: using /downloadHub to connection to server<br><br>"
                    //TermsOfService = new Uri( "https://example.com/terms" ),
                    //Contact = new OpenApiContact
                    //{
                    //  Name = "Example Contact",
                    //  Url = new Uri( "https://example.com/contact" )
                    //},
                    //License = new OpenApiLicense
                    //{
                    //  Name = "Example License",
                    //  Url = new Uri( "https://example.com/license" )
                    //}
                });
            });

            // If we would like to provide request and response examples (Part 2/2)
            // Register examples with the ServiceProvider based on the location assembly or example type.
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            // If we are using FluentValidation, then we can register the following service to add the  fluent validation rules to swagger.
            // Adds FluentValidationRules staff to Swagger. (Minimal configuration)
            services.AddFluentValidationRulesToSwagger();
        }

    }
}
