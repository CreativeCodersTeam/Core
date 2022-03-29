using System.Threading;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.AspNetCore.Blazor;

public class BlazorObservableCollection<T> : ExtendedObservableCollection<T>
{
    public BlazorObservableCollection() : base(new SynchronizationContext(), SynchronizationMethod.Post, () => new NoLockingMechanism())
    {
            
    }
}