using System;
using System.Collections;
using System.Collections.Generic;
using CreativeCoders.Core.Collections;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Collections;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetKeyByValue_GetNoneExistentValue_ThrowsException()
    {
        var dict = new Dictionary<string, int>
        {
            {"Test", 1},
            {"Hello", 2}
        };

        Assert.Throws<KeyNotFoundException>(() => dict.GetKeyByValue(3));
    }

    [Fact]
    public void GetKeyByValue_GetExistentValue_ReturnsKey()
    {
        var dict = new Dictionary<string, int>
        {
            {"Test", 1},
            {"Hello", 2}
        };

        var key = dict.GetKeyByValue(2);

        Assert.Equal("Hello", key);
    }

    [Fact]
    public void TryGetKeyByValue_GetNoneExistentValue_ReturnsFalseAndDefaultKey()
    {
        var dict = new Dictionary<string, int>
        {
            {"Test", 1},
            {"Hello", 2}
        };

        var found = dict.TryGetKeyByValue(3, out var key);

        Assert.False(found);
        Assert.Equal(default, key);
    }

    [Fact]
    public void TryGetKeyByValue_GetExistentValue_ReturnsTrueAndKey()
    {
        var dict = new Dictionary<string, int>
        {
            {"Test", 1},
            {"Hello", 2}
        };

        var found = dict.TryGetKeyByValue(1, out var key);

        Assert.True(found);
        Assert.Equal("Test", key);
    }

    [Fact]
    public void ToDictionary_MatchingInputDictionary_ReturnsCorrectDictionary()
    {
        var dictionary = new Dictionary<string, string>
        {
            {"Key0", "Value1"},
            {"Key1", "Value2"}
        } as IDictionary;

        var output = dictionary.ToDictionary<string, string>(true);
            
        Assert.Equal(2, output.Keys.Count);
            
        Assert.Equal("Value1", output["Key0"]);
        Assert.Equal("Value2", output["Key1"]);
    }
        
    [Fact]
    public void ToDictionary_NotMatchingKeyInputDictionary_SkippedNotMatchingKey()
    {
        var dictionary = new Dictionary<object, string>
        {
            {1, "Value1"},
            {"Key1", "Value2"}
        } as IDictionary;

        var output = dictionary.ToDictionary<string, string>(true);
            
        Assert.Equal(1, output.Keys.Count);
            
        Assert.Equal("Value2", output["Key1"]);
    }
        
    [Fact]
    public void ToDictionary_NotMatchingValueInputDictionary_SkippedNotMatchingValue()
    {
        var dictionary = new Dictionary<string, object>
        {
            {"Key0", 1234},
            {"Key1", "Value2"}
        } as IDictionary;

        var output = dictionary.ToDictionary<string, string>(true);
            
        Assert.Equal(1, output.Keys.Count);
            
        Assert.Equal("Value2", output["Key1"]);
    }
        
    [Fact]
    public void ToDictionary_NotMatchingKeyInputDictionary_ThrowsException()
    {
        var dictionary = new Dictionary<object, string>
        {
            {1, "Value1"},
            {"Key1", "Value2"}
        } as IDictionary;

        Assert.Throws<InvalidCastException>(() => dictionary.ToDictionary<string, string>(false));
    }
        
    [Fact]
    public void ToDictionary_NotMatchingValueInputDictionary_ThrowsException()
    {
        var dictionary = new Dictionary<string, object>
        {
            {"Key0", 1234},
            {"Key1", "Value2"}
        } as IDictionary;

        Assert.Throws<InvalidCastException>(() => dictionary.ToDictionary<string, string>(false));
    }
        
    [Fact]
    public void ToDictionary_WithValueSelectorMatchingInputDictionary_ReturnsCorrectDictionary()
    {
        var dictionary = new Dictionary<string, int>
        {
            {"Key0", 1234},
            {"Key1", 5678}
        } as IDictionary;

        var output = dictionary.ToDictionary<string, string>(o => o.ToString(), true);
            
        Assert.Equal(2, output.Keys.Count);
            
        Assert.Equal("1234", output["Key0"]);
        Assert.Equal("5678", output["Key1"]);
    }
        
    [Fact]
    public void ToDictionary_WithValueSelectorNotMatchingKeyInputDictionary_SkippedNotMatchingKey()
    {
        var dictionary = new Dictionary<object, int>
        {
            {1, 1234},
            {"Key1", 5678}
        } as IDictionary;

        var output = dictionary.ToDictionary<string, string>(o => o.ToString(), true);
            
        Assert.Equal(1, output.Keys.Count);
            
        Assert.Equal("5678", output["Key1"]);
    }
        
    [Fact]
    public void ToDictionary_WithValueSelectorNotMatchingKeyInputDictionary_ThrowsException()
    {
        var dictionary = new Dictionary<object, int>
        {
            {1, 1234},
            {"Key1", 5678}
        } as IDictionary;

        Assert.Throws<InvalidCastException>(() => dictionary.ToDictionary<string, string>(o => o.ToString(), false));
    }
}