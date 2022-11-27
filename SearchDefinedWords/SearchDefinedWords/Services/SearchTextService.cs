using SearchDefinedWords.Common;
using SearchDefinedWords.Data;
using SearchDefinedWords.Models;

namespace SearchDefinedWords.Services {
    public class SearchTextService : ISearchTextService {
        private readonly ISearchProfileService searchProfileService;
        private CustomStringComparer comparer;

        public SearchTextService(ISearchProfileService searchProfileService, CustomStringComparer comparer) {
            this.searchProfileService = searchProfileService;
            this.comparer = comparer;
        }

        public List<string> SearchText(SearchTextDTO searchText) {
            SearchProfile profile = searchProfileService.GetProfile(searchText.ProfileId);
            List<string> definedWords = profile.Words;
            if (definedWords == null) {
                return new List<string>();
            }

            List<string> data = GetDataFromFile(searchText.File);
            return FindWords(definedWords, data, searchText.Direction, searchText.IgnoreCase, searchText.MaxWordsCount);
        }

        private List<string> GetDataFromFile(IFormFile file) {
            List<string> result = new();
            using (var reader = new StreamReader(file.OpenReadStream())) {
                while (reader.Peek() >= 0)
                    result.Add(reader.ReadLine());
            }
            return result;
        }

        private List<string> FindWords(List<string> definedWords, List<string> data, DirectionType direction, bool ignoreCase, int maxWordsCount) {
            comparer.ignoreCase = ignoreCase;

            string directionType = direction.ToString();
            Console.WriteLine("DirectionType: {0}", directionType);

            if (directionType.Equals("right") || directionType.Equals("left")) {
                string oneStringData = string.Join(" ", data);
                return GetWordsHorizontally(definedWords, oneStringData, maxWordsCount, directionType);
            }
            return GetWordsVertically(definedWords, data, maxWordsCount, directionType);
        }
        private List<string> GetWordsHorizontally(List<string> definedWords, string data, int maxWordsCount, string direction) {
            List<string> words = new();

            IEnumerable<string> dataWordByWord = data.Split();
            List<string> dataToCheck = dataWordByWord.ToList();

            // iterate over all defined words
            foreach (string word in definedWords) {
                //Console.WriteLine($"definedWord: {word}");
                
                if (comparer.Contains(data, word)) {
                    // get index of first word (left) or index of last word (right)
                    int index = GetIndexHorizontally(direction, dataToCheck, word);
                    //Console.WriteLine($"index: {index}");

                    // if definedWord was found
                    if (index >= 0) {
                        string wordsToAdd = GetWordsNearDefinedWords(maxWordsCount, direction, dataToCheck, index);
                        words.Add(wordsToAdd);
                    }
                }
            }   

            return words;
        }

        private static string GetWordsNearDefinedWords(int maxWordsCount, string direction, List<string> dataToCheck, int index) {
            string words = "";
            for (int i = 1; i <= maxWordsCount; i++) {
                // move word by word in correct way
                int wordIndex;
                if (direction.Equals("left")) {
                    wordIndex = index + i - (maxWordsCount + 1);
                } else {
                    wordIndex = index + i;
                }
                //Console.WriteLine($"wordIndex: {wordIndex}");

                words += $" {dataToCheck[wordIndex]}";
            }

            return words.TrimStart();
        }

        private int GetIndexHorizontally(string direction, List<string> dataToCheck, string word) {
            if (direction.Equals("left")) {
                string firstWord = word.Split(" ").First();
                return comparer.IndexOf(dataToCheck, firstWord);
            } else {
                string lastWord = word.Split(" ").Last();
                return comparer.IndexOf(dataToCheck, lastWord);
            }
        }

        private List<string> GetWordsVertically(List<string> definedWords, List<string> data, int maxWordsCount, string direction) {
            List<string> words = new();

            List<WordPosition> positions = ParseData(data, definedWords);
            List<WordPosition> foundWords = new();

            foreach (string word in definedWords) {
                WordPosition position = new();
                for (int i = 0; i < data.Count; i++) {
                    position.line = i;
                    position.startIndex = comparer.IndexOf(data[i], word);
                    position.endIndex = position.startIndex + word.Length - 1;

                    if (position.startIndex != -1) {
                        break;
                    }
                }

                if (position.startIndex != -1) {
                    foundWords = positions.FindAll(x =>
                        CheckLine(position.line, x.line, direction) &&
                        CheckIndexes(position.startIndex, position.endIndex, x.startIndex, x.endIndex)
                    );

                    if (foundWords.Count > 0) {
                        foundWords = foundWords.Take(maxWordsCount).ToList();
                        string wordToAdd = "";
                        foreach (WordPosition wordPosition in foundWords) {
                            int length = wordPosition.endIndex - wordPosition.startIndex + 1;
                            wordToAdd += $"{data[wordPosition.line].Substring(wordPosition.startIndex, length)} ";
                            //Console.WriteLine(wordToAdd);
                        }
                        words.Add(wordToAdd.TrimEnd());
                    }
                }
            }

            return words;
        }

        private bool CheckIndexes(int startIndexDefined, int endIndexDefined, int startIndexToCheck, int endIndexToCheck) {
            if (endIndexToCheck >= startIndexDefined && endIndexToCheck <= endIndexDefined) {
                return true;
            } else if (startIndexToCheck >= startIndexDefined && startIndexToCheck <= endIndexDefined) {
                return true;
            }
            return false;
        }

        private static bool CheckLine(int definedWordLine, int lineToCheck, string direction) {
            if (direction.Equals("top")) {
                return definedWordLine - 1 == lineToCheck;
            }
            return definedWordLine + 1 == lineToCheck;
        }

        private List<WordPosition> ParseData(List<string> data, List<string> definedWords) {
            List<WordPosition> positions = new();
            WordPosition position = new(-1, -1, -1);

            for (int i = 0; i < data.Count; i++) {
                position.line = i;
                int index = 0;
                //Console.WriteLine("line: {0}", i);

                foreach (char c in data[i]) {
                    // start of word
                    if (position.startIndex == -1 && !Char.IsWhiteSpace(c)) {
                        position.startIndex = index;
                        //Console.WriteLine("start: {0}", index);
                    }
                    //Console.WriteLine("char: {0}", c);

                    // if char is space or last elemet of string -> end of word
                    if (Char.IsWhiteSpace(c)) {
                        position.endIndex = index - 1;
                        //Console.WriteLine("end: {0}", position.endIndex);
                        positions.Add(position);
                        position = new(i, -1, -1);

                    } else if ((data[i].Length) == index + 1) {
                        position.endIndex = index;
                        //Console.WriteLine("end: {0}", position.endIndex);
                        positions.Add(position);
                        position = new(i, -1, -1);
                    }

                    index++;
                }
            }

            return positions;
        }
    }
}
