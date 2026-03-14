using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Acme.SimpleTaskApp.Authorization;
using Acme.SimpleTaskApp.Tasks;
using Acme.SimpleTaskApp.Tasks.Dtos;

namespace Acme.SimpleTaskApp;

[DependsOn(
    typeof(SimpleTaskAppCoreModule),
    typeof(AbpAutoMapperModule))]
public class SimpleTaskAppApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<SimpleTaskAppAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(SimpleTaskAppApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
        {
            cfg.AddMaps(thisAssembly);
            cfg.CreateMap<Task, TaskListDto>();
            cfg.CreateMap<CreateTaskInput, Task>();
            cfg.CreateMap<UpdateTaskInput, Task>();
        });
    }
}
