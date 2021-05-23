using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.General
{
    public interface IRandomGeneratorService
    {
        public int RandomInteger(int min, int max);
        public string RandomAlphabetString(int minLength, int maxLength);
        public string RandomAlphabetString(int length);
        public string RandomPasswordString(int minLength, int maxLength);
        public string RandomPasswordString(int length);
        public string RandomHeximalString(int minLength, int maxLength);
        public string RandomHeximalString(int length);
    }
}
