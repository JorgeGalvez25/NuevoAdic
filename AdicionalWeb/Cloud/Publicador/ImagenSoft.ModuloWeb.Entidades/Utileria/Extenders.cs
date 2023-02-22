using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ImagenSoft.ModuloWeb.Extensiones
{
    public static class StreamExtender
    {
        private const int DEFAULT_BUFFER_SIZE = short.MaxValue;

        public static void CopyTo(this Stream input, Stream output)
        {
            input.CopyTo(output, DEFAULT_BUFFER_SIZE);
            return;
        }

        public static void CopyTo(this Stream input, Stream output, int bufferSize)
        {
            if (!input.CanRead) throw new InvalidOperationException("input must be open for reading");
            if (!output.CanWrite) throw new InvalidOperationException("output must be open for writing");

            if (input is MemoryStream) { input.Seek(0, SeekOrigin.Begin); }
            if (output is MemoryStream) { output.Seek(0, SeekOrigin.Begin); }

            byte[][] buf = { new byte[bufferSize], new byte[bufferSize] };
            int[] bufl = { 0, 0 };
            int bufno = 0;
            IAsyncResult read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            IAsyncResult write = null;

            while (true)
            {
                // wait for the read operation to complete
                read.AsyncWaitHandle.WaitOne();
                bufl[bufno] = input.EndRead(read);

                // if zero bytes read, the copy is complete
                if (bufl[bufno] == 0) { break; }

                // wait for the in-flight write operation, if one exists, to complete
                // the only time one won't exist is after the very first read operation completes
                if (write != null)
                {
                    write.AsyncWaitHandle.WaitOne();
                    output.EndWrite(write);
                }

                // start the new write operation
                write = output.BeginWrite(buf[bufno], 0, bufl[bufno], null, null);

                // toggle the current, in-use buffer
                // and start the read operation on the new buffer.
                //
                // Changed to use XOR to toggle between 0 and 1.
                // A little speedier than using a ternary expression.
                bufno ^= 1; // bufno = ( bufno == 0 ? 1 : 0 ) ;
                read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            }

            // wait for the final in-flight write operation, if one exists, to complete
            // the only time one won't exist is if the input stream is empty.
            if (write != null)
            {
                write.AsyncWaitHandle.WaitOne();
                output.EndWrite(write);
            }

            output.Flush();

            // return to the caller ;
            return;
        }
    }

    public static class StringExtender
    {
        public static string ReplaceExx(this string original, string pattern, string replacement)
        {
            return original.ReplaceExx(pattern, replacement, StringComparison.CurrentCultureIgnoreCase);
        }
        public static string ReplaceExx(this string original, string pattern, string replacement, StringComparison comparisonType)
        {
            if (original == null)
            {
                return null;
            }

            if (String.IsNullOrEmpty(pattern))
            {
                return original;
            }

            int lenPattern = pattern.Length;
            int idxPattern = -1;
            int idxLast = 0;

            System.Text.StringBuilder result = new System.Text.StringBuilder();

            while (true)
            {
                idxPattern = original.IndexOf(pattern, idxPattern + 1, comparisonType);

                if (idxPattern < 0)
                {
                    result.Append(original, idxLast, original.Length - idxLast);
                    break;
                }

                result.Append(original, idxLast, idxPattern - idxLast);
                result.Append(replacement);

                idxLast = idxPattern + lenPattern;
            }

            return result.ToString();
        }

        public static string ToTitle(this string original)
        {
            if (string.IsNullOrEmpty(original)) { return string.Empty; }
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(original.ToLower());
        }

        public static bool EqualsIgnoreCase(this string original, string equal)
        {
            if (original == null) { return false; }
            return original.Equals(equal, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public static class DateTimeExtender
    {
        public static bool IsBetween(this DateTime dt, DateTime start, DateTime end)
        {
            return (dt >= start && dt <= end);
        }
    }

    public static class ObjectExtender
    {
        public static T Clonar<T>(this T current) where T : class, new()
        {
            T tgt = new T();
            System.Reflection.PropertyInfo[] props = current.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo pi in props)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(tgt, pi.GetValue(current, null), null);
                }
            }

            return tgt;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source) where T : class, new()
        {
            if (!typeof(T).IsSerializable)
            {
                return null;
                //throw new ArgumentException("El objeto debe ser serializable", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static bool Compare<T>(this T current, T target)
        {
            return ObjectExtender.AreObjectsEqual(current, target, new string[0]);

            //try
            //{
            //    //return false if any of the object is false
            //    if (current == null || target == null)
            //        return false;

            //    //Get the type of the object
            //    Type type = typeof(T);

            //    if (current is IEnumerable && target is IEnumerable)
            //    {
            //        return (current as IEnumerable).Cast<T>().SequenceEqual((target as IEnumerable).Cast<T>());
            //    }

            //    string Object1Value = string.Empty;
            //    string Object2Value = string.Empty;

            //    foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            //    {
            //        if (property.Name != "ExtensionData")
            //        {
            //            if (typeof(ICollection).IsAssignableFrom(type.GetProperty(property.Name).PropertyType))
            //            {

            //            }

            //            if (type.GetProperty(property.Name).GetValue(current, null) != null)
            //                Object1Value = type.GetProperty(property.Name).GetValue(current, null).ToString();
            //            if (type.GetProperty(property.Name).GetValue(target, null) != null)
            //                Object2Value = type.GetProperty(property.Name).GetValue(target, null).ToString();
            //            if (Object1Value.Trim() != Object2Value.Trim())
            //            {
            //                return false;
            //            }

            //            Object1Value = string.Empty;
            //            Object2Value = string.Empty;
            //        }
            //    }
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Compares the properties of two objects of the same type and returns if all properties are equal.
        /// </summary>
        /// <param name="objectA">The first object to compare.</param>
        /// <param name="objectB">The second object to compre.</param>
        /// <param name="ignoreList">A list of property names to ignore from the comparison.</param>
        /// <returns><c>true</c> if all property values are equal, otherwise <c>false</c>.</returns>
        public static bool AreObjectsEqual(object objectA, object objectB, params string[] ignoreList)
        {
            bool result;

            if (objectA != null && objectB != null)
            {
                Type objectType = objectA.GetType();

                result = true; // assume by default they are equal

                if (objectA is ICollection && objectB is ICollection)
                {
                    return (objectA as IEnumerable).Cast<object>().SequenceEqual((objectA as IEnumerable).Cast<object>());
                }

                foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && !ignoreList.Contains(p.Name)))
                {
                    object valueA = propertyInfo.GetValue(objectA, null);
                    object valueB = propertyInfo.GetValue(objectB, null);

                    // if it is a primative type, value type or implements IComparable, just directly try and compare the value
                    if (CanDirectlyCompare(propertyInfo.PropertyType))
                    {
                        if (!AreValuesEqual(valueA, valueB))
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName, propertyInfo.Name);
                            result = false;
                        }
                    }
                    // if it implements IEnumerable, then scan any items
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        IEnumerable<object> collectionItems1;
                        IEnumerable<object> collectionItems2;
                        int collectionItemsCount1;
                        int collectionItemsCount2;

                        // null check
                        if (valueA == null && valueB != null || valueA != null && valueB == null)
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName, propertyInfo.Name);
                            result = false;
                        }
                        else if (valueA != null && valueB != null)
                        {
                            collectionItems1 = ((IEnumerable)valueA).Cast<object>();
                            collectionItems2 = ((IEnumerable)valueB).Cast<object>();
                            collectionItemsCount1 = collectionItems1.Count();
                            collectionItemsCount2 = collectionItems2.Count();

                            // check the counts to ensure they match
                            if (collectionItemsCount1 != collectionItemsCount2)
                            {
                                Console.WriteLine("Collection counts for property '{0}.{1}' do not match.", objectType.FullName, propertyInfo.Name);
                                result = false;
                            }
                            // and if they do, compare each item... this assumes both collections have the same order
                            else
                            {
                                for (int i = 0; i < collectionItemsCount1; i++)
                                {
                                    object collectionItem1;
                                    object collectionItem2;
                                    Type collectionItemType;

                                    collectionItem1 = collectionItems1.ElementAt(i);
                                    collectionItem2 = collectionItems2.ElementAt(i);
                                    collectionItemType = collectionItem1.GetType();

                                    if (CanDirectlyCompare(collectionItemType))
                                    {
                                        if (!AreValuesEqual(collectionItem1, collectionItem2))
                                        {
                                            Console.WriteLine("Item {0} in property collection '{1}.{2}' does not match.", i, objectType.FullName, propertyInfo.Name);
                                            result = false;
                                        }
                                    }
                                    else if (!AreObjectsEqual(collectionItem1, collectionItem2, ignoreList))
                                    {
                                        Console.WriteLine("Item {0} in property collection '{1}.{2}' does not match.", i, objectType.FullName, propertyInfo.Name);
                                        result = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (!AreObjectsEqual(propertyInfo.GetValue(objectA, null), propertyInfo.GetValue(objectB, null), ignoreList))
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName, propertyInfo.Name);
                            result = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot compare property '{0}.{1}'.", objectType.FullName, propertyInfo.Name);
                        result = false;
                    }
                }
            }
            else
                result = object.Equals(objectA, objectB);

            return result;
        }

        /// <summary>
        /// Determines whether value instances of the specified type can be directly compared.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this value instances of the specified type can be directly compared; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>
        /// <param name="valueA">The first value to compare.</param>
        /// <param name="valueB">The second value to compare.</param>
        /// <returns><c>true</c> if both values match, otherwise <c>false</c>.</returns>
        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;
            IComparable selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false; // one of the values is null
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false; // the comparison using IComparable failed
            else if (!object.Equals(valueA, valueB))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }

        public static bool IsBetween(this int source, int start, int end)
        {
            return (source >= start && source <= end);
        }
    }
}