using System;

namespace CreativeCoders.Scripting.ClassTemplating {
    internal class PropertyInjection<T> : IScriptClassInjection {
        private readonly string _propertyName;

        private readonly Func<T> _getInjectionDataFunc;

        public PropertyInjection(string propertyName, Func<T> getInjectionDataFunc)
        {
            _propertyName = propertyName;
            _getInjectionDataFunc = getInjectionDataFunc;
        }

        public void Inject(object scriptObject)
        {
            var property = scriptObject.GetType().GetProperty(_propertyName);
            property?.SetValue(scriptObject, _getInjectionDataFunc());
        }
    }
}