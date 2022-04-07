using System;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure;

[PublicAPI]
public interface IViewLocator
{
    object CreateViewForViewModel(object viewModel);

    object CreateViewForViewModel(object viewModel, object context);

    Type GetViewType(object viewModel);

    Type GetViewType(object viewModel, object context);
}
