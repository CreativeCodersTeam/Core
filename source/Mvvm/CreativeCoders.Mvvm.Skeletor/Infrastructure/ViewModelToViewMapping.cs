using System;
using CreativeCoders.Core;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure
{
    public class ViewModelToViewMapping
    {
        public ViewModelToViewMapping(Type viewModelType, Type viewType, string name)
        {
            Ensure.IsNotNull(viewModelType, nameof(viewModelType));
            Ensure.IsNotNull(viewType, nameof(viewType));

            ViewModelType = viewModelType;
            ViewType = viewType;
            Name = name;
        }

        public Type ViewModelType { get; }

        public Type ViewType { get; }

        public string Name { get; }
    }
}