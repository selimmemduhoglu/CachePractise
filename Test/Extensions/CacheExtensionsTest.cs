using PG.Extensions.Cache;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace PG.Test.Extensions.Cache;

public class CacheTest
{
    private readonly IMemoryCache _memoryCache;
    public CacheTest(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [Fact]
    public void HasVariableCachedData_ReturnTrue()
    {
        string dummy = "test", oldDummy = "test";
        string uniqueCacheKey = this.GetUniqueCacheKey(nameof(dummy));
        
        dummy.ToCache(_memoryCache, uniqueCacheKey);
        dummy = string.Empty;

        if (dummy.FromCache<string>(_memoryCache, uniqueCacheKey) == oldDummy)
            Assert.True(true);
        else
            Assert.False(true, "Variable cache is not working");
    }
}