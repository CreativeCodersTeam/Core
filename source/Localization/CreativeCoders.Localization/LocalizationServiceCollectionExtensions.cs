using System;
using System.Reflection;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CreativeCoders.Localization
{
    /// <summary>   Extension methods for setting up extended localization services in an <see cref="IServiceCollection" />. </summary>
    [PublicAPI]
    public static class LocalizationServiceCollectionExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds services required for default application localization. </summary>
        ///
        /// <param name="services">         The services to act on. </param>
        /// <param name="resourcesPath">    Pathname of the resources file in the assembly. </param>
        /// <param name="setupOptions">     Options for controlling the setup of the default
        ///                                 implementation <see cref="IStringLocalizer"/>. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SetupLocalization(this IServiceCollection services, string resourcesPath,
            Action<ExtendedLocalizationOptions> setupOptions)
        {
            Ensure.IsNotNullOrWhitespace(resourcesPath, nameof(resourcesPath));
            Ensure.IsNotNull(setupOptions, nameof(setupOptions));

            services.Configure<ExtendedLocalizationOptions>(options =>
            {
                options.Assembly = Assembly.GetCallingAssembly();
                
                setupOptions(options);
            });

            services.AddLocalization(opts => opts.ResourcesPath = resourcesPath);

            services.AddSingleton<IStringLocalizer, DefaultStringLocalizer>();

            services.AddSingleton(typeof(IExtendedStringLocalizer<>),
                typeof(DefaultExtendedStringLocalizer<>));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds services required for default application localization. </summary>
        ///
        /// <param name="services">         The services to act on. </param>
        /// <param name="resourcesPath">    Pathname of the resources file in the assembly. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SetupLocalization(this IServiceCollection services, string resourcesPath)
        {
            services.SetupLocalization(resourcesPath, _ => { });
        }
    }
}
