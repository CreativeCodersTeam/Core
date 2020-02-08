using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = propertyExpression.GetMemberName();
            OnPropertyChanged(propertyName);
        }

        protected void Set<T>(ref T value, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, newValue))
            {
                return;
            }
            OnPropertyChanging(propertyName);
            value = newValue;
            OnPropertyChanged(propertyName);
        }

        protected void Set<T>(ref T value, T newValue, Expression<Func<T>> propertyExpression)
        {
            var propertyName = propertyExpression.GetMemberName();
            Set(ref value, newValue, propertyName);
        }

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
    }
}