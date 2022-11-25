using Microsoft.Extensions.Caching.Memory;
using SearchDefinedWords.Models;

namespace SearchDefinedWords.Services {
    public class SearchProfileService : ISearchProfileService {
        private IMemoryCache cache;

        public SearchProfileService(IMemoryCache cache) {
            this.cache = cache;
        }

        public IEnumerable<SearchProfile> getProfiles() {
            int id = GetSearchProfileId();

            return GetAllProfiles(id);
        }

        private IEnumerable<SearchProfile> GetAllProfiles(int id) {
            List<SearchProfile> profiles = new List<SearchProfile>();

            for (int i = 0; i < id; i++) {
                cache.TryGetValue(i, out int profileId);
                List<string> words = (List<string>)cache.Get(profileId);

                profiles.Add(new SearchProfile(id, words));
            }

            return profiles;
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
                cacheId = 0;
                cache.Set(cacheIdKey, cacheId, cacheIdOptions);
            }

            Console.WriteLine("After: {0}", cacheId);
            return cacheId;
        }
    }
}
