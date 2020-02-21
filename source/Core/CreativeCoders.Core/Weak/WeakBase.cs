using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakBase<T> : IDisposable
        where T : class
    {
        private WeakReference _ownerReference;
        
        private WeakReference<T> _dataReference;
        
        private object _owner;

        public WeakBase(object owner, T data, KeepOwnerAliveMode keepOwnerAliveMode)
        {
            Ensure.IsNotNull(data, nameof(data));

            if (owner != null)
            {
                _ownerReference = new WeakReference(owner);

                KeepOwnerAlive = GetKeepAlive(keepOwnerAliveMode, owner);
                    
                if (KeepOwnerAlive)
                {
                    _owner = owner;
                }
            }
            
            _dataReference = new WeakReference<T>(data);
        }

        private static bool GetKeepAlive(KeepOwnerAliveMode keepOwnerAliveMode, object target)
        {
            return keepOwnerAliveMode switch
            {
                KeepOwnerAliveMode.KeepAlive => true,
                KeepOwnerAliveMode.AutoGuess => (target?.GetType().Name.Contains("<>") == true),
                _ => false
            };
        }

        public T GetData()
        {
            if (_ownerReference == null)
            {
                return _dataReference?.GetTarget();
            }
            
            var target = _ownerReference.Target;

            return target == null 
                ? default
                : _dataReference?.GetTarget();
        }
        
        public bool KeepOwnerAlive { get; }

        public bool IsAlive()
        {
            return (_ownerReference == null || _ownerReference.IsAlive) && _dataReference?.GetIsAlive() == true;
        }

        public object GetTarget()
        {
            return _owner ?? _ownerReference?.Target;
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _owner = null;
            _ownerReference = null;
            _dataReference = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}