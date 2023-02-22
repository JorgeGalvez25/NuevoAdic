using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia
{
    public class CachePersistencia
    {
        private static int _maxMinutes = 30;
        public static int MaxMinutes { set { _maxMinutes = value; } }

        private static readonly object _lock = new object();
        private static Dictionary<long, CacheItem> _cache = new Dictionary<long, CacheItem>();

        public static void Add(long key, string valor)
        {
            lock (_lock)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache[key].Valor = valor;
                    _cache[key].MaxCache = DateTime.Now.AddMinutes(_maxMinutes);
                }
                else
                {
                    _cache.Add(key, new CacheItem(valor, DateTime.Now.AddMinutes(_maxMinutes)));
                }
            }
        }

        public static bool Exist(long key)
        {
            return _cache.ContainsKey(key);
        }

        public static void Purge()
        {
            lock (_lock)
            {
                DateTime now = DateTime.Now;
                Dictionary<long, CacheItem> aux = new Dictionary<long, CacheItem>();
                Parallel.ForEach(_cache, p =>
                {
                    if (p.Value.MaxCache < now)
                    {
                        aux.Add(p.Key, p.Value);
                    }
                });

                _cache.Clear();
                _cache = aux;
            }
        }

        public static string Get(long key)
        {
            DateTime now = DateTime.Now;
            if (_cache.ContainsKey(key))
            {
                return _cache[key].MaxCache < now ? string.Empty : _cache[key].Valor;
            }

            return string.Empty;
        }
    }

    public class CacheItem
    {
        public CacheItem(string valor, DateTime maxCache)
        {
            this.Valor = valor;
            this.MaxCache = maxCache;
        }

        public string Valor { get; set; }

        public DateTime MaxCache { get; set; }
    }
}
