using System;
using System.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    /// <summary>   Interface for CLI action runtime builder. </summary>
    public interface ICliActionRuntimeBuilder
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Use middleware for the execution pipeline. </summary>
        ///
        /// <typeparam name="TMiddleware">  Type of the middleware. </typeparam>
        /// <param name="arguments">    Arguments passed to the middleware constructor. </param>
        ///
        /// <returns>   This ICliActionRuntimeBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        ICliActionRuntimeBuilder UseMiddleware<TMiddleware>(params object[]? arguments)
            where TMiddleware : CliActionMiddlewareBase;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all controllers from the calling assembly. </summary>
        ///
        /// <returns>   This ICliActionRuntimeBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        ICliActionRuntimeBuilder AddControllers();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a controller. </summary>
        ///
        /// <param name="controllerType">   Type of the controller. </param>
        ///
        /// <returns>   This ICliActionRuntimeBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        ICliActionRuntimeBuilder AddController(Type controllerType);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a controller. </summary>
        ///
        /// <typeparam name="TController">  Type of the controller. </typeparam>
        ///
        /// <returns>   This ICliActionRuntimeBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        ICliActionRuntimeBuilder AddController<TController>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all controllers from the assembly. </summary>
        ///
        /// <param name="assembly"> The assembly. </param>
        ///
        /// <returns>   This ICliActionRuntimeBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        ICliActionRuntimeBuilder AddControllers(Assembly assembly);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the runtime. </summary>
        ///
        /// <returns>   An ICliActionRuntime. </returns>
        ///-------------------------------------------------------------------------------------------------
        ICliActionRuntime Build();
    }
}
