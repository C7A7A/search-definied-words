namespace SearchDefinedWords.Models {
    public class SearchProfile {
        private long Id { get; set; }

        private List<string> Words { get; set; }

        public SearchProfile(long id, List<string> words) {
            this.Id = id;
            this.Words = words;
        }

        public void AddWords(List<string> words) {
            this.Words.AddRange(words);
        }
    }
}
