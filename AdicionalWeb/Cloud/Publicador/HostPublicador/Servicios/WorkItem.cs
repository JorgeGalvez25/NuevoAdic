using System;
using System.Collections;
using System.Collections.Generic;

public static class WorkItem
{
    public class Objetos<T> where T : class
    {
        internal static Dictionary<Type, T> objetos = new Dictionary<Type, T>();

        public static void Add(T valor)
        {
            if (objetos.ContainsKey(typeof(T)))
            {
                objetos.Remove(typeof(T));
            }

            objetos.Add(typeof(T), valor);
        }

        public static void Delete(T valor)
        {
            if (objetos.ContainsKey(typeof(T)))
            {
                objetos.Remove(typeof(T));
            }
        }

        public static T Get()
        {
            if (objetos.ContainsKey(typeof(T)))
            {
                return (T)objetos[typeof(T)];
            }

            return null;
        }

        public static bool Exist()
        {
            return objetos.ContainsKey(typeof(T));
        }

        public static void Clear()
        {
            foreach (Type item in objetos.Keys)
            {
                if (objetos[item] is IList)
                {
                    (objetos[item] as IList).Clear();
                }

                if (objetos[item] is IDisposable)
                {
                    (objetos[item] as IDisposable).Dispose();
                }

                objetos[item] = null;
            }
            objetos.Clear();
        }
    }

    public static void Clear()
    {
        Objetos<object>.Clear();
    }
}
