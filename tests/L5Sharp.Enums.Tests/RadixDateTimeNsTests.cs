﻿using System;
using FluentAssertions;
using L5Sharp.Atomics;
using NUnit.Framework;

namespace L5Sharp.Enums.Tests
{
    [TestFixture]
    public class RadixDateTimeNsTests
    {
        [Test]
        public void DateTimeNs_WhenCalled_ShouldBeExpected()
        {
            var radix = Radix.DateTimeNs;

            radix.Should().NotBeNull();
            radix.Should().Be(Radix.DateTimeNs);
        }

        [Test]
        public void Format_Null_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => Radix.DateTimeNs.Format(null!)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Format_NonSupportedAtomic_ShouldThrowNotSupportedException()
        {
            FluentActions.Invoking(() => Radix.DateTimeNs.Format(new Real())).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void Format_ValidTimeExample1_ShouldBeExpectedFormat()
        {
            var radix = Radix.DateTimeNs;

            var result = radix.Format(new Lint(1638277952000000));

            result.Should().Be("LDT#1970-01-19-17:04:37.952_000_000(UTC-06:00)");
        }
        
        [Test]
        public void Format_ValidTimeExample2_ShouldBeExpectedFormat()
        {
            var radix = Radix.DateTimeNs;

            var result = radix.Format(new Lint(1641016800000000000));

            result.Should().Be("LDT#2022-01-01-00:00:00.000_000_000(UTC-06:00)");
        }
        
        [Test]
        public void Format_ValidTimeExample3_ShouldBeExpectedFormat()
        {
            var radix = Radix.DateTimeNs;

            var result = radix.Format(new Lint(1641016800000001001));

            //loss of accuracy causes the 1 nano second to be lost
            result.Should().Be("LDT#2022-01-01-00:00:00.000_001_000(UTC-06:00)");
        }
        
        [Test]
        public void Format_ValidTimeExample4_ShouldBeExpectedFormat()
        {
            var radix = Radix.DateTimeNs;

            var result = radix.Format(new Lint(1641016800000001500));

            result.Should().Be("LDT#2022-01-01-00:00:00.000_001_500(UTC-06:00)");
        }

        [Test]
        public void Parse_ValidTimeExample1_ShouldBeExpectedValue()
        {
            var radix = Radix.DateTimeNs;
            
            var result = radix.Parse("LDT#1970-01-19-17:04:37.952_000_000(UTC-06:00)");

            result.Value.Should().Be(1638277952000000);
        }
        
        [Test]
        public void Parse_NoSpecifier_ShouldThrowFormatException()
        {
            FluentActions.Invoking(() => Radix.DateTimeNs.Parse("2021-11-30-07:12:32.000_000_000(UTC-06:00)")).Should()
                .Throw<FormatException>();
        }
        
        [Test]
        public void Parse_ValidTimeExample2_ShouldBeExpectedFormat()
        {
            var radix = Radix.DateTimeNs;

            var result = radix.Parse("LDT#2022-01-01-00:00:00.000_000_000(UTC-06:00)");

            result.Value.Should().Be(1641016800000000000);
        }
    }
}