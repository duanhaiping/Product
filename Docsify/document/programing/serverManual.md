> git 项目代码文件结构

git  的目录下有三个文件 aspnet-core，Docsify和vue-element-admin ，Docsify 是在线文档，vue-element-admin 是前端项目，主要看aspnet-core：

```
│   .gitattributes
│   .gitignore
│   common.props
│   BIMPlatform.sln
├───src
│   ├───BIMPlatform.Application
│   ├───BIMPlatform.Application.Contracts
│   ├───BIMPlatform.BackgroundJobs
│   ├───BIMPlatform.DbMigrator
│   ├───BIMPlatform.Domain
│   ├───BIMPlatform.Domain.Shared
│   ├───BIMPlatform.EntityFrameworkCore
│   ├───BIMPlatform.EntityFrameworkCore.DbMigrations
│   ├───BIMPlatform.HttpApi
│   ├───BIMPlatform.HttpApi.Host
│   ├───BIMPlatform.Swagger
│   └───BIMPlatform.ToolKits
└───test
    ├───BIMPlatform.Application.Tests
    ├───BIMPlatform.Domain.Tests
    ├───BIMPlatform.EntityFrameworkCore.Tests
    ├───BIMPlatform.HttpApi.Client.ConsoleTestApp
    ├───BIMPlatform.TestBase
    └───BIMPlatform.Web.Tests
```

- Domain项目<br/>
  解决方案的领域层. 它主要包含 实体, 集合根, 领域服务, 值类型, 仓储接口 和解决方案的其他领域对象.
  实体和 Repository 接口都放在这个项目中.
  依赖 Domain.Shared 项目,因为项目中会用到它的一些常量,枚举和定义其他对象.

- Application 项目<br/>
  项目包含 Application.Contracts 项目的 [应用服务](https://docs.abp.io/en/abp/latest/Application-Services) 接口实现.
  依赖 Application.Contracts 项目,需要实现接口与使用DTO.
  依赖 Domain 项目,使用领域对象(实体,仓储接口等)执行应用程序逻辑.

- Application.Contracts 项目<br/>
  项目包含 [应用服务](https://docs.abp.io/en/abp/latest/Application-Services) 接口、应用层的DTO. 它用于分离应用层的接口和实现. 这样就可以把契约共享出去成为系统的标准.
  依赖 Domain.Shared 因为会在应用接口和DTO中使用常量,枚举和其他的共享对象。

- EntityFrameworkCore 项目<br/>
  集成EF Core的项目. 定义DbContext 和实现Domain 项目中定义的仓储接口.

- EntityFrameworkCore.DbMigrations 项目
  EF Core数据库迁移. 独立的 DbContext （仅用于数据库迁移）进行迁移动作.
  依赖 .EntityFrameworkCore 项目，用应用程序的 DbContext 配置

- DbMigrator 项目
  控制台应用程序,有单独的 appsettings.json 文件. 记得配置更改.
  依赖 EntityFrameworkCore.DbMigrations 项目 访问迁移文件.
  依赖 Application.Contracts 项目.

- HttpApi 项目
  定义API控制器.（Abp会自动动态创建，有需要另外实现的可在此项目中实现）
  依赖 Application.Contracts 项目,注入应用服务接口.

- Host项目
  依赖 HttpApi 项目,使用API和应用服务接口

- BackgroundJobs项目
  后台任务处理程序，处理定时任务

  依赖 Application.Contracts 项目

- Swagger项目

  接口文档工具项目

- Test 项目
  测试项目

> 模块开发（demo project）

1.  **创建实体**

    在BIMPlatform.Domain 项目中，创建模块文件夹Projects，在文件夹中创建 实体Project，必须继承接口Entity<Key>,根据需要继承其他默认接口，介绍几种常见的接口

-    IAuditedObject  ：审计属性接口

  包含属性有 CreationTime，CreatorId，LastModifierId，LastModificationTime

- ISoftDelete ： 软删除接口

  包含属性有 IsDeleted

- IDeletionAuditedObject ：删除属性

  继承ISoftDelete 

  包含属性有 DeleterId ，DeletionTime，IsDeleted

- IMultiTenant ：多租户属性

  包含属性有 TenantId

  实体兼容ASP.NET Core模型验证系统 ，具体参考 ASP.NET Core 内置特性

2. **生成数据库迁移文件**

   在BIMPlatform.EntityFrameworkCore文件中，在BIMPlatformDbContext 中增加自己创建的实体， public DbSet<Projects.Project> Projects { get; set; }，在BIMPlatformDbContextModelCreatingExtensions 中增加实体创建的配置信息，完成后在程序包管理器控制台中 生成迁移文件，迁移命令：add-migration [Name] , 如：add-migration init 

3. **生成数据库表**

   切换启动项目为BIMPlatform.DbMigrator ，点击运行就可以
   
4. **创建Dto和params**

   在contracts 项目中，创建对应的领域模型文件夹project ，定义projectDto和ProjectCreateParam 的等

   在接下来的service 中使用

5. **创建repository** 

   在domain 层中的projects 文件夹中，定义IProjectRepository 接口

   在EntityFrameworkCore 层中的Repositories对应的文件夹project 下，创建实现，ProjectRepository

6. **创建autoMapper 映射**

   autoMapper 映射需要在BIMPlatformApplicationAutoMapperProfile 中添加映射关系，  CreateMap<Projects.Project, ProjectDto>(); CreateMap< ProjectDto, Projects.Project>();

7. **创建service**

   在BIMPlatform.Application 中创建ProjectService ,在文件夹下定义接口IProjectService ，新建impl 文件夹，创建接口的实现ProjectService 

8. 创建api

   在httpapi 层中的Controllers 下创建对应的文件夹project ，在project 文件夹中创建ProjectController ，继承BaseController ，注意项：

   - route

     路由采用restful 规则，基本不需要修改，只有在查询指定的id  时,需要加载[Route("{id}")] 标识

   - http 类型

     包含post , put , get ,delete,通用规则为，请求获取数据，只读操作使用get，新增，申请、文件上传使用post,更新，审核，审批操作使用put,删除使用delete

   - 参数类型

     参数类型<br/>

     FromQuery : QueryString 类型传参，如api/prohect/get?name=zhangsan

     FromBody:Boby 传参。参数卸载ajax 中的data 中，

     FromForm:表单提交，参数来源于form表单

     FromRoute ：参数来源于路由，

     FromHeader：参数来源于请求头

   - 返回类型

     通用返回类型ServiceResult ，支持分页，列表等

   - 接口文档返回类型说明

      [SwaggerResponse(200, "", typeof(ServiceResult<IList<ProjectDto>>))]

     各个相应状态的返回包装类型，swagger 可见
   
   > TODO 待办事项(待办任务列表)

在项目中暂时为解决的问题的地方增加 // TODO: 【描述】，在视图=》任务列表中查看当前待办事项，任务结束后删除 // TODO: 【描述】