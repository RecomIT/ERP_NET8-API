using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public static class TextFormatter
    {
        // Input: YeasinAhmed // Output: Yeasin Ahmed
        public static string AddSpaceInTextWhenProperCase(string input)
        {
            StringBuilder output = new StringBuilder(input);
            List<int> indexes = new List<int>();
            for (int i = 1; i < input.Length; i++) {
                if (char.IsUpper(input[i])) {
                    indexes.Add(i);
                }
            }

            int inceremnet = 0;
            foreach (var index in indexes) {
                output.Insert(index + inceremnet, ' ');
                inceremnet++;
            }
            return output.ToString();
        }
    }
}
