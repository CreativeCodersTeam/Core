namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Represents an individual property link between two objects and handles property change
///     propagation based on the configured <see cref="LinkDirection"/>.
/// </summary>
public class PropertyLinkItem
{
    private readonly PropertyValueCopier _propertyValueCopier;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyLinkItem"/> class.
    /// </summary>
    /// <param name="propertyLinkInfo">The property link information describing the link configuration.</param>
    public PropertyLinkItem(PropertyLinkInfo propertyLinkInfo)
    {
        Info = propertyLinkInfo;
        _propertyValueCopier = new PropertyValueCopier();
    }

    /// <summary>
    ///     Handles a property change notification and copies the value in the appropriate direction
    ///     if the changed property matches the link configuration.
    /// </summary>
    /// <param name="changedInstance">The object instance that raised the property change.</param>
    /// <param name="changedPropertyName">The name of the property that changed.</param>
    public void HandleChange(object changedInstance, string changedPropertyName)
    {
        // ReSharper disable once SwitchStatementMissingSomeCases
        switch (Info.Direction)
        {
            case LinkDirection.TwoWay
                when (Info.Source == changedInstance && Info.SourceProperty.Name == changedPropertyName)
                     || (Info.Target == changedInstance && Info.TargetProperty.Name == changedPropertyName):
            {
                if (Info.Source == changedInstance)
                {
                    CopyToTarget();
                }
                else
                {
                    CopyToSource();
                }

                return;
            }
            case LinkDirection.OneWayFromTarget
                when Info.Target == changedInstance && Info.TargetProperty.Name == changedPropertyName:
                CopyToSource();
                return;
            case LinkDirection.OneWayToTarget
                when Info.Source == changedInstance && Info.SourceProperty.Name == changedPropertyName:
                CopyToTarget();
                break;
        }
    }

    /// <summary>
    ///     Initializes the link by copying the current property value in the appropriate direction
    ///     based on the link configuration.
    /// </summary>
    public void Init()
    {
        if (Info.Direction == LinkDirection.OneWayFromTarget || Info.InitWithTargetValue)
        {
            CopyToSource();
        }
        else
        {
            CopyToTarget();
        }
    }

    private void CopyToTarget()
    {
        _propertyValueCopier.CopyPropertyValue(Info.Source, Info.SourceProperty, Info.Target,
            Info.TargetProperty,
            false, Info);
    }

    private void CopyToSource()
    {
        _propertyValueCopier.CopyPropertyValue(Info.Target, Info.TargetProperty, Info.Source,
            Info.SourceProperty,
            true, Info);
    }

    /// <summary>
    ///     Gets the property link information describing the link configuration.
    /// </summary>
    public PropertyLinkInfo Info { get; }
}
