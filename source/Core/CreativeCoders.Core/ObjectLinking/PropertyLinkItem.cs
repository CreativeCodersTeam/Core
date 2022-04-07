namespace CreativeCoders.Core.ObjectLinking;

public class PropertyLinkItem
{
    private readonly PropertyValueCopier _propertyValueCopier;

    public PropertyLinkItem(PropertyLinkInfo propertyLinkInfo)
    {
        Info = propertyLinkInfo;
        _propertyValueCopier = new PropertyValueCopier();
    }

    public void HandleChange(object changedInstance, string changedPropertyName)
    {
        // ReSharper disable once SwitchStatementMissingSomeCases
        switch (Info.Direction)
        {
            case LinkDirection.TwoWay
                when Info.Source == changedInstance && Info.SourceProperty.Name == changedPropertyName
                     || Info.Target == changedInstance && Info.TargetProperty.Name == changedPropertyName:
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

    public PropertyLinkInfo Info { get; }
}
