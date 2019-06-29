using System;
using System.Linq.Expressions;

namespace EzTrace.Jaegers
{
    public class EnvVarHelper
    {
        public static TModel SetEnvItem<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, EnvironmentVariableTarget target)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                var name = memberExpression.Member.Name;
                var compile = expression.Compile();
                var value = compile.Invoke(model).ToString();

                Environment.SetEnvironmentVariable(name, value, target);
                Console.WriteLine(@"{0}:        {1}     => {2}", name, Environment.GetEnvironmentVariable(name, target), target);
            }

            return model;
        }

        public static TModel DeleteEnvItem<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, EnvironmentVariableTarget target)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                var name = memberExpression.Member.Name;
                var theOne = Environment.GetEnvironmentVariable(name, target);
                if (theOne != null)
                {
                    Environment.SetEnvironmentVariable(name, null, target);
                }
                Console.WriteLine(@"{0}:        {1}     => {2}", name, Environment.GetEnvironmentVariable(name, target), target);
            }

            return model;
        }
    }
}