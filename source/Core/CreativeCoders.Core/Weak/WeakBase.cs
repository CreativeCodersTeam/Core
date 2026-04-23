using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

/// <summary>
/// Provides a base class for weak reference wrappers that hold a weak reference to both an owner and associated data.
/// </summary>
/// <typeparam name="T">The type of data held by the weak reference wrapper.</typeparam>
[PublicAPI]
public class WeakBase<T> : IDisposable
    where T : class
{
    private WeakReference _ownerReference;

    private WeakReference<T> _dataReference;

    private object _owner;

    [SuppressMessage("csharpsquid", "S4487", Justification = "Needed to hold a strong reference to the data")]
    private T _data;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakBase{T}"/> class.
    /// </summary>
    /// <param name="owner">The object that owns the data.</param>
    /// <param name="data">The data to hold a weak reference to.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakBase(object owner, T data, KeepOwnerAliveMode keepOwnerAliveMode)
    {
        Ensure.IsNotNull(data);

        KeepOwnerAlive = GetKeepAlive(keepOwnerAliveMode, owner);

        if (owner != null)
        {
            _ownerReference = new WeakReference(owner);

            if (KeepOwnerAlive)
            {
                _owner = owner;
            }

            _data = data;
        }

        if (KeepOwnerAlive)
        {
            _data = data;
        }

        _dataReference = new WeakReference<T>(data);
    }

    private static bool GetKeepAlive(KeepOwnerAliveMode keepOwnerAliveMode, object target)
    {
        return keepOwnerAliveMode switch
        {
            KeepOwnerAliveMode.KeepAlive => true,
            KeepOwnerAliveMode.AutoGuess => target?.GetType().Name.Contains("<>") == true,
            _ => false
        };
    }

    /// <summary>
    /// Retrieves the data held by this weak reference wrapper.
    /// </summary>
    /// <returns>The data if both the owner and data are still alive; otherwise, <see langword="null"/>.</returns>
    public T GetData()
    {
        if (_ownerReference == null)
        {
            return _dataReference?.GetTarget();
        }

        var target = _ownerReference.Target;

        return target == null
            ? null
            : _dataReference?.GetTarget();
    }

    /// <summary>
    /// Gets a value indicating whether the owner is kept alive by a strong reference.
    /// </summary>
    public bool KeepOwnerAlive { get; }

    /// <summary>
    /// Determines whether the owner and data references are still alive.
    /// </summary>
    /// <returns><see langword="true"/> if the references are still alive; otherwise, <see langword="false"/>.</returns>
    public bool IsAlive()
    {
        return _ownerReference == null
            ? _dataReference?.GetIsAlive() == true
            : _ownerReference.IsAlive && _dataReference?.GetIsAlive() == true;
    }

    /// <summary>
    /// Retrieves the owner target of this weak reference wrapper.
    /// </summary>
    /// <returns>The owner object if it is still alive; otherwise, <see langword="null"/>.</returns>
    public object GetTarget()
    {
        return _owner ?? _ownerReference?.Target;
    }

    /// <summary>
    /// Releases the managed resources used by this instance.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release managed resources; <see langword="false"/> if called from a finalizer.</param>
    [ExcludeFromCodeCoverage]
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _owner = null;
        _data = null;
        _ownerReference = null;
        _dataReference = null;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
