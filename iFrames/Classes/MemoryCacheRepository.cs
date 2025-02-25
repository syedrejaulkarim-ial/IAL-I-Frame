using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames.Classes
{
    public class MemoryCacheRepository : ICacheRepository
    {
        private Dictionary<string, ICacheable> CacheRepo = new Dictionary<string, ICacheable>();
        #region ICacheRepository Members

        public CacheTypes CacheType
        {
            get;
            set;
        }

        public TimeSpan CacheDefaultTimeOut
        {
            get;
            set;
        }

        public bool Add(ICacheable CacheElement)
        {
            try
            {
                lock (CacheElement)
                {
                    if (CacheElement.CacheTimeOut == DateTime.MinValue)
                        CacheElement.CacheTimeOut = DateTime.Now.Add(CacheDefaultTimeOut);
                    if (!CacheRepo.ContainsKey(CacheElement.Name))
                        CacheRepo.Add(CacheElement.Name, CacheElement);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Exists(string Key)
        {
            try
            {
                lock (CacheRepo)
                {
                    if (!CacheRepo.ContainsKey(Key))
                        return false;
                    var Cache = CacheRepo[Key];
                    var IsAvial = (Cache.CacheTimeOut > DateTime.Now);
                    if (!IsAvial)
                        Remove(Key);
                    return IsAvial;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                RemoveInActive();// Clear other items which is not required 
            }
        }
        public ICacheable Get(string Key)
        {
            lock (CacheRepo)
            {
                if (!CacheRepo.ContainsKey(Key))
                    return null;
                var Cache = CacheRepo[Key];
                if (Cache.CacheTimeOut > DateTime.Now)
                    return Cache;
                Remove(Key);
                return null;
            }
        }

        public bool Remove(string Key)
        {
            try
            {
                lock (CacheRepo)
                {
                    RemoveInActive();
                    return CacheRepo.Remove(Key);
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveAll()
        {
            CacheRepo = null;
            CacheRepo = new Dictionary<string, ICacheable>();
            return true;
        }
        public bool RemoveInActive()
        {
            try
            {
                lock (CacheRepo)
                {
                    foreach (ICacheable cache in CacheRepo.Values)
                    {
                        if (cache.CacheTimeOut < DateTime.Now)
                            CacheRepo.Remove(cache.Name);
                    }
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public ICacheable Get(string Key, bool AddPrefix)
        {
            throw new NotImplementedException();
        }
        #endregion
        public MemoryCacheRepository()
        {
            this.CacheDefaultTimeOut = new TimeSpan(0, 0, 0);
            this.CacheType = CacheTypes.InMemory;
        }

    }

    public enum CacheTypes
    {
        InMemory = 0,
        InFile = 1
    }
    public interface ICacheable
    {
        DateTime CacheTimeOut { get; set; }
        string Name { get; }
        object GetData();
        T GetData<T>();
    }
    public interface ICacheRepository
    {
        CacheTypes CacheType { get; set; }
        TimeSpan CacheDefaultTimeOut { get; set; }
        bool Add(ICacheable CacheElement);
        ICacheable Get(string Key);
        ICacheable Get(string Key, bool AddPrefix);
        bool Exists(string Key);
        bool Remove(string Key);
        bool RemoveAll();
        bool RemoveInActive();
    }
    public class Cacheable : ICacheable
    {
        private object _Data;
        #region ICacheable Members

        public DateTime CacheTimeOut
        {
            get;
            set;
        }

        public string Name
        {
            get;
            private set;
        }

        public object GetData()
        {
            return _Data;
        }

        public T GetData<T>()
        {
            return (T)_Data;//Convert.ChangeType(_Data, typeof(T));
        }

        #endregion
        public Cacheable(object Data, string Key)
        {
            this._Data = Data;
            this.Name = Key;
        }

    }
}