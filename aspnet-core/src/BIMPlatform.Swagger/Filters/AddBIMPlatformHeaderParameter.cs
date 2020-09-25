using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BIMPlatform.Swagger.Filters
{
    public class AddBIMPlatformHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// swagger 增加头部信息
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                //非匿名的方法,链接中添加accesstoken值
                if (!isAnonymous)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "__currentProject",
                        In = ParameterLocation.Header,//query header body path formData
                        Required = false //是否必选
                    });
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "accept-language",
                        Description= "value in en or zh-Hans" ,
                        In = ParameterLocation.Header,//query header body path formData
                        Required = false //是否必选
                    });
                }
            }

        }
    }
}
