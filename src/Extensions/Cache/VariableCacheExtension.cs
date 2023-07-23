using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace PG.Extensions.Cache
{
    public static class VariableCacheExtension
    {
        public static string GetUniqueCacheKey(this object param, string variableName)
        {
            var _stackTrace = new StackTrace();
            var _methodBase = _stackTrace.GetFrame(1).GetMethod();

            var firstLettersOfParameters = string.Join("", _methodBase.GetParameters().Select(x => x.Name[..1]));
            if (string.IsNullOrWhiteSpace(firstLettersOfParameters))
                firstLettersOfParameters = "noparam";

            var _class = _methodBase.ReflectedType;
            var _namespace = _class.Namespace.Replace(".", "|");
            return _namespace + "|" + _class.Name + "|" + _methodBase.Name + "|" + firstLettersOfParameters + "|" + variableName;
        }

        public static object ToCache(this object param, IMemoryCache memoryProvider, string dependencyCacheKey)
        {
            memoryProvider.Set(dependencyCacheKey, param, DateTimeOffset.Now.AddMinutes(40));
            return param;
        }

        public static T FromCache<T>(this object param, IMemoryCache memoryProvider, string dependencyCacheKey)
        {
            return memoryProvider.Get<T>(dependencyCacheKey) ?? (T)param;
        }
    }
}