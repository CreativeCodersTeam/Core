using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Config.Base;

[PublicAPI]
public interface IConfiguration
{
    IConfiguration AddSource<T>(IConfigurationSource<T> source) where T : class;

    IConfiguration AddSources<T>(IEnumerable<IConfigurationSource<T>> sources) where T : class;

    T GetItem<T>()
        where T : class;

    IEnumerable<T> GetItems<T>()
        where T : class;

    void OnSourceException(Action<IConfigurationSource, Exception, SourceExceptionHandleResult> onSourceException);
}