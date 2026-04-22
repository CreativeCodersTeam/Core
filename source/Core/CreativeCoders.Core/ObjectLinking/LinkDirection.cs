namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Specifies the direction in which property values are synchronized between linked objects.
/// </summary>
public enum LinkDirection
{
    /// <summary>
    ///     Synchronizes property values from the source object to the target object only.
    /// </summary>
    OneWayToTarget,

    /// <summary>
    ///     Synchronizes property values from the target object to the source object only.
    /// </summary>
    OneWayFromTarget,

    /// <summary>
    ///     Synchronizes property values in both directions between source and target objects.
    /// </summary>
    TwoWay
}
