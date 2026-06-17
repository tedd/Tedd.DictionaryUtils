using System;
using System.Collections.Generic;
using Xunit;

namespace Tedd.DictionaryUtils.Tests
{
    public class AddExtensionsTest
    {
        [Theory]
        [InlineData("key1", "value1")]
        [InlineData("key2", "value2")]
        public void GetOrAdd_NewKey_AddsAndReturnsValue(string key, string expectedValue)
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();

            // Act
            var result = dictionary.GetOrAdd(key, () => expectedValue);

            // Assert
            Assert.Equal(expectedValue, result);
            Assert.True(dictionary.ContainsKey(key));
            Assert.Equal(expectedValue, dictionary[key]);
        }

        [Theory]
        [InlineData("key1", "existingValue", "newValue")]
        public void GetOrAdd_ExistingKey_ReturnsExistingValue(string key, string existingValue, string newValueFactoryResult)
        {
            // Arrange
            var dictionary = new Dictionary<string, string>
            {
                { key, existingValue }
            };

            // Act
            var result = dictionary.GetOrAdd(key, () => newValueFactoryResult);

            // Assert
            Assert.Equal(existingValue, result);
            Assert.Equal(existingValue, dictionary[key]);
        }

        [Fact]
        public void GetOrAdd_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            string key = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>("key", () => dictionary.GetOrAdd(key, () => "value"));
        }

        [Fact]
        public void GetOrAdd_NullValueFactory_ThrowsArgumentNullException()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            string key = "key";

            // Act & Assert
            Assert.Throws<ArgumentNullException>("valueFactory", () => dictionary.GetOrAdd(key, null!));
        }
    }
}
