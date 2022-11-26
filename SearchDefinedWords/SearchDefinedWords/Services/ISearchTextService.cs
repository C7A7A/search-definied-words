using SearchDefinedWords.Data;

namespace SearchDefinedWords.Services {
    public interface ISearchTextService {
        List<string> SearchText(SearchTextDTO searchText);
    }
}
