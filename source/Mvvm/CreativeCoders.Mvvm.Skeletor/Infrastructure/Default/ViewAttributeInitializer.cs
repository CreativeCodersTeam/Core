using System;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;

[PublicAPI]
public class ViewAttributeInitializer : IViewAttributeInitializer
{
    private readonly IViewModelToViewMappings _viewModelToViewMappings;

    public ViewAttributeInitializer(IViewModelToViewMappings viewModelToViewMappings)
    {
        _viewModelToViewMappings = viewModelToViewMappings;
    }

    public void InitFromAssembly(Assembly assembly)
    {
        var typesWithAttributes =
            from type in assembly.GetTypesSafe()
            let attribute = type.GetCustomAttribute<ViewAttribute>()
            where attribute != null
            select (ViewType: type, ViewAttribute: attribute);

        typesWithAttributes.ForEach(x =>
        {
            var viewModelType = x.ViewAttribute?.ViewModelType;

            if (viewModelType == null)
            {
                throw new ArgumentException("No view model type declared");
            }
                
            _viewModelToViewMappings.AddMapping(viewModelType, x.ViewType);
        });
    }

    public void InitFromAllAssemblies()
    {
        ReflectionUtils.GetAllAssemblies().ForEach(InitFromAssembly);
    }
}