using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CreativeCoders.Core.Reflection;

public static class ExpressionExtensions
{
    public static string GetMemberName<T>(this Expression<Func<T>> memberExpression)
    {
        return InternalGetMemberName(memberExpression);
    }

    public static string GetMemberName<T, TProperty>(this Expression<Func<T, TProperty>> memberExpression)
    {
        return InternalGetMemberName(memberExpression);
    }

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
        Ensure.IsNotNull(memberExpression, nameof(memberExpression));
        var body = memberExpression.Body as MemberExpression;

        Ensure.IsNotNull(body, "body");

        return body.Member.Name;
    }
}
