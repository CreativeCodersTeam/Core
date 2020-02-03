using System;
using System.Collections.Generic;
using CreativeCoders.Core;

namespace CreativeCoders.Scripting.ClassTemplating {
    public class ScriptClassInjections
    {
        private readonly ScriptClassTemplate _template;

        private readonly IList<IScriptClassInjection> _injections;

        public ScriptClassInjections(ScriptClassTemplate template)
        {
            Ensure.IsNotNull(template, nameof(template));

            _template = template;
            _injections = new List<IScriptClassInjection>();
        }

        public void AddProperty<T>(string propertyName, Func<T> getInjectionDataFunc)
        {
            Ensure.IsNotNullOrWhitespace(propertyName, nameof(propertyName));
            Ensure.IsNotNull(getInjectionDataFunc, nameof(getInjectionDataFunc));

            var propertyInjection = new PropertyInjection<T>(propertyName, getInjectionDataFunc);
            _injections.Add(propertyInjection);
            _template.Members.AddProperty(propertyName, typeof(T).Name);
        }

        public void SetupScriptObject(object scriptObject)
        {
            Ensure.IsNotNull(scriptObject, nameof(scriptObject));

            _injections.ForEach(injection => injection.Inject(scriptObject));
        }
    }
}