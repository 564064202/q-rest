﻿using QRest.Core.Contracts;
using QRest.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QRest.Core.Operations.Aggregations
{
    public class SumOperation : QueryOperationBase
    {
        public override string Key { get; } = "sum";        

        protected override Expression CreateExpression(ParameterExpression root, Expression context, Type element, Type collection, IReadOnlyList<Expression> arguments, IAssemblerContext assembler)
        {
            if (arguments.Count != 0)
                throw new TermTreeCompilationException($"Method '{Key}' expects to have no parameters.");

            var exp = Expression.Call(collection, nameof(Queryable.Sum), new Type[] { }, context);

            return assembler.SetName(exp, Key);
        }
    }
}
