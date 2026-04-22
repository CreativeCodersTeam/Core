namespace CreativeCoders.Core.Threading;

/// <summary>
///     Specifies the method used to dispatch operations to a synchronization context.
/// </summary>
public enum SynchronizationMethod
{
    /// <summary>
    ///     No synchronization is performed.
    /// </summary>
    None,

    /// <summary>
    ///     Dispatches a synchronous message to the synchronization context.
    /// </summary>
    Send,

    /// <summary>
    ///     Dispatches an asynchronous message to the synchronization context.
    /// </summary>
    Post
}
