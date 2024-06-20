using Application.Interfaces;
using Application.Services;
using Data.Context;
using Data.Messaging;
using Data.Repository;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;

namespace APIPagamento
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    // Cria um builder de aplicação web com os argumentos passados.
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria um builder de aplicação web com os argumentos passados.
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();

            // Adiciona serviços ao contêiner.
            //// Adiciona um serviço do tipo MySQLContext ao objeto builder.Services.
            // builder.Services.AddDbContext<MySQLContext>();

            // Adiciona um serviço do tipo MongoDBContext ao objeto builder.Services.
            builder.Services.AddSingleton<MongoDBContext>();

            // Adiciona os serviços de controllers ao builder.
            builder.Services.AddControllers(options =>
            {
                // Insere um formato de entrada personalizado para o JsonPatch.
                options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
            });

            // Adiciona os serviços específicos ao contêiner.
            builder.Services.AddScoped<IPagamentoService, PagamentoService>();

            // Adiciona os repositórios específicos ao contêiner.
            builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

            builder.Services.AddScoped<IPagamentoMessageQueue, PagamentoMessageQueue>();
            builder.Services.AddScoped<IPagamentoMessageSender, PagamentoMessageSender>();
            builder.Services.AddScoped<IPagamentoMessageService, PagamentoMessageService>();
            builder.Services.AddHostedService<PagamentoWorkerService>();
            builder.Services.AddScoped<IPagamentoScopedService, PagamentoScopedService>();            

            // Adiciona o suporte ao NewtonsoftJson aos controllers.
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Configura as opções de rota para usar URLs e query strings em minúsculo.
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            // Configura os serviços relacionados aos controladores.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(AjustaDataHoraLocal));
            }).AddNewtonsoftJson(options =>
            {
                // Usa a formatação padrão (PascalCase) para as propriedades.
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });

            // Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var licenseUrl = Environment.GetEnvironmentVariable("LicenseUrl");
            // Configuração do SwaggerGen para gerar a documentação da Web API.
            builder.Services.AddSwaggerGen(
                c =>
                {
                    // Habilita o uso de anotações (como [SwaggerOperation]) para melhorar a documentação.
                    c.EnableAnnotations();
                    // Define a versão da documentação Swagger como "v1".
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Tech Challenge - Grupo 13 - Fase IV",
                        Description = "Documentação dos endpoints da API sobre o uso de microsserviços de pagamento.",
                        Contact = new OpenApiContact() { Name = "Tech Challenge - Grupo 13", Email = "grupo13@fiap.com" },
                        License = new OpenApiLicense() { Name = "MIT License", Url = licenseUrl != null ? new Uri(licenseUrl) : null },
                        Version = "1.0.11"
                    });

                    // Habilita o uso para registrar o SchemaFilter.
                    c.SchemaFilter<PagamentoInputSchemaFilter>();
                }
            );

            var app = builder.Build();

            // Adiciona o middleware de codificação para garantir a codificação correta em todas as respostas.
            app.Use((context, next) =>
            {
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.Headers.ContentEncoding = "utf-8";
                context.Response.Headers.ContentLanguage = CultureInfo.CurrentCulture.Name;
                return next();
            });

            // Configura a pipeline de solicitação HTTP.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.DefaultModelRendering(ModelRendering.Example);
                    /// c.DefaultModelExpandDepth(-1);
                    /// c.DefaultModelsExpandDepth(-1);
                    /// c.DocExpansion(DocExpansion.None);
                    /// c.DisplayRequestDuration();
                    c.DisplayOperationId();
                    c.EnableDeepLinking();
                    c.EnableFilter();
                    c.ShowExtensions();
                    c.EnableValidator();
                    /// c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
                });

                app.UseReDoc(c =>
                {
                    c.DocumentTitle = "REDOC API Documentation";
                    c.SpecUrl = "/swagger/v1/swagger.json";
                });
            }

            // Adiciona o middleware de autorização à pipeline de solicitação HTTP.
            // Este middleware é responsável por garantir que o usuário esteja autorizado a acessar os recursos solicitados.
            app.UseAuthorization();

            // Adiciona o middleware de roteamento de controladores à pipeline de solicitação HTTP.
            // Este middleware é responsável por rotear as solicitações HTTP para os controladores apropriados.
            app.MapControllers();

            // Inicia a execução da aplicação.
            // Este método bloqueia o thread chamado e aguarda até que a aplicação seja encerrada.
            app.Run();
        }
    }
}