﻿using QRest.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QRest.Core.Operations.Query
{
    public class SkipOperation : QueryOperationBase
    {
        public override string Key { get; } = "skip";        

        protected override Expression CreateExpression(ParameterExpression root, Expression context, Type element, Type collection, IReadOnlyList<Expression> arguments, IAssemblerContext assembler)
        {
            if (arguments.Count != 1)
                throw new ExpressionCreationException();

            if (!assembler.TryConvert(arguments[0], typeof(int), out var argument))
                throw new ExpressionCreationException($"Cannot cast {arguments[0].Type} to Int32");

            var exp = Expression.Call(collection, nameof(Queryable.Skip), new Type[] { element }, context, argument);

            return assembler.SetName(exp, "data");
        }
    }
}
