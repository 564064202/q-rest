﻿using Sprache;
using System;
using Xunit;

namespace QRest.Semantics.MethodChain.Tests
{
    public class ParserTests
    {
        [Fact(DisplayName = "Correct String Constant Is Parsed")]
        public void ValidStringConstant()
        {
            var expected = "Test123-:=/\\+()!@#$%$String\r\n\t!";
            var actual = MethodChainSemantics.StringConstant(new MethodChainSemantics()).TryParse($"`{expected}`");

            Assert.True(actual.Remainder.AtEnd);
            Assert.Empty(actual.Remainder.Memos);

            Assert.True(actual.WasSuccessful);
            Assert.Empty(actual.Expectations);

            Assert.Equal(expected, actual.Value.Value);
        }

        [Fact(DisplayName = "Correct Empty String Constant Is Parsed")]
        public void ValidEmptyStringConstant()
        {
            var expected = "";
            var actual = MethodChainSemantics.StringConstant(new MethodChainSemantics()).TryParse($"`{expected}`");

            Assert.True(actual.WasSuccessful);
            Assert.Empty(actual.Expectations);

            Assert.Equal(expected, actual.Value.Value);
        }

        [Fact(DisplayName = "Incorrect String Constant Shows Error")]
        public void InValidStringConstant1()
        {
            var actual = MethodChainSemantics.StringConstant(new MethodChainSemantics()).TryParse($"`text");

            Assert.NotEmpty(actual.Expectations);
            Assert.False(actual.WasSuccessful);
        }

        [Fact(DisplayName = "Correct Float Constant Is Parsed")]
        public void FloatParseTest()
        {
            var actual = MethodChainSemantics.NumberConstant(new MethodChainSemantics { UseDefferedConstantParsing = DefferedConstantParsing.Disabled }).TryParse($"1.12");

            Assert.Empty(actual.Expectations);
            Assert.True(actual.WasSuccessful);
            Assert.Equal(1.12f, (float)actual.Value.Value);
        }

        [Fact(DisplayName = "Correct Int Constant Is Parsed")]
        public void IntParseTest()
        {
            var actual = MethodChainSemantics.NumberConstant(new MethodChainSemantics { UseDefferedConstantParsing = DefferedConstantParsing.Disabled }).TryParse($"567");

            Assert.Empty(actual.Expectations);
            Assert.True(actual.WasSuccessful);
            Assert.Equal(567, (int)actual.Value.Value);
        }
    }
}
