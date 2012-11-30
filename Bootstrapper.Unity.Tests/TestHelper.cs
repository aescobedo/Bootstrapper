namespace Bootstrapper.Unity
{
    using System;
    using System.IO;
    using System.Reflection;

    using log4net;

    using NUnit.Framework;

    /// <summary>
    /// Provides support functionality for common testing tasks.
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// A delegate for a unit test method.
        /// </summary>
        public delegate void UnitTestMethod();

        /// <summary>
        /// Mocks the log4 net static logger property "Log".
        /// </summary>
        /// <param name="testee">The testee.</param>
        /// <param name="loggerMock">The logger mock.</param>
        public static void MockLog4Net(object testee, ILog loggerMock)
        {
            FieldInfo fieldInfo = testee.GetType().GetField("Log", BindingFlags.NonPublic | BindingFlags.Static);

            if (fieldInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "Field '{0}' does not exist on type '{1}'.", "Log", testee.GetType()));
            }

            fieldInfo.SetValue(testee, loggerMock);
        }

        /// <summary>
        /// Gets a field value of an object.
        /// </summary>
        /// <typeparam name="T">Type of the value to retrieve.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Retrieved value.</returns>
        public static T GetFieldValue<T>(object obj, string fieldName)
        {
            FieldInfo info = null;
            Type type = obj.GetType();
            while (info == null && type != null)
            {
                info = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                type = type.BaseType;
            }

            if (info == null)
            {
                throw new ArgumentException(string.Format(
                    "Field '{0}' does not exist on type '{1}'.", fieldName, obj.GetType()));
            }

            return (T)info.GetValue(obj);
        }

        /// <summary>
        /// Sets a field of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static void SetFieldValue(object obj, string fieldName, object value)
        {
            FieldInfo info = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (info == null)
            {
                throw new ArgumentException(string.Format(
                    "Field '{0}' does not exist on type '{1}'.", fieldName, obj.GetType()));
            }

            info.SetValue(obj, value);
        }

        /// <summary>
        /// Gets a property value of an object.
        /// </summary>
        /// <typeparam name="T">Type of the value to retrieve.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Retrieved value.</returns>
        public static T GetPrivatePropery<T>(object obj, string propertyName)
        {
            PropertyInfo info = null;
            Type type = obj.GetType();
            while (info == null && type != null)
            {
                info = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
                type = type.BaseType;
            }

            if (info == null)
            {
                throw new ArgumentException(string.Format(
                    "Property '{0}' does not exist on type '{1}'.", propertyName, obj.GetType()));
            }

            return (T)info.GetValue(obj, null);
        }


        /// <summary>
        /// Sets a property value of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public static void SetPrivateProperty(object obj, string propertyName, object value)
        {
            PropertyInfo info = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (info == null)
            {
                throw new ArgumentException(string.Format(
                    "Property '{0}' does not exist on type '{1}'.", propertyName, obj.GetType()));
            }

            info.SetValue(obj, value, null);
        }

        /// <summary>
        /// Gets the value of a property of an object.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Retrieved value.</returns>
        public static T GetPropertyValue<T>(object obj, string propertyName)
        {
            PropertyInfo info = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (info == null)
            {
                throw new ArgumentException(string.Format(
                    "Property '{0}' does not exist on type '{1}'.", propertyName, obj.GetType()));
            }

            return (T)info.GetValue(obj, null);
        }

        /// <summary>
        /// Calls a private method.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the called method.</returns>
        public static object CallPrivateMethod(object obj, string methodName, params object[] parameters)
        {
            MethodInfo[] methodInfos = obj.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MethodInfo method in methodInfos)
            {
                if (method.Name == methodName)
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    if (parameterInfos.Length == parameters.Length)
                    {
                        bool match = true;
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i] != null && !parameterInfos[i].ParameterType.IsInstanceOfType(parameters[i]))
                            {
                                match = false;
                            }
                        }

                        if (match)
                        {
                            return method.Invoke(obj, parameters);
                        }
                    }
                }
            }

            throw new ArgumentException(string.Format("IntegerType {0} does not have a private method with name {1}.", obj.GetType().FullName, methodName));
        }

        /// <summary>
        /// Executes the required unit test.
        /// </summary>
        /// <param name="unitTestMethod">The unit test method.</param>
        public static void ExecuteRequiredUnitTest(UnitTestMethod unitTestMethod)
        {
            const string ErrorMessage = "Required unit test failed: ";
            try
            {
                unitTestMethod.Invoke();
            }
            catch (Exception e)
            {
                if (e.Message.StartsWith(ErrorMessage))
                {
                    throw;
                }

                Assert.Fail(ErrorMessage + unitTestMethod.Method.Name);
            }
        }

        /// <summary>
        /// Compares the files
        /// </summary>
        /// <param name="file1">The file1 to compare</param>
        /// <param name="file2">The file2 to compare</param>
        /// <returns>True if the are identical otherwise false</returns>
        public bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            var fs1 = new FileStream(file1, FileMode.Open);
            var fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            } 
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return (file1byte - file2byte) == 0;
        }
    }
}
