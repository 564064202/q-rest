﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QRest.AspNetCore.Contracts;
using QRest.Core;
using QRest.Core.Contracts;
using QRest.Core.Exceptions;
using QRest.Core.Operations;
using QRest.Core.Terms;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRest.AspNetCore.Native
{
    public class NativeSemantics : ISemantics
    {
        private static readonly NativeQueryStructure _default = new NativeQueryStructure { Data = new RootTerm(new MethodTerm(new RootOperation()).AsSequence()) };

        private readonly Lazy<Parser<RootTerm>> _parser;

        private readonly NativeSemanticsOptions _opts;

        public NativeSemantics(IOptions<NativeSemanticsOptions> option = null)
        {
            _parser = new Lazy<Parser<RootTerm>>(() => PrepareParser());
            _opts = option?.Value ?? new NativeSemanticsOptions();
        }

        private Parser<RootTerm> PrepareParser()
        {
            var opNames = OperationsMap.GetRegisteredOperationNames()
                .Concat(_opts.CustomOperations.Keys).ToArray();

            return new TermParser(_opts.UseDefferedConstantParsing, opNames, Lookup).Build();
        }

        private IOperation Lookup(string name)
        {
            if (_opts.CustomOperations.TryGetValue(name, out var op))
                return op;

            op = OperationsMap.LookupOperation(name);
            if (op != null) return op;

            throw new UnknownOperationException(name);
        }

        public ActionResult WriteQueryResponse(IQueryStructure query, IReadOnlyDictionary<RootTerm, object> results)
        {
            var result = results[query.Data];
            return new OkObjectResult(result);
        }

        public IQueryStructure ReadQueryStructure(IReadOnlyList<string> values, HttpRequest request)
        {
            if (values.Count == 0)
                return _default;

            var result = _parser.Value.TryParse(values[0]);

            return new NativeQueryStructure { Data = result.Value };
        }
    }
}