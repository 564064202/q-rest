﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QRest.Core.Terms
{
    public class LambdaTerm : MethodTerm
    {      
        public override Expression CreateExpression(Expression prev, ParameterExpression root, Registry registry)
        {
            var execRoot = Expression.Parameter(GetQueryElementType(prev));

            var op = registry.Operations[Method];

            var args = Arguments.Select(a => a.CreateExpression(execRoot, execRoot, registry)).ToList();
            var exp = op.CreateExpression(prev, execRoot, args);

            return Next?.CreateExpression(exp, root, registry) ?? exp;
        }

        public override string ToString()
        {
            return $":{base.ToString().Substring(1)}";
        }

        protected Type GetQueryElementType(Expression query)
        {
            var typeInfo = query.Type.GetTypeInfo();

            if ($"{typeInfo.Namespace}.{typeInfo.Name}" != "System.Linq.IQueryable`1")
            {
                typeInfo = typeInfo.GetInterface("System.Linq.IQueryable`1").GetTypeInfo();
            }

            return typeInfo.GetGenericArguments()[0];
        }
    }
}