using SearchDefinedWords.Models;

namespace SearchDefinedWords.Services {
    public interface ISearchProfileService {
        public SearchProfile AddProfile(SearchProfile searchProfile);
        SearchProfile GetProfile(int profileId);
        IEnumerable<SearchProfile> GetProfiles();
    }
}
