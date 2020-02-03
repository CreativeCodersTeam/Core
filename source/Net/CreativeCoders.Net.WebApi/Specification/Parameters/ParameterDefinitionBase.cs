using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Parameters
{
    [PublicAPI]
    public abstract class ParameterDefinitionBase<TValue>
    {
        private readonly Func<object, TValue> _transformValueFunc;

        protected ParameterDefinitionBase(ParameterInfo parameterInfo, Func<object, TValue> transformValueFunc)
        {
            _transformValueFunc = transformValueFunc;
            ParameterInfo = parameterInfo;
        }

        public ParameterInfo ParameterInfo { get; }

        public TValue GetValue(object[] arguments)
        {
            if (ParameterInfo.Position >= arguments.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            var value = arguments[ParameterInfo.Position];

            return _transformValueFunc(value);
        }
    }
}