using Microsoft.Extensions.Caching.Memory;
using SearchDefinedWords.Models;

namespace SearchDefinedWords.Services {
    public class SearchProfileService : ISearchProfileService {
        private IMemoryCache cache;

        public SearchProfileService(IMemoryCache cache) {
            this.cache = cache;
        }

        public SearchProfile AddProfile(SearchProfile searchProfile) {
            int id = GetSearchProfileId();
            searchProfile.Id = id;

            return SaveSearchProfile(searchProfile);
        }

        private SearchProfile SaveSearchProfile(SearchProfile searchProfile) {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            cache.Set(searchProfile.Id, searchProfile.Words, cacheOptions);

            return searchProfile;
        }

        // basic id autoincrement
        private int GetSearchProfileId() {
            string cacheIdKey = "currentId";

            var cacheIdOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            // if there is already id -> increment by 1
            if (cache.TryGetValue(cacheIdKey, out int cacheId)) {
                Console.WriteLine("Before: {0}", cacheId);
                cache.Remove(cacheIdKey);
                cache.Set(cacheIdKey, ++cacheId, cacheIdOptions);
            } 
            // if there is no id -> create id = 1
            else {
                cacheId = 1;
                cache.Set(cacheIdKey, cacheId, cacheIdOptions);
            }

            Console.WriteLine("After: {0}", cacheId);
            return cacheId;
        }
    }
}
