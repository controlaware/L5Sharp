﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using L5Sharp.Abstractions;
using L5Sharp.Atomics;
using L5Sharp.Core;
using L5Sharp.Enums;
using L5Sharp.Factories;
using L5Sharp.Predefined;
using NUnit.Framework;
using String = L5Sharp.Predefined.String;

namespace L5Sharp.Extensions.Tests
{
    [TestFixture]
    public class DataTypeExtensionsTests
    {
        [Test]
        public void StructureEquals_SameAtomic_ShouldBeTrue()
        {
            var t1 = new Bool();
            var t2 = new Bool();

            var result = t1.StructureEquals(t2);

            result.Should().BeTrue();
        }
        
        [Test]
        public void StructureEquals_SameAtomicDifferentValues_ShouldBeTrue()
        {
            var t1 = new Int();
            var t2 = new Int(32);

            var result = t1.StructureEquals(t2);

            result.Should().BeTrue();
        }

        [Test]
        public void StructureEquals_DifferentAtomics_ShouldBeFalse()
        {
            var t1 = new Int();
            var t2 = new Dint();
            
            var result = t1.StructureEquals(t2);

            result.Should().BeFalse();
        }

        [Test]
        public void StructureEquals_SameComplex_ShouldBeTure()
        {
            var t1 = new Timer();
            var t2 = new Timer();
            
            var result = t1.StructureEquals(t2);

            result.Should().BeTrue();
        }
        
        [Test]
        public void StructureEquals_DifferentComplex_ShouldBeFalse()
        {
            var t1 = new Timer();
            var t2 = new Counter();
            
            var result = t1.StructureEquals(t2);

            result.Should().BeFalse();
        }
        
        [Test]
        public void StructureEquals_SameArray_ShouldBeTure()
        {
            var t1 = new ArrayType<Dint>(10);
            var t2 = new ArrayType<Dint>(10);
            
            var result = t1.StructureEquals(t2);

            result.Should().BeTrue();
        }
        
        [Test]
        public void StructureEquals_DifferentArrayLength_ShouldBeFalse()
        {
            var t1 = new ArrayType<Dint>(10);
            var t2 = new ArrayType<Dint>(11);
            
            var result = t1.StructureEquals(t2);

            result.Should().BeFalse();
        }
        
        [Test]
        public void StructureEquals_DifferentArrayType_ShouldBeFalse()
        {
            var t1 = new ArrayType<Dint>(5);
            var t2 = new ArrayType<Int>(5);
            
            var result = t1.StructureEquals(t2);

            result.Should().BeFalse();
        }
        
        [Test]
        public void StructureEquals_SameCustomType_ShouldBeTure()
        {
            var t1 = new MyNestedType();
            var t2 = new MyNestedType();
            
            var result = t1.StructureEquals(t2);

            result.Should().BeTrue();
        }
        
        [Test]
        public void StructureEquals_DifferentCustomType_ShouldBeFalse()
        {
            var t1 = new MyNestedType();
            var t2 = new MyOtherNestedType();
            
            var result = t1.StructureEquals(t2);

            result.Should().BeFalse();
        }

        [Test]
        public void StructureEquals_SameUserDefined_ShouldBeTrue()
        {
            var t1 = new UserDefined("Test", "This is a test", new List<IMember<IDataType>>
            {
                Member.Create<Bool>("Member01"),
                Member.Create<Int>("Member02"),
                Member.Create<Real>("Member03"),
            });
            
            var t2 = new UserDefined("Test", "This is a test", new List<IMember<IDataType>>
            {
                Member.Create<Bool>("Member01"),
                Member.Create<Int>("Member02"),
                Member.Create<Real>("Member03"),
            });

            t1.StructureEquals(t2).Should().BeTrue();
        }
        
        [Test]
        public void GetMembers_Atomic_ShouldBeEmpty()
        {
            var type = new Int();

            var members = type.GetMembers();

            members.Should().BeEmpty();
        }

        [Test]
        public void GetMembers_Complex_ShouldNotBeEmpty()
        {
            var type = new Timer();

            var members = type.GetMembers();

            members.Should().NotBeEmpty();
        }

        [Test]
        public void GetMembers_ArrayOfAtomic_ShouldNotBeEmpty()
        {
            var type = new ArrayType<Bool>(5);

            var members = type.GetMembers();

            members.Should().NotBeEmpty();
        }

        [Test]
        public void GetMembers_ArrayOfComplex_ShouldNotBeEmpty()
        {
            var type = new ArrayType<String>(5);

            var members = type.GetMembers();

            members.Should().NotBeEmpty();
        }

        [Test]
        public void GetMember_Atomic_ShouldBeNull()
        {
            var type = new Bool();

            var member = type.GetMember("Child");

            member.Should().BeNull();
        }

        [Test]
        public void GetMember_Complex_ShouldNotBeNull()
        {
            var type = new Timer();

            var member = type.GetMember("PRE");

            member.Should().NotBeNull();
        }

        [Test]
        public void GetMember_ArrayOfAtomic_ShouldNotBeNull()
        {
            var type = new ArrayType<Bool>(5);

            var member = type.GetMember("[2]");

            member.Should().NotBeNull();
        }

        [Test]
        public void GetTagNames_Atomic_ShouldBeEmpty()
        {
            var type = new Int();

            var members = type.GetTagNames();

            members.Should().BeEmpty();
        }

        [Test]
        public void GetTagNames_Complex_ShouldContainExpectedTagNames()
        {
            var type = new Timer();

            var members = type.GetTagNames().ToList();

            members.Should().Contain("PRE");
            members.Should().Contain("ACC");
            members.Should().Contain("EN");
            members.Should().Contain("DN");
            members.Should().Contain("TT");
        }

        [Test]
        public void GetTagNames_ArrayOfAtomic_ShouldContainExpectedTagNames()
        {
            var type = new ArrayType<Bool>(5);

            var members = type.GetTagNames().ToList();

            members.Should().Contain("[0]");
            members.Should().Contain("[1]");
            members.Should().Contain("[2]");
            members.Should().Contain("[3]");
            members.Should().Contain("[4]");
        }

        [Test]
        public void GetTagNames_ArrayOfComplex_ShouldContainExpectedTagNames()
        {
            var type = new ArrayType<String>(5);

            var members = type.GetTagNames().ToList();

            members.Should().Contain("[0]");
            members.Should().Contain("[0].LEN");
            members.Should().Contain("[0].DATA");
            members.Should().Contain("[1]");
            members.Should().Contain("[1].LEN");
            members.Should().Contain("[1].DATA");
            members.Should().Contain("[2]");
            members.Should().Contain("[2].LEN");
            members.Should().Contain("[2].DATA");
            members.Should().Contain("[3]");
            members.Should().Contain("[3].LEN");
            members.Should().Contain("[3].DATA");
            members.Should().Contain("[4]");
            members.Should().Contain("[4].LEN");
            members.Should().Contain("[4].DATA");
        }

        [Test]
        public void GetDependentTypes_Atomic_ShouldBeEmpty()
        {
            var type = new Int();

            var types = type.GetDependentTypes();

            types.Should().BeEmpty();
        }

        [Test]
        public void GetDependentTypes_Complex_ShouldHaveExpectedTypes()
        {
            var type = new Timer();

            var types = type.GetDependentTypes().ToList();

            types.Should().HaveCount(2);
            types.Should().Contain(t => t.Name == "BOOL");
            types.Should().Contain(t => t.Name == "DINT");
        }

        [Test]
        public void GetDependentTypes_Array_ShouldHaveExpectedTypes()
        {
            var type = new ArrayType<Timer>(3);

            var types = type.GetDependentTypes().ToList();

            types.Should().HaveCount(3);
            types.Should().Contain(t => t.Name == "TIMER");
            types.Should().Contain(t => t.Name == "BOOL");
            types.Should().Contain(t => t.Name == "DINT");
        }

        [Test]
        public void ContainsMember_Null_ShouldThrowArgumentNullException()
        {
            var type = new Timer();

            FluentActions.Invoking(() => type.HasMember(null!)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ContainsMember_Atomic_ShouldBeFalse()
        {
            var type = new Bool();

            var result = type.HasMember("MemberName");

            result.Should().BeFalse();
        }

        [Test]
        public void ContainsMember_ComplexAndExists_ShouldBeTrue()
        {
            var type = new Timer();

            var result = type.HasMember("PRE");

            result.Should().BeTrue();
        }

        [Test]
        public void ContainsMember_ComplexAndDoesNotExist_ShouldBeFalse()
        {
            var type = new Timer();

            var result = type.HasMember("PRESET");

            result.Should().BeFalse();
        }

        [Test]
        public void ContainsMember_ArrayAndExists_ShouldBeTrue()
        {
            var type = new ArrayType<Bool>(5);

            var result = type.HasMember("[3]");

            result.Should().BeTrue();
        }

        [Test]
        public void ContainsMember_ArrayAndDoesNotExist_ShouldBeFalse()
        {
            var type = new ArrayType<Bool>(5);

            var result = type.HasMember("[5]");


            result.Should().BeFalse();
        }
        
        [Test]
        public void GetMembersTo_Null_ShouldThrowArgumentException()
        {
            var type = new Bool();

            FluentActions.Invoking(() => type.GetMembersTo(null!)).Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetMembersTo_Atomic_ShouldBeEmpty()
        {
            var type = new Bool();

            var members = type.GetMembersTo("MemberName");

            members.Should().BeEmpty();
        }

        [Test]
        public void GetMembersTo_ComplexAndInvalidPath_ShouldBeEmpty()
        {
            var type = new Timer();

            var members = type.GetMembersTo("PRESET");

            members.Should().BeEmpty();
        }

        [Test]
        public void GetMembersTo_ComplexAndValidPath_ShouldHaveExpectedCount()
        {
            var type = new Timer();

            var members = type.GetMembersTo("PRE").ToList();

            members.Should().HaveCount(1);
            members.Should().Contain(m => m.Name == "PRE");
        }

        [Test]
        public void GetMembersTo_MyNestedType_ShouldHaveExpectedMembers()
        {
            var type = new MyNestedType();

            var members = type.GetMembersTo("Tmr.PRE").ToList();

            members.Should().HaveCount(2);
            members.Should().Contain(m => m.Name == "Tmr");
            members.Should().Contain(m => m.Name == "PRE");
        }

        [Test]
        public void GetMembersTo_MyNestedArrayMember_ShouldHaveExpectedMembers()
        {
            var type = new MyNestedType();

            var members = type.GetMembersTo("Counters[1].ACC").ToList();

            members.Should().HaveCount(3);
            members.Should().Contain(m => m.Name == "Counters");
            members.Should().Contain(m => m.Name == "[1]");
            members.Should().Contain(m => m.Name == "ACC");
        }

        [Test]
        public void SetData_IncompatibleAtomics_WillThrowArgumentException()
        {
            var target = new Int(20);
            var source = new Dint(100);

            FluentActions.Invoking(() => target.SetData(source)).Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void SetData_DifferentButConvertableAtomics_WillSetData()
        {
            var target = new Dint(100);
            var source = new Int(30);

            target.SetData(source);
            
            target.Value.Should().Be(30);
        }
        
        [Test]
        public void SetData_SourceSmallerThanTarget_TargetShouldHaveExpectedData()
        {
            var target = new ArrayType<Int>(5);
            var types = new List<Int> { 10, 20, 30 };
            
            var source = types.ToArrayType();

            target.SetData(source);

            target[0].DataType.Value.Should().Be(10);
            target[1].DataType.Value.Should().Be(20);
            target[2].DataType.Value.Should().Be(30);
            target[3].DataType.Value.Should().Be(0);
            target[4].DataType.Value.Should().Be(0);
        }
        
        [Test]
        public void SetData_TargetSmallerThanSource_TargetShouldHaveExpectedData()
        {
            var target = new ArrayType<Int>(3);
            var types = new List<Int> { 10, 20, 30, 40, 50 };
            
            var source = types.ToArrayType();

            target.SetData(source);

            target[0].DataType.Value.Should().Be(10);
            target[1].DataType.Value.Should().Be(20);
            target[2].DataType.Value.Should().Be(30);
        }
        
        [Test]
        public void SetData_SameSizedArrays_TargetShouldHaveExpectedData()
        {
            var target = new ArrayType<Int>(5);
            var types = new List<Int> { 10, 20, 30, 40, 50 };
            
            var source = types.ToArrayType();

            target.SetData(source);

            target[0].DataType.Value.Should().Be(10);
            target[1].DataType.Value.Should().Be(20);
            target[2].DataType.Value.Should().Be(30);
            target[3].DataType.Value.Should().Be(40);
            target[4].DataType.Value.Should().Be(50);
        }

        [Test]
        public void SetData_DifferentComplexTypes_ShouldHaveExpectedData()
        {
            var target = new MyNestedType();
            var source = new MyOtherNestedType();
            source.Indy.DataType.SetValue(true);
            source.Str.DataType.SetValue("This is a test value");

            target.SetData(source);

            target.Indy.DataType.Value.Should().BeTrue();
            target.Str.DataType.Value.Should().Be("This is a test value");
            target.Tmr.DataType.PRE.DataType.Value.Should().Be(0);
            target.Counters.DataType[0].DataType.PRE.DataType.Value.Should().Be(0);
        }

        [Test]
        public void SetData_SameComplexTypes_ShouldHaveExpectedData()
        {
            var target = new MyNestedType();
            var source = GetTypeWithData();

            target.SetData(source);

            target.Indy.DataType.Value.Should().BeTrue();
            target.Str.DataType.Value.Should().Be("This is a test value");
            target.Tmr.DataType.PRE.DataType.Value.Should().Be(5000);
            target.Tmr.DataType.ACC.DataType.Value.Should().Be(0);
            target.Tmr.DataType.DN.DataType.Value.Should().Be(true);
            target.Tmr.DataType.TT.DataType.Value.Should().Be(false);
            target.Tmr.DataType.EN.DataType.Value.Should().Be(false);
            target.Counters.DataType[0].DataType.PRE.DataType.Value.Should().Be(0);
            target.Counters.DataType[0].DataType.OV.DataType.Value.Should().Be(false);
            target.Counters.DataType[1].DataType.PRE.DataType.Value.Should().Be(1000);
            target.Counters.DataType[1].DataType.OV.DataType.Value.Should().Be(true);
            target.Counters.DataType[2].DataType.PRE.DataType.Value.Should().Be(2000);
            target.Counters.DataType[2].DataType.OV.DataType.Value.Should().Be(true);
            target.Counters.DataType[3].DataType.PRE.DataType.Value.Should().Be(3000);
            target.Counters.DataType[3].DataType.OV.DataType.Value.Should().Be(true);
            target.Counters.DataType[4].DataType.PRE.DataType.Value.Should().Be(0);
            target.Counters.DataType[4].DataType.OV.DataType.Value.Should().Be(false);
        }

        private static IDataType GetTypeWithData()
        {
            var source = new MyNestedType();
            source.Indy.DataType.SetValue(true);
            source.Tmr.DataType.PRE.DataType.SetValue(5000);
            source.Tmr.DataType.DN.DataType.SetValue(true);
            source.Str.DataType.SetValue("This is a test value");
            source.Counters.DataType[1].DataType.OV.DataType.SetValue(true);
            source.Counters.DataType[2].DataType.OV.DataType.SetValue(true);
            source.Counters.DataType[3].DataType.OV.DataType.SetValue(true);
            source.Counters.DataType[1].DataType.PRE.DataType.SetValue(1000);
            source.Counters.DataType[2].DataType.PRE.DataType.SetValue(2000);
            source.Counters.DataType[3].DataType.PRE.DataType.SetValue(3000);
            return source;
        }
    }
    
    public class MyNestedType : ComplexTypeBase
    {
        public MyNestedType() : base(nameof(MyNestedType))
        {
        }

        public IMember<Bool> Indy = Member.Create<Bool>(nameof(Indy));
        public IMember<String> Str = Member.Create<String>(nameof(Str));
        public IMember<Timer> Tmr = Member.Create<Timer>(nameof(Tmr));
        public IMember<IArrayType<Counter>> Counters = Member.Create<Counter>(nameof(Counters), 5);
        
        public override DataTypeClass Class => DataTypeClass.User;
        protected override IDataType New() => new MyNestedType();
    }
    
    public class MyOtherNestedType : ComplexTypeBase
    {
        public MyOtherNestedType() : base(nameof(MyOtherNestedType))
        {
        }

        public IMember<Bool> Indy = Member.Create<Bool>(nameof(Indy));
        public IMember<String> Str = Member.Create<String>(nameof(Str));

        public override DataTypeClass Class => DataTypeClass.User;
        protected override IDataType New() => new MyNestedType();
    }
}