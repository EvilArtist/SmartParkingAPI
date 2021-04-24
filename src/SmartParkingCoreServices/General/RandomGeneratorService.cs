using SmartParkingAbstract.Services.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.General
{
    public class RandomGeneratorService : IRandomGeneratorService
    {
        private Random random = new();
        private readonly string aphabetLowerCase = "abcdefghijklmnopqrstuvwxyz";
        private readonly string aphabetUpperCase = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        private readonly string symbols = "&!*$#^|+";
        private readonly string hexString = "0123456789ABCDEF";
        private readonly string numberString = "0123456789";

        public RandomGeneratorService()
        {

        }
        private string RandomInString(string originalString, int minLenght, int maxLength)
        {
            int length = RandomInteger(minLenght, maxLength);
            string generatedString = "";
            for (int i = 0; i < length; i++)
            {
                var charactorIndex = RandomInteger(0, originalString.Length - 1);
                generatedString += originalString[charactorIndex];
            }
            return generatedString;
        }

        public string RandomAlphabetString(int minLength, int maxLength)
        {
            string alphabetFullCase = aphabetLowerCase + aphabetUpperCase;
            return RandomInString(alphabetFullCase, minLength, maxLength);
        }

        public string RandomAlphabetString(int length) => RandomAlphabetString(length, length);

        public string RandomHeximalString(int minLength, int maxLength) => RandomInString(hexString, minLength, maxLength);

        public string RandomHeximalString(int length) => RandomHeximalString(length, length);

        public int RandomInteger(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public string RandomPasswordString(int minLength, int maxLength)
        {
            string passwordString = aphabetLowerCase + aphabetUpperCase + numberString + symbols;
            string password = RandomInString(passwordString, minLength, maxLength);
            while (!ValidatePassword(password))
            {
                password = RandomInString(passwordString, minLength, maxLength);
            }
            return password;
        }

        private bool ValidatePassword(string password)
        {
            bool containsLowerCase = password.Any(x => aphabetLowerCase.Contains(x));
            bool containsUpperCase = password.Any(x => aphabetUpperCase.Contains(x));
            bool containsNumber = password.Any(x => numberString.Contains(x));
            bool containsSymbol = password.Any(x => symbols.Contains(x));
            return containsLowerCase && containsUpperCase && containsNumber && containsSymbol;
        }

        public string RandomPasswordString(int length) => RandomPasswordString(length, length);
    }
}
