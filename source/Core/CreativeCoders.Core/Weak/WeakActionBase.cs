using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public abstract class WeakActionBase : IDisposable
    {
        private WeakReference _reference;

        private readonly MethodInfo _methodInfo;

        // ReSharper disable once NotAccessedField.Local
        private object _actionTarget;

        private WeakReference _actionTargetReference;

        protected WeakActionBase(object target, MethodInfo methodInfo, object actionTarget) : this(target, methodInfo,
            actionTarget, KeepActionTargetAliveMode.NotKeepAlive) { }

        protected WeakActionBase(object target, MethodInfo methodInfo, object actionTarget, KeepActionTargetAliveMode keepActionTargetAliveMode)
        {
            Ensure.IsNotNull(methodInfo, "methodInfo");
            Ensure.That(methodInfo.IsStatic || target != null, "target",
                "Method must be static or target must not be null.");
            
            if (target != null)
            {
                _reference = new WeakReference(target);
            }
            _methodInfo = methodInfo;

            KeepActionTargetAlive = GetKeepAlive(keepActionTargetAliveMode, actionTarget);

            if (KeepActionTargetAlive)
            {
                _actionTarget = actionTarget;
            }

            if (actionTarget != null)
            {
                _actionTargetReference = new WeakReference(actionTarget);
            }
        }

        private static bool GetKeepAlive(KeepActionTargetAliveMode keepActionTargetAliveMode, object actionTarget)
        {
            return keepActionTargetAliveMode switch
            {
                KeepActionTargetAliveMode.KeepAlive => true,
                KeepActionTargetAliveMode.AutoGuess => (actionTarget?.GetType().Name.Contains("<>") == true),
                _ => false
            };
        }

        public bool IsAlive => GetIsAlive();

        private bool GetIsAlive()
        {
            if (_actionTargetReference != null)
            {
                return _actionTargetReference.IsAlive && (_reference == null || _reference.IsAlive);
            }

            return _methodInfo.IsStatic || _reference?.IsAlive == true;
        }

        public T CreateAction<T>() where T : class
        {
            var target = Target;
            var actionTarget = ActionTarget;
            return !IsAlive ? null : CreateDelegate<T>(target, actionTarget);
        }

        // ReSharper disable once UnusedParameter.Local
        private T CreateDelegate<T>(object target, object actionTarget) where T : class
        {
            if (actionTarget == null || _methodInfo.IsStatic)
            {
                return Delegate.CreateDelegate(typeof (T), _methodInfo) as T;
            }

            return Delegate.CreateDelegate(typeof(T), actionTarget, _methodInfo) as T;
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            
            _actionTarget = null;
            _actionTargetReference = null;
            _reference = null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public object Target => _reference?.Target;

        public object ActionTarget => _actionTargetReference?.Target;

        public bool KeepActionTargetAlive { get; set; }
    }
}