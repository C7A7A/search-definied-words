namespace SearchDefinedWords.Common {
    public class CustomStringComparer {
        public bool ignoreCase { get; set; }

        public CustomStringComparer(bool ignoreCase) {
            this.ignoreCase = ignoreCase;
        }

        public CustomStringComparer() {
        }

        public bool Contains(string mainString, string value) {
            Console.WriteLine(ignoreCase.ToString());
            if (ignoreCase) {
                Console.WriteLine(mainString);
                Console.WriteLine(value);
                Console.WriteLine(mainString.IndexOf(value, StringComparison.OrdinalIgnoreCase).ToString());
                return mainString.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
            } else {
                return mainString.Contains(value);
            }
        }

        public bool Contains(List<string> list, string value) {
            if (ignoreCase) { 
                return list.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)) >= 0;
            } else {
                return list.Contains(value);
            }
        }

        public int IndexOf(string mainString, string value) {
            if (ignoreCase) {
                return mainString.IndexOf(value, StringComparison.OrdinalIgnoreCase);
            }
            return mainString.IndexOf(value);
        }

        public int IndexOf(List<string> list, string value) {
            if (ignoreCase) {
                return list.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
            }
            return list.IndexOf(value);
        }

        public int LastIndexOf(string mainString, string value) {
            if (ignoreCase) {
                return mainString.LastIndexOf(value, StringComparison.OrdinalIgnoreCase);
            }
            return mainString.LastIndexOf(value);
        }

        public int LastIndexOf(List<string> list, string value) {
            if (ignoreCase) {
                return list.FindLastIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
            }
            return list.LastIndexOf(value);
        }
    }
}
