using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Config.Base;
using CreativeCoders.Core;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.Config
{
    public class Configuration : IConfiguration
    {
        private readonly IList<SourceRegistration> _sourceRegistrations;

        private readonly IList<Action<IConfigurationSource, Exception, SourceExceptionHandleResult>> _onSourceExceptions;

        public Configuration()
        {
            _sourceRegistrations = new ConcurrentList<SourceRegistration>();
            _onSourceExceptions = new ConcurrentList<Action<IConfigurationSource, Exception, SourceExceptionHandleResult>>();
        }

        public IConfiguration AddSource<T>(IConfigurationSource<T> source)
            where T : class
        {
            Ensure.IsNotNull(source, nameof(source));

            _sourceRegistrations.Add(new SourceRegistration(typeof(T), source));
            return this;
        }

        public IConfiguration AddSources<T>(IEnumerable<IConfigurationSource<T>> sources)
            where T : class
        {
            Ensure.IsNotNull(sources, nameof(sources));

            sources.ForEach(source => AddSource(source));
            return this;
        }

        public T GetItem<T>()
            where T : class
        {
            var registration = _sourceRegistrations.FirstOrDefault(reg => typeof(T).IsAssignableFrom(reg.DataType));
            return InvokeWithExceptionHandling(registration?.Source, source => source.GetSettingObject() as T);
        }

        public IEnumerable<T> GetItems<T>()
            where T : class
        {
            var registrations = _sourceRegistrations.Where(reg => typeof(T).IsAssignableFrom(reg.DataType));
            return registrations
                .Select(reg => InvokeWithExceptionHandling(reg.Source, source => source.GetSettingObject() as T))
                .WhereNotNull()
                .ToArray();
        }

        private T InvokeWithExceptionHandling<T>(IConfigurationSource source, Func<IConfigurationSource, T> function)
            where T : class
        {
            try
            {
                return function(source);
            }
            catch (Exception ex)
            {
                var handleResult = new SourceExceptionHandleResult();

                _onSourceExceptions.ForEach(onSourceException => onSourceException(source, ex, handleResult));

                if (!handleResult.IsHandled)
                {
                    throw;
                }

                return source.GetDefaultSettingObject() as T;
            }
        }

        public void OnSourceException(Action<IConfigurationSource, Exception, SourceExceptionHandleResult> onSourceException)
        {
            Ensure.IsNotNull(onSourceException, nameof(onSourceException));

            _onSourceExceptions.Add(onSourceException);
        }
    }
}