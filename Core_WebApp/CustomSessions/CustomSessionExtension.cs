using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Core_WebApp.CustomSessions
{
    /// <summary>
    ///  static method for ISession interface
    ///  to store CLR object in JSON format in Session State
    /// </summary>
    public static class CustomSessionExtension
    {
        public static void SetSessionData<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetSessionData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
    }
}
