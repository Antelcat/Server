using Antelcat.Foundation.Core.Implements.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Server.Implements;

public abstract class ControllerActivatorBase<TServiceProvider> : IControllerActivator 
    where TServiceProvider : IServiceProvider
{
    protected abstract TServiceProvider ProvideService(IServiceProvider provider);
    public object Create(ControllerContext? context)
    {
        if (context == null)
            throw new ArgumentNullException($"{nameof(ControllerContext)} is null");
        var provider = context.HttpContext.RequestServices;
        if (provider is not TServiceProvider)
            provider = ProvideService(context.HttpContext.RequestServices);
        return provider.GetRequiredService(context.ActionDescriptor.ControllerTypeInfo.AsType());
    }
    public void Release(ControllerContext? context, object controller)
    {
        if (context == null) throw new ArgumentNullException($"{nameof(ControllerContext)} is null");
        switch (controller)
        {
            case null:
                throw new ArgumentNullException(nameof(controller));
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }
}

public class AutowiredControllerActivator<TAttribute> 
    : ControllerActivatorBase<TransientAutowiredServiceProvider<TAttribute>> 
    where TAttribute : Attribute
{
    protected override TransientAutowiredServiceProvider<TAttribute> ProvideService(
        IServiceProvider provider) =>
        new(provider);
}
