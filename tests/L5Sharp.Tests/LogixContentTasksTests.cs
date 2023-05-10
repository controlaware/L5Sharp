﻿using FluentAssertions;

namespace L5Sharp.Tests;

[TestFixture]
public class LogixContentTasksTests
{
    [Test]
    public void Collection_WhenCalled_ShouldNotBeEmpty()
    {
        var content = LogixContent.Load(Known.Test);

        var dataTypes = content.Tasks().ToList();

        dataTypes.Should().NotBeEmpty();
    }

    [Test]
    public void Add_ValidComponent_ShouldAddComponent()
    {
        var content = LogixContent.Load(Known.Test);
        
        var component = new L5Sharp.Components.LogixTask
        {
            Name = "Test",
            Description = "This is a test",
        };

        content.Tasks().Add(component);

        var result = content.Tasks().Find("Test");

        result.Should().NotBeNull();
    }
    
    [Test]
    public void Add_ToEmptyFile_ShouldAddComponent()
    {
        var content = LogixContent.Load(Known.Empty);
        
        var component = new L5Sharp.Components.LogixTask
        {
            Name = "Test",
            Description = "This is a test",
        };

        content.Tasks().Add(component);

        var result = content.Tasks().Find("Test");

        result.Should().NotBeNull();
    }
    
    [Test]
    public void Add_AlreadyExists_ShouldThrowException()
    {
        var content = LogixContent.Load(Known.Test);
        
        var component = new L5Sharp.Components.LogixTask
        {
            Name = Known.Task,
            Description = "This is a test",
        };

        FluentActions.Invoking(() => content.Tasks().Add(component)).Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Contains_Existing_ShouldBeTrue()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Contains(Known.Task);

        result.Should().BeTrue();
    }
    
    [Test]
    public void Contains_NonExisting_ShouldBeFalse()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Contains("Fake");

        result.Should().BeFalse();
    }

    [Test]
    public void Find_Existing_ShouldBeExpected()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Find(Known.Task);

        result.Should().NotBeNull();
        result.Name.Should().Be(Known.Task);
    }
    
    [Test]
    public void Find_NonExisting_ShouldBeNull()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Find("Fake");

        result.Should().BeNull();
    }

    [Test]
    public void Get_Existing_ShouldBeExpected()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Get(Known.Task);

        result.Should().NotBeNull();
        result.Name.Should().Be(Known.Task);
    }
    
    [Test]
    public void Get_NonExisting_ShouldThrowException()
    {
        var content = LogixContent.Load(Known.Test);

        FluentActions.Invoking(() => content.Tasks().Get("Fake")) .Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Remove_Existing_ShouldBeTrue()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Remove(Known.Task);

        result.Should().BeTrue();

        var component = content.Tasks().Find(Known.Task);
        component.Should().BeNull();
    }
    
    [Test]
    public void Remove_NonExisting_ShouldBeFalse()
    {
        var content = LogixContent.Load(Known.Test);

        var result = content.Tasks().Remove("Fake");

        result.Should().BeFalse();
    }

    [Test]
    public void Upsert_NonExisting_ShouldHaveExpected()
    {
        var content = LogixContent.Load(Known.Test);
        
        var component = new L5Sharp.Components.LogixTask
        {
            Name = "New",
            Description = "This is a test",
        };

        content.Tasks().Update(component);

        var result = content.Tasks().Find("New");
        
        result.Should().NotBeNull();
        result.Name.Should().Be("New");
        result.Description.Should().Be("This is a test");
    }

    [Test]
    public void Upsert_Existing_ShouldHaveExpected()
    {
        var content = LogixContent.Load(Known.Test);
        
        var component = new L5Sharp.Components.LogixTask
        {
            Name = Known.Task,
            Description = "This is a test"
        };

        content.Tasks().Update(component);

        var result = content.Tasks().Find(Known.Task);
        
        result.Should().NotBeNull();
        result.Name.Should().Be(Known.Task);
        result.Description.Should().Be("This is a test");
    }
}