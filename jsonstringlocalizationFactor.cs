using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace json_based_localization
{
    public class jsonstringlocalizationFactor : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;

        public jsonstringlocalizationFactor(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalzation(_cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalzation(_cache);
        }
    }
}
