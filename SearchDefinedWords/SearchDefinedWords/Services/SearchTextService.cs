using SearchDefinedWords.Data;
using SearchDefinedWords.Models;
using System;
using System.Reflection;
using System.Text;

namespace SearchDefinedWords.Services {
    public class SearchTextService : ISearchTextService {
        private readonly ISearchProfileService searchProfileService;

        public SearchTextService(ISearchProfileService searchProfileService) {
            this.searchProfileService = searchProfileService;
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
            if (ignoreCase) {
                definedWords = definedWords.ConvertAll(word => word.ToLower());
                data = data.ConvertAll(line => line.ToLower());
            }

            string directionType = direction.ToString();
            Console.WriteLine("DirectionType: {0}", directionType);

            if (directionType.Equals("right") || directionType.Equals("left")) {
                string oneStringData = string.Join(" ", data);
                return GetWordsHorizontally(definedWords, oneStringData, maxWordsCount, directionType);
            }
            return GetWordsVertically(definedWords, data, maxWordsCount, direction);
        }
        private List<string> GetWordsHorizontally(List<string> definedWords, string data, int maxWordsCount, string direction) {
            List<string> words = new();

            IEnumerable<string> dataWordByWord = data.Split();
            List<string> dataToCheck = dataWordByWord.ToList();

            // iterate over all defined words
            foreach (string word in definedWords) {
                Console.WriteLine($"definedWord: {word}");
                
                if (data.Contains(word)) {
                    // get index of first word (left) or index of last word (right)
                    int index;
                    if (direction.Equals("left")) {
                        string firstWord = word.Split(" ").First();
                        index = dataToCheck.IndexOf(firstWord);
                    } else {
                        string lastWord = word.Split(" ").Last();
                        index = dataToCheck.IndexOf(lastWord);
                    }

                    Console.WriteLine($"index: {index}");

                    // if definedWord was found
                    if (index >= 0) {
                        string wordsToAdd = "";
                        for (int i = 1; i <= maxWordsCount; i++) {
                            // move word by word in correct way
                            int wordIndex;
                            if (direction.Equals("left")) {
                                wordIndex = index + i - (maxWordsCount + 1);
                            } else {
                                wordIndex = index + i;
                            }
                            Console.WriteLine($"wordIndex: {wordIndex}");

                            wordsToAdd += $" {dataToCheck[wordIndex]}";
                        }

                        words.Add(wordsToAdd.TrimStart().Replace(@"\", ""));
                    }
                }
            }   

            return words;
        }

        private List<string> GetWordsVertically(List<string> definedWords, object oneStringData, int maxWordsCount, DirectionType direction) {
            return definedWords;
        }

    }

    }
