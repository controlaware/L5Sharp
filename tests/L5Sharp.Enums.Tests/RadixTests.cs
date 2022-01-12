﻿using FluentAssertions;
using L5Sharp.Core;
using L5Sharp.Types;
using NUnit.Framework;

namespace L5Sharp.Enums.Tests
{
    [TestFixture]
    public class RadixTests
    {
        [Test]
        public void Default_UserDefined_ShouldBeNullRadix()
        {
            var userDefined = new UserDefined("Test", "This is a test");

            var radix = Radix.Default(userDefined);

            radix.Should().Be(Radix.Null);
        }

        [Test]
        public void Default_Real_ShouldBeDecimalRadix()
        {
            var type = new Real();

            var radix = Radix.Default(type);

            radix.Should().Be(Radix.Float);
        }

        [Test]
        public void Default_Bool_ShouldBeDecimalRadix()
        {
            var type = new Bool();

            var radix = Radix.Default(type);

            radix.Should().Be(Radix.Decimal);
        }

        [Test]
        public void Default_Sint_ShouldBeDecimalRadix()
        {
            var type = new Sint();

            var radix = Radix.Default(type);

            radix.Should().Be(Radix.Decimal);
        }


        [Test]
        public void Default_Int_ShouldBeDecimalRadix()
        {
            var type = new Int();

            var radix = Radix.Default(type);

            radix.Should().Be(Radix.Decimal);
        }


        [Test]
        public void Default_Dint_ShouldBeDecimalRadix()
        {
            var type = new Dint();

            var radix = Radix.Default(type);

            radix.Should().Be(Radix.Decimal);
        }

        [Test]
        public void Default_Lint_ShouldBeDecimalRadix()
        {
            var type = new Lint();

            var radix = Radix.Default(type);

            radix.Should().Be(Radix.Decimal);
        }

        [Test]
        public void ParseValue_ValidBinary_ShouldBeExpectedValue()
        {
            const string value = "2#0000_0101";

            var parsed = Radix.ParseValue(value);

            parsed.Value.Should().Be(5);
        }

        [Test]
        public void ParseValue_ValidOctal_ShouldBeExpectedValue()
        {
            const string value = "8#014";

            var parsed = Radix.ParseValue(value);

            parsed.Value.Should().Be(0x0C);
        }

        [Test]
        public void SupportsType_NonAtomic_ShouldBeFalse()
        {
            var result = Radix.Decimal.SupportsType(typeof(object));

            result.Should().BeFalse();
        }


        [Test]
        public void SupportsType_IDataType_ShouldBeFalse()
        {
            var result = Radix.Decimal.SupportsType(typeof(IDataType));

            result.Should().BeFalse();
        }

        [Test]
        public void SupportsType_BoolAsIDataType_ShouldBeTrue()
        {
            var type = (IDataType)new Bool();

            var supported = Radix.Decimal.SupportsType(type);

            supported.Should().BeTrue();
        }

        [Test]
        public void SupportsType_BoolArrayAsIDataType_ShouldBeTrue()
        {
            var type = new ArrayType<IDataType>(new Dimensions(5), new Bool());

            var supported = Radix.Decimal.SupportsType(type);

            supported.Should().BeTrue();
        }
    }
}