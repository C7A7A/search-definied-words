namespace SearchDefinedWords.Data {
    public class SearchTextDTO
    {
        public int ProfileId { get; set; }
        public IFormFile File { get; set; }
        public DirectionType Direction { get; set; }
        public int MaxWordsCount { get; set; }
        public bool IgnoreCase { get; set; }

        public SearchTextDTO() {
        }

        public SearchTextDTO(int profileId, IFormFile file, DirectionType direction, int maxWordsCount, bool ignoreCase)
        {
            ProfileId = profileId;
            File = file;
            Direction = direction;
            MaxWordsCount = maxWordsCount;
            IgnoreCase = ignoreCase;
        }
    }
}
