using JetBrains.Annotations;

namespace CreativeCoders.Core;

[PublicAPI]
public delegate void EventHandlerEx<in TSender, in TEventArg>(TSender sender, TEventArg e);

[PublicAPI]
public delegate void EventHandlerEx<in TSender>(TSender sender);