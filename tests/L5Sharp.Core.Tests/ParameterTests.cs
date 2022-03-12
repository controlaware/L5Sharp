﻿using System;
using AutoFixture;
using FluentAssertions;
using L5Sharp.Atomics;
using L5Sharp.Enums;
using L5Sharp.Exceptions;
using L5Sharp.Predefined;
using NUnit.Framework;

namespace L5Sharp.Core.Tests
{
    [TestFixture]
    public class ParameterTests
    {
        [Test]
        public void New_InvalidName_ShouldThrowComponentNameInvalidException()
        {
            var fixture = new Fixture();
            FluentActions.Invoking(() => new Parameter<Bool>(fixture.Create<string>(), new Bool())).Should()
                .Throw<ComponentNameInvalidException>();
        }

        [Test]
        public void New_NullName_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new Parameter<Bool>(null!, new Bool())).Should()
                .Throw<ArgumentException>();
        }

        [Test]
        public void New_EmptyName_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new Parameter<Bool>(string.Empty, new Bool())).Should()
                .Throw<ArgumentException>();
        }

        [Test]
        public void New_AtomicType_ShouldNotBeNull()
        {
            var parameter = new Parameter<Bool>("Test", new Bool());

            parameter.Should().NotBeNull();
        }

        [Test]
        public void New_PredefinedType_ShouldNoteBeNull()
        {
            var parameter = new Parameter<Timer>("Test", new Timer());

            parameter.Should().NotBeNull();
        }

        [Test]
        public void New_UserType_ShouldNotBeNull()
        {
            var dataType = new UserDefined("Test");
            var parameter = new Parameter<IDataType>("Test", dataType);

            parameter.Should().NotBeNull();
        }

        [Test]
        public void New_Default_ShouldBeExpectedProperties()
        {
            var parameter = new Parameter<Dint>("Test", new Dint());

            parameter.Name.Should().Be("Test");
            parameter.Description.Should().BeEmpty();
            parameter.DataType.Should().Be(new Dint());
            parameter.Dimensions.Should().Be(Dimensions.Empty);
            parameter.Radix.Should().Be(Radix.Decimal);
            parameter.ExternalAccess.Should().Be(ExternalAccess.ReadWrite);
            parameter.Usage.Should().Be(TagUsage.Input);
            parameter.TagType.Should().Be(TagType.Base);
            parameter.Required.Should().BeFalse();
            parameter.Visible.Should().BeFalse();
            parameter.Alias.Should().Be(TagName.Empty);
            parameter.Constant.Should().BeFalse();
            parameter.Default.Should().BeEquivalentTo(new Dint());
        }

        [Test]
        public void New_Overloaded_ShouldBeExpected()
        {
            var parameter = new Parameter<Dint>("Test", new Dint(34), TagUsage.Output, Radix.Hex,
                ExternalAccess.ReadOnly, true, true, "LocalTag", true, "This is a test");

            parameter.Name.Should().Be("Test");
            parameter.Description.Should().Be("This is a test");
            parameter.DataType.Should().BeOfType<Dint>();
            parameter.Dimensions.Should().Be(Dimensions.Empty);
            parameter.Radix.Should().Be(Radix.Hex);
            parameter.ExternalAccess.Should().Be(ExternalAccess.ReadOnly);
            parameter.Usage.Should().Be(TagUsage.Output);
            parameter.TagType.Should().Be(TagType.Alias);
            parameter.Required.Should().BeTrue();
            parameter.Visible.Should().BeTrue();
            parameter.Alias.Should().Be(new TagName("LocalTag"));
            parameter.Constant.Should().BeTrue();
            parameter.Default.Should().BeEquivalentTo(new Dint(34));
        }

        [Test]
        public void New_SimpleArray_ShouldBeExpectedProperties()
        {
            var parameter = new Parameter<IArrayType<Dint>>("Test", new ArrayType<Dint>(10));

            parameter.Name.Should().Be("Test");
            parameter.DataType.Should().BeOfType<ArrayType<Dint>>();
            parameter.Dimensions.Should().BeEquivalentTo(new Dimensions(10));
            parameter.Usage.Should().Be(TagUsage.InOut);
            parameter.Default.Should().BeNull();
        }

        [Test]
        public void New_Predefined_ShouldHaveInOutUsage()
        {
            var parameter = new Parameter<Timer>("Test", new Timer());

            parameter.Usage.Should().Be(TagUsage.InOut);
        }

        [Test]
        public void New_Array_ShouldHaveInOutUsage()
        {
            var parameter = new Parameter<IArrayType<Dint>>("Test", new ArrayType<Dint>(10));

            parameter.Usage.Should().Be(TagUsage.InOut);
        }
        
        [Test]
        public void New_Predefined_RequiredShouldBeTrue()
        {
            var parameter = new Parameter<Timer>("Test", new Timer());

            parameter.Required.Should().BeTrue();
        }

        [Test]
        public void New_Array_RequiredShouldBeTrue()
        {
            var parameter = new Parameter<IArrayType<Dint>>("Test", new ArrayType<Dint>(10));

            parameter.Required.Should().BeTrue();
        }
        
        [Test]
        public void New_Predefined_VisibleShouldBeTrue()
        {
            var parameter = new Parameter<Timer>("Test", new Timer());

            parameter.Required.Should().BeTrue();
        }

        [Test]
        public void New_Array_VisibleShouldBeTrue()
        {
            var parameter = new Parameter<IArrayType<Dint>>("Test", new ArrayType<Dint>(10));

            parameter.Required.Should().BeTrue();
        }
    }
}