﻿using System;
using System.Linq.Expressions;

namespace QRest.Core.Contracts
{
    public interface IAssemblerContext
    {
        (Expression Left, Expression Right) Convert(Expression left, Expression right);
        bool TryConvert(Expression expression, Type target, out Expression result);
    }
}
