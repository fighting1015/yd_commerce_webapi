using System;
using System.Linq;
using System.Reflection;

namespace Vapps
{
    public static class Requires
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new System.ArgumentNullException(parameterName);
            }
            return value;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static string NotNullOrEmpty(string value, string parameterName)
        {
            Requires.NotNull<string>(value, parameterName);
            Requires.True(value.Length > 0, parameterName, Strings.EmptyStringNotAllowed);
            return value;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void NotNullOrEmpty<T>(System.Collections.Generic.IEnumerable<T> value, string parameterName)
        {
            Requires.NotNull<System.Collections.Generic.IEnumerable<T>>(value, parameterName);
            Requires.True(value.Any<T>(), parameterName, Strings.InvalidArgument);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void NullOrWithNoNullElements<T>(System.Collections.Generic.IEnumerable<T> sequence, string parameterName) where T : class
        {
            Func<T, bool> func = null;
            if (sequence != null)
            {
                if (func == null)
                {
                    func = ((T e) => e == null);
                }
                if (sequence.Any(func))
                {
                    throw new System.ArgumentException(MessagingStrings.SequenceContainsNullElement, parameterName);
                }
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void InRange(bool condition, string parameterName, string message = null)
        {
            if (!condition)
            {
                throw new System.ArgumentOutOfRangeException(parameterName, message);
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void True(bool condition, string parameterName = null, string message = null)
        {
            if (!condition)
            {
                throw new System.ArgumentException(message ?? Strings.InvalidArgument, parameterName);
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void True(bool condition, string parameterName, string unformattedMessage, params object[] args)
        {
            if (!condition)
            {
                throw new System.ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, unformattedMessage, args), parameterName);
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void ValidState(bool condition)
        {
            if (!condition)
            {
                throw new System.InvalidOperationException();
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void ValidState(bool condition, string message)
        {
            if (!condition)
            {
                throw new System.InvalidOperationException(message);
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void ValidState(bool condition, string unformattedMessage, params object[] args)
        {
            if (!condition)
            {
                throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.CurrentCulture, unformattedMessage, args));
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void NotNullSubtype<T>(System.Type type, string parameterName)
        {
            Requires.NotNull<System.Type>(type, parameterName);
            Requires.True(typeof(T).GetTypeInfo().IsAssignableFrom(type), parameterName, MessagingStrings.UnexpectedType, new object[]
            {
                typeof(T).FullName,
                type.FullName
            });
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void Format(bool condition, string message)
        {
            if (!condition)
            {
                throw new System.FormatException(message);
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void Support(bool condition, string message)
        {
            if (!condition)
            {
                throw new System.NotSupportedException(message);
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void Fail(string parameterName, string message)
        {
            throw new System.ArgumentException(message, parameterName);
        }
    }
}
