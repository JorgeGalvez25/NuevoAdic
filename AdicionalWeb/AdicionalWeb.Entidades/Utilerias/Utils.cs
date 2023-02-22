using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

public static class ObjectExtender
{
    public static string GetMD5(Stream f)
    {
        MD5 hash = MD5.Create();
        byte[] buffer = hash.ComputeHash(f);
        return CompileMD5(buffer);
    }

    public static string GetMD5(this string cadena)
    {
        return GetMD5(Encoding.Default.GetBytes(cadena));
    }

    public static string GetMD5(this byte[] buffering)
    {
        MD5 hash = MD5.Create();
        byte[] buffer = hash.ComputeHash(buffering);
        return CompileMD5(buffer);
    }

    private static string CompileMD5(this byte[] buffer)
    {
        StringBuilder sb = new StringBuilder(buffer.Length);
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(buffer[i].ToString("x2"));
            }
        }
        return sb.ToString();
    }

    public static bool ValidarEncriptado(this string contrasenia, string encriptado)
    {
        return GetMD5(contrasenia).Equals(encriptado, StringComparison.CurrentCultureIgnoreCase);
    }

    public static T Clone<T>(this T source) where T : class, new()
    {
        if (!typeof(T).IsSerializable)
        {
            return null;
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

    public static string ToJSON<T>(this T source) where T : class, new()
    {
        if (!typeof(T).IsSerializable)
        {
            return null;
        }
        if (Object.ReferenceEquals(source, null))
        {
            return null;
        }

        return new JavaScriptSerializer().Serialize(source);
    }
}

public static class StringExtender
{
    public static string ReplaceEx(this string original, string pattern, string replacement)
    {
        return original.ReplaceEx(pattern, replacement, StringComparison.CurrentCultureIgnoreCase);
    }
    public static string ReplaceEx(this string original, string pattern, string replacement, StringComparison comparisonType)
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

        StringBuilder result = new StringBuilder();

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
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(original.ToLower());
    }
}

public static class DateTimeExtender
{
    public static bool IsBetween(this DateTime dt, DateTime start, DateTime end)
    {
        return (dt >= start && dt <= end);
    }
}

public static class IEnumerableExtender
{
    public static void AsyncForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        if (items == null)
            throw new ArgumentNullException("enumerable");
        if (action == null)
            throw new ArgumentNullException("action");

        var count = items.Count();

        if (count == 0)
        {
            return;
        }

        if (count == 1)
        {
            action(items.FirstOrDefault());
        }
        else
        {
            var resetEvents = new List<System.Threading.ManualResetEvent>();

            foreach (var item in items)
            {
                var evt = new System.Threading.ManualResetEvent(false);
                System.Threading.ThreadPool.QueueUserWorkItem((i) =>
                {
                    try { action((T)i); }
                    finally { evt.Set(); }
                }, item);
                resetEvents.Add(evt);
            }

            foreach (var re in resetEvents)
            {
                re.WaitOne();
            }
        }
    }
}