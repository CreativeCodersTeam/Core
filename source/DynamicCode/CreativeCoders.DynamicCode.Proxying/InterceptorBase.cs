using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.DynamicCode.Proxying;

[PublicAPI]
public class InterceptorBase<T> : IInterceptor
    where T : class
{
    private readonly IDictionary<MethodInfo, PropertyInfo> _propertyGetterInfos;

    private readonly IDictionary<MethodInfo, PropertyInfo> _propertySetterInfos;

    public InterceptorBase()
    {
        _propertyGetterInfos = new ConcurrentDictionary<MethodInfo, PropertyInfo>();
        _propertySetterInfos = new ConcurrentDictionary<MethodInfo, PropertyInfo>();

        InitPropertyInfos();
    }

    private void InitPropertyInfos()
    {
        foreach (var propertyInfo in typeof(T).GetProperties())
        {
            if (propertyInfo.GetMethod != null)
            {
                _propertyGetterInfos.Add(propertyInfo.GetMethod, propertyInfo);
            }

            if (propertyInfo.SetMethod != null)
            {
                _propertySetterInfos.Add(propertyInfo.SetMethod, propertyInfo);
            }
        }
    }

    public void Intercept(IInvocation invocation)
    {
        if (_propertyGetterInfos.TryGetValue(invocation.Method, out var propertyInfoForGet))
        {
            var propertyValue = GetProperty(propertyInfoForGet);
            invocation.ReturnValue = propertyValue;
            return;
        }

        if (_propertySetterInfos.TryGetValue(invocation.Method, out var propertyInfoForSet))
        {
            SetProperty(propertyInfoForSet, invocation.Arguments.First());
            return;
        }

        ExecuteMethod(invocation);
    }

    protected virtual void SetProperty(PropertyInfo propertyInfo, object value) { }

    protected virtual object GetProperty(PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType.GetDefault();
    }

    protected virtual void ExecuteMethod(IInvocation invocation) { }
}
