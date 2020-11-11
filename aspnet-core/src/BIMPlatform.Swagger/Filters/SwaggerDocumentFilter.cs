using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace BIMPlatform.Swagger.Filters
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tags = new List<OpenApiTag>
            {
                new OpenApiTag {
                    Name = "MinIO",
                    Description = "MinIO 文件上传接口",
                    ExternalDocs = new OpenApiExternalDocs { Description = "文件上传接口" }
                },
                new OpenApiTag
                {
                    Name="Oss",
                    Description="OSS文件上传接口",
                    ExternalDocs= new OpenApiExternalDocs { Description="OSS文件上传接口"}
                }
               
            };

            #region 实现添加自定义描述时过滤不属于同一个分组的API
            var path = swaggerDoc.Paths;
            // 当前分组名称
            var groupName = swaggerDoc.Info.Version;
            var apis = context.ApiDescriptions.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(context.ApiDescriptions) as IEnumerable<ApiDescription>;

            string[] defaultcontroller = { "Account", "AbpTenant", "IdentityRole", "IdentityUser", "IdentityUserLookup", "Profile", "Permissions", "Features", "Tenant", "AbpApplicationConfiguration", "AbpApiDefinition" };

            if (groupName != ApiGrouping.GroupName_v1)
            {

                //controllers = controllers.Where(c => !defaultcontroller.Contains(c));
                foreach (ApiDescription apiDescription in apis)
                {
                    var item = ((ControllerActionDescriptor)apiDescription.ActionDescriptor).ControllerName;
                    if (defaultcontroller.Contains(item))
                    {
                        string key = "/" + apiDescription.RelativePath;
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        swaggerDoc.Paths.Remove(key);
                    }
                }
                // 当前所有的API对象

                // 不属于当前分组的所有Controller
                // 注意：配置的OpenApiTag，Name名称要与Controller的Name对应才会生效。
                var controllers = apis.Where(x => x.GroupName == groupName).Select(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName).Distinct();

                // 筛选一下tags
                //swaggerDoc.Tags = tags.Where(x => controllers.Contains(x.Name)).OrderBy(x => x.Name).ToList();
            }
            else
            {
                
                foreach (var item in path)
                {
                    if (!swaggerDoc.Paths.ContainsKey(item.Key))
                    {
                        swaggerDoc.Paths.Add(item.Key, item.Value);
                    }
                }

            }
           

            #endregion
        }


    }
}
