namespace SearchDefinedWords.Common {
    public class WordPosition {
        public int line { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }

        public WordPosition(int line, int startIndex, int endIndex) {
            this.line = line;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        public WordPosition() {
        }
    }
}
