﻿using System;
using AutoFixture;
using FluentAssertions;
using L5Sharp.Enums;
using L5Sharp.Exceptions;
using L5Sharp.Types;
using NUnit.Framework;
using String = L5Sharp.Types.String;

namespace L5Sharp.Core.Tests
{
    [TestFixture]
    public class ParameterTests
    {
        [Test]
        public void Create_ValidName_ShouldNotBeNull()
        {
            var parameter = Parameter.Create("Test", new Bool());

            parameter.Should().NotBeNull();
        }

        [Test]
        public void Create_InvalidName_ShouldThrowComponentNameInvalidException()
        {
            var fixture = new Fixture();
            FluentActions.Invoking(() => Parameter.Create(fixture.Create<string>(), new Bool())).Should()
                .Throw<ComponentNameInvalidException>();
        }

        [Test]
        public void Create_NullName_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => Parameter.Create(null, new Bool())).Should()
                .Throw<ArgumentException>();
        }
        
        [Test]
        public void Create_EmptyName_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => Parameter.Create(string.Empty, new Bool())).Should()
                .Throw<ArgumentException>();
        }
        
        [Test]
        public void Create_GenericType_ShouldNotBeNull()
        {
            var parameter = Parameter.Create<Bool>("Test");

            parameter.Should().NotBeNull();
        }

        [Test]
        public void Create_UserType_ShouldNotBeNull()
        {
            var dataType = new DataType("Test");
            var parameter = Parameter.Create("Test", dataType);

            parameter.Should().NotBeNull();
        }

        [Test]
        public void Create_Defaults_ShouldBeExpected()
        {
            var parameter = Parameter.Create<Dint>("Test");

            parameter.Name.ToString().Should().Be("Test");
            parameter.DataType.Should().Be(new Dint());
            parameter.Dimension.Should().Be(Dimensions.Empty);
            parameter.Radix.Should().Be(Radix.Decimal);
            parameter.Required.Should().BeFalse();
            parameter.Visible.Should().BeFalse();
            parameter.Usage.Should().Be(TagUsage.Input);
            parameter.TagType.Should().Be(TagType.Base);
            parameter.Alias.Should().BeNull();
            parameter.Description.Should().BeNull();
            parameter.Constant.Should().BeFalse();
            parameter.Default.Should().Be(new Dint());
        }

        [Test]
        public void SetName_ValidName_ShouldBeExpectedNae()
        {
            var parameter = Parameter.Create<Dint>("Test");
            
            parameter.SetName("Different");

            parameter.Name.ToString().Should().Be("Different");
        }

        [Test]
        public void SetName_Null_ShouldThrowArgumentException()
        {
            var parameter = Parameter.Create<Sint>("Test");
            
            FluentActions.Invoking(() => parameter.SetName(null)).Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void SetName_Empty_ShouldThrowArgumentException()
        {
            var parameter = Parameter.Create<Sint>("Test");
            
            FluentActions.Invoking(() => parameter.SetName(string.Empty)).Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void SetName_Invalid_ShouldThrowComponentNameInvalidException()
        {
            var parameter = Parameter.Create<Sint>("Test");
            
            FluentActions.Invoking(() => parameter.SetName("Not_Valid#01")).Should().Throw<ComponentNameInvalidException>();
        }

        [Test]
        public void SetUsage_ValidUsageForType_ShouldUpdateUsage()
        {
            var parameter = Parameter.Create<Dint>("Test");
            
            parameter.SetUsage(TagUsage.Output);

            parameter.Usage.Should().Be(TagUsage.Output);
        }
        
        [Test]
        public void SetUsage_InvalidUsageForType_ShouldThrowInvalidOperationException()
        {
            var parameter = Parameter.Create<Timer>("Test");
            
            FluentActions.Invoking(() => parameter.SetUsage(TagUsage.Input)).Should().Throw<InvalidOperationException>();
        }
        
        [Test]
        public void SetUsage_Null_ShouldThrowArgumentNullException()
        {
            var parameter = Parameter.Create<Dint>("Test");
            
            FluentActions.Invoking(() => parameter.SetUsage(null)).Should().Throw<ArgumentNullException>();
        }
        
        
        [Test]
        public void SetUsage_NotInputOutputOrInOut_ShouldThrowArgumentException()
        {
            var parameter = Parameter.Create<Real>("Test");
            
            FluentActions.Invoking(() => parameter.SetUsage(TagUsage.Normal)).Should().Throw<ArgumentException>();
        }

        [Test]
        public void SetDimensions_ValidUsage_ShouldUpdatedDimensionsAndElements()
        {
            var parameter = Parameter.Build<Dint>("Test").WithUsage(TagUsage.InOut).Create();
            
            parameter.SetDimensions(new Dimensions(5));

            parameter.Dimension.Should().Be(new Dimensions(5));
        }
        
        [Test]
        public void SetDimensions_InvalidUsage_ShouldThrowNInvalidOperationException()
        {
            var parameter = Parameter.Build<Dint>("Test").WithUsage(TagUsage.Input).Create();

            FluentActions.Invoking(() => parameter.SetDimensions(new Dimensions(5))).Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public void SetDimensions_Null_ShouldThrowArgumentNullException()
        {
            var parameter = Parameter.Create<Dint>("Test");
            
            FluentActions.Invoking(() => parameter.SetDimensions(null)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void SetExternalAccess_ValidUsageAndAccess_ShouldUpdateToExpectedAccess()
        {
            var parameter = Parameter.Create<Dint>("Test");
            
            parameter.SetExternalAccess(ExternalAccess.ReadOnly);

            parameter.ExternalAccess.Should().Be(ExternalAccess.ReadOnly);
        }
        
        [Test]
        public void SetExternalAccess_InvalidUsage_ShouldUpdateToExpectedAccess()
        {
            var parameter = Parameter.Build<Dint>("Test").WithUsage(TagUsage.InOut).Create();

            FluentActions.Invoking(() => parameter.SetExternalAccess(ExternalAccess.ReadOnly)).Should()
                .Throw<InvalidOperationException>();
        }
        
        
    }
}