using System.Runtime.CompilerServices;
using CreativeCoders.Core.Comparing;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Comparing
{
    public class ComparableObjectTests
    {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1234, 34567)]
        [InlineData(-10, 3)]
        public void CompareTo_Object0LesserThanObject1_ReturnsLesserThanZero(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            var intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.CompareTo(intObject1);

            // Assert
            result
                .Should()
                .BeLessThan(0);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(2, -12)]
        [InlineData(3456, 1234)]
        public void CompareTo_Object0GreaterThanObject1_ReturnsGreaterThanZero(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            var intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.CompareTo(intObject1);

            // Assert
            result
                .Should()
                .BeGreaterThan(0);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(-12, -12)]
        public void CompareTo_Object0EqualToObject1_ReturnsGreaterThanZero(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            var intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.CompareTo(intObject1);

            // Assert
            result
                .Should()
                .Be(0);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(1234)]
        public void GetHashCode_ObjectWithIntProperty_ReturnsIntValue(int value0)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            
            // Act
            var result = intObject0.GetHashCode();

            // Assert
            result
                .Should()
                .Be(value0.GetHashCode());
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1234, 34567)]
        [InlineData(-10, 3)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(2, -12)]
        [InlineData(3456, 1234)]
        public void Equals_Object0NotEqualObject1_ReturnsFalse(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            var intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.Equals(intObject1);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1234, 34567)]
        [InlineData(-10, 3)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(2, -12)]
        [InlineData(3456, 1234)]
        public void Equals_WithObjectArgObject0NotEqualObject1_ReturnsFalse(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            object intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.Equals(intObject1);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(-12, -12)]
        public void Equals_Object0EqualObject1_ReturnsTrue(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            var intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.Equals(intObject1);

            // Assert
            result
                .Should()
                .BeTrue();
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(-12, -12)]
        public void Equals_WithObjectArgObject0EqualObject1_ReturnsTrue(int value0, int value1)
        {
            var intObject0 = new ComparableIntObject { IntValue = value0 };
            object intObject1 = new ComparableIntObject { IntValue = value1 };

            // Act
            var result = intObject0.Equals(intObject1);

            // Assert
            result
                .Should()
                .BeTrue();
        }


        [Theory]
        [InlineData("A", "B")]
        [InlineData("A", "C")]
        [InlineData("XY", "Z")]
        [InlineData("A", "Z")]
        public void CompareTo_StringObject0LesserThanObject1_ReturnsLesserThanZero(string value0, string value1)
        {
            var textObject0 = new ComparableStringObject { TextValue = value0 };
            var textObject1 = new ComparableStringObject { TextValue = value1 };

            // Act
            var result = textObject0.CompareTo(textObject1);

            // Assert
            result
                .Should()
                .BeLessThan(0);
        }

        [Theory]
        [InlineData("B", "A")]
        [InlineData("C", "A")]
        [InlineData("Z", "B")]
        [InlineData("Z", "X")]
        public void CompareTo_StringObject0GreaterThanObject1_ReturnsGreaterThanZero(string value0, string value1)
        {
            var textObject0 = new ComparableStringObject { TextValue = value0 };
            var textObject1 = new ComparableStringObject { TextValue = value1 };

            // Act
            var result = textObject0.CompareTo(textObject1);

            // Assert
            result
                .Should()
                .BeGreaterThan(0);
        }

        [Theory]
        [InlineData("ABC", "ABC")]
        [InlineData("G", "G")]
        [InlineData("Hello", "Hello")]
        public void CompareTo_StringObject0EqualToObject1_ReturnsGreaterThanZero(string value0, string value1)
        {
            var textObject0 = new ComparableStringObject { TextValue = value0 };
            var textObject1 = new ComparableStringObject { TextValue = value1 };

            // Act
            var result = textObject0.CompareTo(textObject1);

            // Assert
            result
                .Should()
                .Be(0);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("ABC")]
        [InlineData("XYZ")]
        [InlineData("Hello")]
        public void GetHashCode_StringObjectWithIntProperty_ReturnsIntValue(string value0)
        {
            var textObject0 = new ComparableStringObject { TextValue = value0 };

            // Act
            var result = textObject0.GetHashCode();

            // Assert
            result
                .Should()
                .Be(RuntimeHelpers.GetHashCode(textObject0));
        }

        [Theory]
        [InlineData("A", "B")]
        [InlineData("A", "C")]
        [InlineData("XY", "Z")]
        [InlineData("A", "Z")]
        public void CompareTo_TextObject0LesserThanObject1_ReturnsLesserThanZero(string value0, string value1)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            var textObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = textObject0.CompareTo(textObject1);

            // Assert
            result
                .Should()
                .BeLessThan(0);
        }

        [Theory]
        [InlineData("B", "A")]
        [InlineData("C", "A")]
        [InlineData("Z", "B")]
        [InlineData("Z", "X")]
        public void CompareTo_TextObject0GreaterThanObject1_ReturnsGreaterThanZero(string value0, string value1)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            var textObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = textObject0.CompareTo(textObject1);

            // Assert
            result
                .Should()
                .BeGreaterThan(0);
        }

        [Theory]
        [InlineData("ABC", "ABC")]
        [InlineData("G", "G")]
        [InlineData("Hello", "Hello")]
        public void CompareTo_TextObject0EqualToObject1_ReturnsGreaterThanZero(string value0, string value1)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            var textObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = textObject0.CompareTo(textObject1);

            // Assert
            result
                .Should()
                .Be(0);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("ABC")]
        [InlineData("XYZ")]
        [InlineData("Hello")]
        public void GetHashCode_TextObjectWithIntProperty_ReturnsIntValue(string value0)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };

            // Act
            var result = textObject0.GetHashCode();

            // Assert
            result
                .Should()
                .Be(value0.GetHashCode());
        }

        [Theory]
        [InlineData("A", "B")]
        [InlineData("A", "C")]
        [InlineData("G", "T")]
        [InlineData("C", "R")]
        [InlineData("B", "A")]
        [InlineData("C", "A")]
        [InlineData("Z", "C")]
        [InlineData("ZZ", "ABC")]
        public void Equals_TextObject0NotEqualObject1_ReturnsFalse(string value0, string value1)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            var textObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = textObject0.Equals(textObject1);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Theory]
        [InlineData("A", "B")]
        [InlineData("A", "C")]
        [InlineData("G", "T")]
        [InlineData("C", "R")]
        [InlineData("B", "A")]
        [InlineData("C", "A")]
        [InlineData("Z", "C")]
        [InlineData("ZZ", "ABC")]
        public void Equals_TextWithObjectArgObject0NotEqualObject1_ReturnsFalse(string value0, string value1)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            object textObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = textObject0.Equals(textObject1);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Theory]
        [InlineData("A", "A")]
        [InlineData("XYZ", "XYZ")]
        [InlineData("Hello", "Hello")]
        public void Equals_TextObject0EqualObject1_ReturnsTrue(string value0, string value1)
        {
            var textObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            var textObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = textObject0.Equals(textObject1);

            // Assert
            result
                .Should()
                .BeTrue();
        }

        [Theory]
        [InlineData("A", "A")]
        [InlineData("XYZ", "XYZ")]
        [InlineData("Hello", "Hello")]
        public void Equals_TextWithObjectArgObject0EqualObject1_ReturnsTrue(string value0, string value1)
        {
            var intObject0 = new ComparableTextPropertyObject { TextValue = value0 };
            object intObject1 = new ComparableTextPropertyObject { TextValue = value1 };

            // Act
            var result = intObject0.Equals(intObject1);

            // Assert
            result
                .Should()
                .BeTrue();
        }
    }

    public class ComparableIntObject : ComparableObject<ComparableIntObject>
    {
        static ComparableIntObject()
        {
            InitComparableObject(x => x.IntValue);
        }

        public int IntValue { get; set; }
    }

    public class ComparableStringObject : ComparableObject<ComparableStringObject>
    {
        public string TextValue { get; set; }

        public override string ToString() => TextValue;
    }

    public class ComparableTextPropertyObject : ComparableObject<ComparableTextPropertyObject>
    {
        static ComparableTextPropertyObject()
        {
            InitComparableObject(x => x.TextValue);
        }

        public string TextValue { get; set; }

        public override string ToString() => TextValue;
    }
}
