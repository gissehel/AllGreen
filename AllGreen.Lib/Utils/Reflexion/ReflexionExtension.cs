using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AllGreen.Lib.Utils.Reflexion
{
    public static class ReflexionExtension
    {
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }

        public static string GetMethodName<X>(this Expression<X> expression)
        {
            var body = (MethodCallExpression)expression.Body;
            return body.Method.Name;
        }

        public static IEnumerable<KeyValuePair<object, Type>> GetMethodArguments<X>(this Expression<X> expression)
        {
            var body = (MethodCallExpression)expression.Body;
            foreach (var argument in body.Arguments)
            {
                if (argument is ConstantExpression constant)
                {
                    yield return new KeyValuePair<object, Type>(constant.Value, constant.Value.GetType());
                }
            }
        }

        public static string GetPropertyName<X>(this Expression<X> expression)
        {
            if (expression.Body is UnaryExpression body)
            {
                if (body.Operand is MemberExpression operand)
                {
                    return operand.Member.Name;
                }
            }
            if (expression.Body is MemberExpression bodyMemberExpression)
            {
                return bodyMemberExpression.Member.Name;
            }
            return null;
        }

        public static PropertyInfo GetPropertyInfo<X>(this X instance, string name)
        {
            return instance.GetType().GetProperty(name);
        }

        public static Func<X, Y> GetProperyGetter<X, Y>(string name)
        {
            var typeX = typeof(X);
            var parameterX = Expression.Parameter(typeX, typeX.Name);
            var property = Expression.Property(parameterX, name);
            var conversionToY = Expression.Convert(property, typeof(Y));
            var lambda = Expression.Lambda<Func<X, Y>>(conversionToY, parameterX);
            var lambdaCompiled = lambda.Compile();

            return lambdaCompiled as Func<X, Y>;
        }

        public static Action<X, Y> GetProperySetter<X, Y>(string name)
        {
            var typeX = typeof(X);
            var parameterX = Expression.Parameter(typeX, typeX.Name);
            var property = Expression.Property(parameterX, name);
            var parameterY = Expression.Parameter(typeof(Y), "value");
            // .Net 4+ only // var lambda = Expression.Lambda<Action<X, Y>>(Expression.Assign(property, parameterY), parameterX, parameterY);
            var lambda = Expression.Lambda<Action<X, Y>>(Expression.Call(parameterX, typeX.GetProperty(name).GetSetMethod(), parameterY), parameterX, parameterY);
            var lambdaCompiled = lambda.Compile();

            return lambdaCompiled as Action<X, Y>;
        }

        public static Func<X, object> GetProperyGetter<X>(string name)
        {
            return GetProperyGetter<X, object>(name);
        }

        public static Action<X, object> GetProperySetter<X>(string name)
        {
            var typeX = typeof(X);
            var propertyInfo = typeX.GetProperty(name);
            var parameterX = Expression.Parameter(typeX, typeX.Name);
            var property = Expression.Property(parameterX, propertyInfo);
            var parameterY = Expression.Parameter(typeof(object), "value");
            var conversionToY = Expression.Convert(parameterY, propertyInfo.PropertyType);
            // .Net 4+ only //var lambda = Expression.Lambda<Action<X, object>>(Expression.Assign(property, conversionToY), parameterX, parameterY);
            // var lambda = Expression.Lambda<Action<X, object>>(Expression.Assign(property, conversionToY), parameterX, parameterY);
            var lambda = Expression.Lambda<Action<X, object>>(Expression.Call(parameterX, typeX.GetProperty(name).GetSetMethod(), conversionToY), parameterX, parameterY);
            var lambdaCompiled = lambda.Compile();

            return lambdaCompiled as Action<X, object>;
        }

        public static Type GetProperyType<X>(string name)
        {
            var typeX = typeof(X);
            var propertyInfo = typeX.GetProperty(name);
            return propertyInfo.PropertyType;
        }

        public static object GetPropertyValue(this object o, string propertyName)
        {
            PropertyInfo propertyInfo = o.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return null;
            }
            return propertyInfo.GetValue(o, null);
        }
    }
}