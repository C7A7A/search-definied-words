using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SearchDefinedWords.Models {
    public class SearchProfile {
        [JsonIgnore]
        public int Id { get; set; }

        public List<string> Words { get; set; }

        public SearchProfile(int id, List<string> words) {
            this.Id = id;
            this.Words = words;
        }

        public void AddWords(List<string> words) {
            this.Words.AddRange(words);
        }
    }
}
