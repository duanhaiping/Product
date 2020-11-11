using BIMPlatform.Configurations;
using BIMPlatform.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BIMPlatform.Swagger
{
    public static class BIMPlatformSwaggerExtensions
    {
        /// <summary>
        /// 当前API版本，从appsettings.json获取
        /// </summary>
        private static readonly string version = $"v{AppSettings.ApiVersion}";

        /// <summary>
        /// Swagger描述信息
        /// </summary>
        private static readonly string description = @"<b>Client</b>：<a target=""_blank"" href=""http://localhost:4200/"">前端项目地址</a> <b>Docsify</b>：<a target=""_blank"" href="" http://localhost:3000"">在线文档</a> <b>Hangfire</b>：<a target=""_blank"" href=""/hangfire"">任务调度中心</a> <code>Powered by .NET Core 3.1 </code>";

        /// <summary>
        /// Swagger分组信息，将进行遍历使用
        /// </summary>
        private static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>()
        {
            new SwaggerApiInfo{
                UrlPrefix = ApiGrouping.GroupName_v1,
                Name = "框架层接口- Abp vnext框架自带接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v1,
                    Title = "BIMPlus - Abp系统默认模块接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v2,
                Name = "宿主模块-平台管理角色使用的接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v2,
                    Title = "BIMPlus - 平台管理角色使用的接口",
                    Description = description
                }
            },
             new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v3,
                Name = "租户系统模块-企业级系统管理模块接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v3,
                    Title = "BIMPlus - 企业级系统管理模块接口",
                    Description = description
                }
             },
            new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v4,
                Name = "租户项目模块接口--企业级项目管理模块接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v4,
                    Title = "BIMPlus - 项目模块接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v5,
                Name = "租户项目模块接口--第三方接口钉钉",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v5,
                    Title = "BIMPlus - 第三方接口钉钉",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v6,
                Name = "租户项目模块接口--第三方接口微信",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v6,
                    Title = "BIMPlus - 第三方接口微信",
                    Description = description
                }
             },
            new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v7,
                Name = "租户项目模块接口--第三方接口QQ",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v7,
                    Title = "BIMPlus - 第三方接口QQ",
                    Description = description
                }
             },
             new SwaggerApiInfo
            {
                UrlPrefix = ApiGrouping.GroupName_v999,
                Name = "租户项目模块接口--未定义分组",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = ApiGrouping.GroupName_v999,
                    Title = "BIMPlus - 未定义分组",
                    Description = description
                }
             },
        };

        /// <summary>
        /// AddSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.DocInclusionPredicate((docName, description) =>
                {
                    if (!description.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var version = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(attr => attr.GroupName);
                    if (docName == ApiGrouping.GroupName_v1 && version.FirstOrDefault() == null)
                    {
                        return true;
                    }
                    return version.Any(v => v.ToString() == docName);
                });
                // 遍历并应用Swagger分组信息
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerDoc(x.UrlPrefix, x.OpenApiInfo);
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BIMPlatform.HttpApi.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BIMPlatform.Domain.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BIMPlatform.Application.Contracts.xml"));


                #region 小绿锁，JWT身份认证配置

                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入 Bearer {Token} 进行身份验证",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("oauth2", security);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();

                #endregion

                // 应用Controller的API文档描述信息
                // options.DocumentFilter<SwaggerDocumentFilter>();
                options.OperationFilter<AddBIMPlatformHeaderParameter>();
            });
        }

        /// <summary>
        /// UseSwaggerUI
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                // 遍历分组信息，生成Json
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerEndpoint($"/swagger/{x.UrlPrefix}/swagger.json", x.Name);
                });

                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                //options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                options.RoutePrefix = string.Empty;
                // API页面Title
                options.DocumentTitle = "😍接口文档 -BIMPlus⭐⭐⭐";
            });
        }

        internal class SwaggerApiInfo
        {
            /// <summary>
            /// URL前缀
            /// </summary>
            public string UrlPrefix { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
            /// </summary>
            public OpenApiInfo OpenApiInfo { get; set; }
        }
    }
}
