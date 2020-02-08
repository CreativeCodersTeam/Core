using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
    [PublicAPI]
    public static class Ensure
    {
        [ContractAnnotation("halt <= value: null")]
        public static void IsNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        [ContractAnnotation("halt <= value: null")]
        public static void IsNotNull<T>(object value, Func<T> createExceptionFunc) where T: Exception
        {
            if (value == null)
            {
                throw createExceptionFunc();
            }
        }

        [ContractAnnotation("halt <= value: null")]
        public static void IsNotNullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Must not be null or empty", paramName);
            }
        }

        [ContractAnnotation("halt <= value: null")]
        public static void IsNotNullOrEmpty<T>(IEnumerable<T> value, string paramName)
        {
            if (value == null || !value.Any())
            {
                throw new ArgumentException("Enumeration must not be null or empty", paramName);
            }
        }

        [ContractAnnotation("halt <= value: null")]
        public static void IsNotNullOrWhitespace(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Must not be null or whitespace", paramName);
            }
        }

        public static void FileExists(string fileName)
        {
            if (!FileSys.File.Exists(fileName))
            {
                throw new FileNotFoundException("File not found", fileName);
            }
        }

        public static void DirectoryExists(string dirName)
        {
            if (!FileSys.Directory.Exists(dirName))
            {
                throw new DirectoryNotFoundException($"Directory not found: '{dirName}'");
            }
        }

        public static void GuidIsNotEmpty(Guid value, string paramName)
        {
            if (value.Equals(Guid.Empty))
            {
                throw new ArgumentException("Guid cannot be empty", paramName);
            }
        }

        [ContractAnnotation("halt <= assertion: false")]
        public static void That(bool assertion, string paramName)
        {
            That(assertion, paramName, "Assertion failed");
        }

        [ContractAnnotation("halt <= assertion: false")]
        public static void That(bool assertion, string paramName, string message)
        {
            if (!assertion)
            {
                throw new ArgumentException(message, paramName);                
            }
        }
        
        [ContractAnnotation("halt <= assertion: false")]
        public static void ThatRange(bool assertion, string paramName)
        {
            ThatRange(assertion, paramName, "Assertion failed");
        }

        [ContractAnnotation("halt <= assertion: false")]
        public static void ThatRange(bool assertion, string paramName, string message)
        {
            if (!assertion)
            {
                throw new ArgumentOutOfRangeException(message, paramName);                
            }
        }

        public static void IndexIsInRange(int index, int startIndex, int endIndex, string paramName)
        {
            if (index < startIndex || index > endIndex)
            {
                throw new ArgumentOutOfRangeException(paramName, index, $"Index '{index}' is out of range '{startIndex}-{endIndex}'");
            }
        }
        
        public static void IndexIsInRange(int index, int collectionLength, string paramName)
        {
            if (index < 0 || index >= collectionLength)
            {
                throw new ArgumentOutOfRangeException(paramName, index, $"Index '{index}' is out of range '{0}-{collectionLength - 1}'");
            }
        }
    }
}