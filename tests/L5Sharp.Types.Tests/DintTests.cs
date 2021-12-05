﻿using System;
using AutoFixture;
using FluentAssertions;
using L5Sharp.Enums;
using NUnit.Framework;

namespace L5Sharp.Types.Tests
{
    [TestFixture]
    public class DintTests
    {
        private int _random;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _random = fixture.Create<int>();
        }
        
        [Test]
        public void New_Default_ShouldNotBeNull()
        {
            var type = new Dint();

            type.Should().NotBeNull();
        }
        
        [Test]
        public void New_Default_ShouldHaveExpectedDefaults()
        {
            var type = new Dint();

            type.Should().NotBeNull();
            type.Name.Should().Be(nameof(Dint).ToUpper());
            type.Class.Should().Be(DataTypeClass.Atomic);
            type.Family.Should().Be(DataTypeFamily.None);
            type.Description.Should().Be("RSLogix representation of a System.Int32");
            type.Value.Should().Be(0);
        }
        
        [Test]
        public void New_ValueOverload_ShouldHaveExpectedValue()
        {
            var type = new Dint(_random);
            
            type.Value.Should().Be(_random);
        }

        [Test]
        public void Update_Null_ShouldReturnExpected()
        {
            var type = new Dint();

            FluentActions.Invoking(() => type.Update(null!)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Update_ValidInt_ShouldReturnExpected()
        {
            var type = new Dint();

            var updated = type.Update(_random);

            updated.Value.Should().Be(_random);
        }
        
        [Test]
        public void Update_ValidDint_ShouldReturnExpected()
        {
            var type = new Dint();

            var updated = type.Update(new Dint(_random));

            updated.Value.Should().Be(_random);
        }
        
        [Test]
        public void Update_ValidObjectValue_ShouldReturnExpected()
        {
            var type = new Dint();

            var updated = type.Update((object) _random);

            updated.Value.Should().Be(_random);
        }

        [Test]
        public void Update_ValidStringValue_ShouldReturnExpected()
        {
            var type = new Dint();

            var updated = type.Update(_random.ToString());

            updated.Value.Should().Be(_random);
        }
        
        [Test]
        public void Update_InvalidString_ShouldThrowArgumentException()
        {
            var fixture = new Fixture();
            var value = fixture.Create<string>();
            var type = new Dint(_random);

            FluentActions.Invoking(() => type.Update(value)).Should().Throw<ArgumentException>()
                .WithMessage($"Could not parse string '{value}' to {typeof(Dint)}");
        }
        
        [Test]
        public void SetValue_InvalidType_ShouldThrowArgumentException()
        {
            var fixture = new Fixture();
            var value = fixture.Create<float>();
            var type = new Dint();

            FluentActions.Invoking(() => type.Update(value)).Should().Throw<ArgumentException>()
                .WithMessage($"Value type '{value.GetType()}' is not a valid for {typeof(Dint)}");
        }

        [Test]
        public void ImplicitOperator_Dint_ShouldBeTrue()
        {
            Dint value = _random;

            value.Value.Should().Be(_random);
        }

        [Test]
        public void ImplicitOperator_int_ShouldBeTrue()
        {
            int value = new Dint(_random);

            value.Should().Be(_random);
        }

        [Test]
        public void TypeEquals_AreEqual_ShouldBeTrue()
        {
            var first = new Dint();
            var second = new Dint();

            var result = first.Equals(second);

            result.Should().BeTrue();
        }
        
        [Test]
        public void TypeEquals_AreSame_ShouldBeTrue()
        {
            var first = new Dint();

            var result = first.Equals(first);

            result.Should().BeTrue();
        }
        
        
        [Test]
        public void TypeEquals_Null_ShouldBeFalse()
        {
            var first = new Dint();

            var result = first.Equals(null);

            result.Should().BeFalse();
        }
        
        [Test]
        public void ObjectEquals_AreEqual_ShouldBeTrue()
        {
            var first = new Dint();
            var second = new Dint();

            var result = first.Equals((object)second);

            result.Should().BeTrue();
        }
        
        [Test]
        public void ObjectEquals_AreSame_ShouldBeTrue()
        {
            var first = new Dint();

            var result = first.Equals((object)first);

            result.Should().BeTrue();
        }
        
        [Test]
        public void ObjectEquals_Null_ShouldBeFalse()
        {
            var first = new Dint();

            var result = first.Equals((object)null);

            result.Should().BeFalse();
        }

        [Test]
        public void OperatorEquals_AreEqual_ShouldBeTrue()
        {
            var first = new Dint();
            var second = new Dint();

            var result = first == second;

            result.Should().BeTrue();
        }
        
        [Test]
        public void OperatorNotEquals_AreEqual_ShouldBeFalse()
        {
            var first = new Dint();
            var second = new Dint();

            var result = first != second;

            result.Should().BeFalse();
        }

        [Test]
        public void GetHashCode_WhenCalled_ShouldNotBeZero()
        {
            var type = new Dint();

            var hash = type.GetHashCode();

            hash.Should().NotBe(0);
        }

        [Test]
        public void CompareTo_ValidOther_ShouldBeZero()
        {
            var first = new Dint();
            var second = new Dint();

            var compare = first.CompareTo(second);

            compare.Should().Be(0);
        }
    }
}