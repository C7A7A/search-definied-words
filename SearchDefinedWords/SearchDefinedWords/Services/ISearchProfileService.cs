using SearchDefinedWords.Models;

namespace SearchDefinedWords.Services {
    public interface ISearchProfileService {
        public SearchProfile AddProfile(SearchProfile searchProfile);
        IEnumerable<SearchProfile> getProfiles();
    }
}
