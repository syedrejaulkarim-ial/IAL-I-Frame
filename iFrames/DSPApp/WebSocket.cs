using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames.DSPApp
{
    public interface IWebSockets
    {
        /// <summary>
        /// Create User session
        /// </summary>
        /// <param name="Key">Unique Name</param>
        /// <param name="ExpireOn">Key Expire at</param>
        /// <returns></returns>
        bool CreateChannel(string Key, string Details);
        /// <summary>
        /// Key Exists or not
        /// </summary>
        /// <param name="Key">The Key</param>
        /// <returns></returns>
        bool Exists(string Key);
        /// <summary>
        /// Modify Key at run time and extend expire
        /// </summary>
        /// <param name="Key">Old key</param>
        /// <param name="NewKey">New Key</param>
        /// <param name="ExpireOn">New Expire time</param>
        /// <returns></returns>
        bool Replace(string Key, string NewKey, string Details);
        AuthDetails Get(string Key);
        bool Set(string Key, string Details);
        string[] GetAllKeys();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        bool Remove(string Key);
        // public bool RemoveDuplicate(string Key);
        bool AddToSocket(string Key, string Value);
    }
}