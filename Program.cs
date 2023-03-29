using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillCipher
{
    class Program
    {
        static void Main(string[] args)
        {
            HillCipher hillCipher = new HillCipher();
            string path = @"C:\Users\Yesle\Desktop\_-_\КМЗИ\Практ2\text.txt";
            hillCipher.Encryprt("дорогатут", path);
        }
    }
}
