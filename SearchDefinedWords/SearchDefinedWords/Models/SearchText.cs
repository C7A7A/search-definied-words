namespace SearchDefinedWords.Models {
    public class SearchText {
        public int ProfileId { get; set; }
        public IFormFile File { get; set; }
        public DirectionType Direction { get; set; }
        public int MaxWordsCount { get; set; }
        public bool IgnoreCase { get; set; } 

        public SearchText(int profileId, IFormFile file, DirectionType direction, int maxWordsCount, bool ignoreCase) {
            ProfileId = profileId;
            File = file;
            Direction = direction;
            MaxWordsCount = maxWordsCount;
            IgnoreCase = ignoreCase;
        }
    }

    public enum DirectionType {
        left,
        right,
        top,
        bottom
    }
}
