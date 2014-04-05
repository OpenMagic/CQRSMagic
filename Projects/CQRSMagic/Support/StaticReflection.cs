using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSMagic.Support
{
    // todo: Move to OpenMagic and tests for the all public methods.

    /// <summary>
    ///     Original source http://joelabrahamsson.com/getting-property-and-method-names-using-static-reflection-in-c/
    /// </summary>
    public static class StaticReflection
    {
        public static MemberInfo GetMemberInfo<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMemberInfo(expression);
        }

        public static MethodInfo GetMethodInfo<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMethodInfo(GetMemberInfo(expression));
        }

        public static MemberInfo GetMemberInfo<T>(Expression<Func<T, object>> expression)
        {
            return GetMemberInfo(expression.Body);
        }

        public static MethodInfo GetMethodInfo<T>(Expression<Func<T, object>> expression)
        {
            return GetMethodInfo(GetMemberInfo(expression.Body));
        }

        public static MemberInfo GetMemberInfo<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMemberInfo(expression);
        }

        public static MethodInfo GetMethodInfo<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMethodInfo(GetMemberInfo(expression));
        }

        public static MemberInfo GetMemberInfo<T>(Expression<Action<T>> expression)
        {
            return GetMemberInfo(expression.Body);
        }

        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return GetMethodInfo(GetMemberInfo(expression.Body));
        }

        private static MethodInfo GetMethodInfo(MemberInfo memberInfo)
        {
            var methodInfo = memberInfo as MethodInfo;

            if (methodInfo == null)
            {
                throw new ArgumentException("Value must be an expression for a method.");
            }

            return methodInfo;
        }

        private static MemberInfo GetMemberInfo(Expression expression)
        {
            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression) expression;
                return memberExpression.Member;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression) expression;
                return methodCallExpression.Method;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression) expression;
                return GetMemberInfo(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static MemberInfo GetMemberInfo(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression) unaryExpression.Operand;
                return methodExpression.Method;
            }

            return ((MemberExpression) unaryExpression.Operand).Member;
        }
    }
}