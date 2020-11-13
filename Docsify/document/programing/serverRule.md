> 命名规范

类，接口，枚举、全局变量等使用驼峰命名法 如：ProjectController ，ProjectService 等

局部变量，参数等使用小驼峰，如:projectService ,createParams 等

权限：

规则为 权限组 . 模块名. 权限类型，

权限类型为：

- Default
- Create
- Update
- Delete
- Manage
- Audit
- Approve
- Apply

语言本地化：

规则：信息类型：模块：Key

比如：Info:Permission:Create (一般信息：权限模块：创建权限)

​			Error:Project:NameDuplicate(错误信息：项目模块：名称重复)

> API 请求规范

Get:获取数据

Delete:删除操作

Post:新增

Put :更新  替换整个资源

Pach:指定更改







