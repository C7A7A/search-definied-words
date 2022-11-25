using Microsoft.Extensions.Caching.Memory;
using SearchDefinedWords.Models;

namespace SearchDefinedWords.Services {
    public class SearchProfileService : ISearchProfileService {
        private IMemoryCache cache;
        private string cacheIdKey;

        public SearchProfileService(IMemoryCache cache) {
            this.cache = cache;
            this.cacheIdKey = "currentId";
        }

        public IEnumerable<SearchProfile> getProfiles() {
            if (cache.TryGetValue(this.cacheIdKey, out int cacheId)) {
                return GetAllProfiles(cacheId);
            }

            return new List<SearchProfile>();
        }

        private IEnumerable<SearchProfile> GetAllProfiles(int highestId) {
            List<SearchProfile> profiles = new List<SearchProfile>();

            for (int i = 0; i <= highestId; i++) {
                List<string> words = cache.Get<List<string>>(i);

                /*
                foreach (var word in words) {
                    Console.WriteLine(word);
                }
                */

                profiles.Add(new SearchProfile(highestId, words));
            }

            return profiles;
        }

        public SearchProfile AddProfile(SearchProfile searchProfile) {
            int id = GetSearchProfileId();
            searchProfile.Id = id;
            //Console.WriteLine($"save profile with {id}");
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
            var cacheIdOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            // if there is already id -> increment by 1
            if (cache.TryGetValue(this.cacheIdKey, out int cacheId)) {
                cache.Remove(this.cacheIdKey);
                cacheId++;
                cache.Set(this.cacheIdKey, cacheId, cacheIdOptions);
            } 
            // if there is no id -> create id = 1
            else {
                cacheId = 0;
                cache.Set(this.cacheIdKey, cacheId, cacheIdOptions);
            }

            return cacheId;
        }
    }
}
