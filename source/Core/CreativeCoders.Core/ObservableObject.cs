using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Provides a base class for objects that support property change notification.
/// </summary>
[PublicAPI]
public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for the specified property expression.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="propertyExpression">The expression identifying the property.</param>
    protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
        var propertyName = propertyExpression.GetMemberName();
        OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// Sets the property value and raises change notification events if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="value">A reference to the backing field.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void Set<T>(ref T? value, T? newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(value, newValue))
        {
            return;
        }

        OnPropertyChanging(propertyName);
        value = newValue;
        OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// Sets the property value and raises change notification events if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="value">A reference to the backing field.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="propertyExpression">The expression identifying the property.</param>
    protected void Set<T>(ref T? value, T? newValue, Expression<Func<T>> propertyExpression)
    {
        var propertyName = propertyExpression.GetMemberName();
        Set(ref value, newValue, propertyName);
    }

    /// <summary>
    /// Occurs when a property value is changing.
    /// </summary>
    public event PropertyChangingEventHandler? PropertyChanging;

    /// <summary>
    /// Raises the <see cref="PropertyChanging"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that is changing.</param>
    protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
}
