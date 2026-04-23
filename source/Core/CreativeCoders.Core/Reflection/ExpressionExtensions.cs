using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Provides extension methods for working with <see cref="Expression"/> trees.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Gets the member name from a member access expression.
    /// </summary>
    /// <typeparam name="T">The type of the member.</typeparam>
    /// <param name="memberExpression">The expression that accesses a member.</param>
    /// <returns>The name of the member referenced by the expression.</returns>
    public static string GetMemberName<T>(this Expression<Func<T>> memberExpression)
    {
        return InternalGetMemberName(memberExpression);
    }

    /// <summary>
    /// Gets the member name from a member access expression on a specific type.
    /// </summary>
    /// <typeparam name="T">The type that contains the member.</typeparam>
    /// <typeparam name="TProperty">The type of the member.</typeparam>
    /// <param name="memberExpression">The expression that accesses a member on <typeparamref name="T"/>.</param>
    /// <returns>The name of the member referenced by the expression.</returns>
    public static string GetMemberName<T, TProperty>(this Expression<Func<T, TProperty>> memberExpression)
    {
        return InternalGetMemberName(memberExpression);
    }

    /// <summary>
    /// Determines whether the expression refers to a property declared directly on type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type expected to declare the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="propertyExpression">The expression that accesses a property.</param>
    /// <returns>
    /// <see langword="true"/> if the expression refers to a property declared on <typeparamref name="T"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsPropertyOf<T, TProperty>(this Expression<Func<T, TProperty>> propertyExpression)
        where T : class
    {
        var memberInfo = GetMemberInfo(propertyExpression);
        return memberInfo?.MemberType.HasFlag(MemberTypes.Property) == true &&
               memberInfo.DeclaringType == typeof(T);
    }

    private static MemberInfo GetMemberInfo<T>(Expression<T> memberExpression)
    {
        return (memberExpression.Body as MemberExpression)?.Member;
    }

    private static string InternalGetMemberName<T>(Expression<T> memberExpression)
    {
        Ensure.IsNotNull(memberExpression);
        var body = memberExpression.Body as MemberExpression;

        Ensure.IsNotNull(body);

        return body.Member.Name;
    }
}
