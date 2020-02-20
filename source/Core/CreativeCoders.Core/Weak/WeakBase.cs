using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakBase<T> : IDisposable
        where T : class
    {
        private WeakReference _targetReference;
        
        private WeakReference<T> _dataReference;
        
        private object _target;

        public WeakBase(object target, T data, KeepTargetAliveMode keepTargetAliveMode)
        {
            Ensure.IsNotNull(data, nameof(data));

            if (target != null)
            {
                _targetReference = new WeakReference(target);

                KeepTargetAlive = GetKeepAlive(keepTargetAliveMode, target);
                    
                if (KeepTargetAlive)
                {
                    _target = target;
                }
            }
            
            _dataReference = new WeakReference<T>(data);
        }

        private static bool GetKeepAlive(KeepTargetAliveMode keepTargetAliveMode, object target)
        {
            return keepTargetAliveMode switch
            {
                KeepTargetAliveMode.KeepAlive => true,
                KeepTargetAliveMode.AutoGuess => (target?.GetType().Name.Contains("<>") == true),
                _ => false
            };
        }

        public T GetData()
        {
            if (_targetReference == null)
            {
                return _dataReference?.GetTarget();
            }
            
            var target = _targetReference.Target;

            return target == null 
                ? default
                : _dataReference?.GetTarget();
        }
        
        public bool KeepTargetAlive { get; }

        public bool GetIsAlive()
        {
            return (_targetReference == null || _targetReference.IsAlive) && _dataReference?.GetIsAlive() == true;
        }

        public object GetTarget()
        {
            return _target ?? _targetReference?.Target;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _target = null;
            _targetReference = null;
            _dataReference = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}