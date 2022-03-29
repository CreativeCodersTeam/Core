using System.Reflection;

namespace CreativeCoders.Localization;

/// <summary>   Localization options used by the <see cref="DefaultStringLocalizer"/>. </summary>
public class ExtendedLocalizationOptions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Localization.ExtendedLocalizationOptions
    ///     class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public ExtendedLocalizationOptions()
    {
        Assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        ResourceName = "Localization";
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the assembly which contains the default localization resource. </summary>
    ///
    /// <value> The assembly. </value>
    ///-------------------------------------------------------------------------------------------------
    public Assembly Assembly { get; set; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the name of the default localization resource. </summary>
    ///
    /// <value> The name of the resource. </value>
    ///-------------------------------------------------------------------------------------------------
    public string ResourceName { get; set; }
}