using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;

namespace MultiConverter.DependencyInjection.Modules;

public class AutoMapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterProfiles(builder);
    }

    private static void RegisterProfiles(ContainerBuilder builder)
    {
        builder.RegisterAutoMapper(typeof(App).Assembly);
    }
}
