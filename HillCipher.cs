using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace HillCipher
{
    class HillCipher
    {
        int n = 37;
        string Ralf = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя,. !";
        public void Encryprt(string k, string path)
        {
            string newpath = GetNewPath(path) + "cipher.txt";
            using (StreamWriter newfile = new StreamWriter(newpath, false, Encoding.UTF8))
                newfile.Close();

            double[] ktemp = new double[9];
            for(int j = 0; j < 9; j++)
            {
                ktemp[j] = Ralf.IndexOf(k[j]);
            }
            Matrix<double> key = Matrix<double>.Build.DenseOfRowMajor(3, 3, ktemp);
            using(StreamReader file = new StreamReader(path, Encoding.UTF8))
            {
                while (file.Peek() != -1)
                {
                    double[] Chartemp = new double[3];
                    char temp = (char)file.Read();
                    if (Ralf.IndexOf(temp) == -1)
                    {
                        using (StreamWriter newfile = new StreamWriter(newpath, true, Encoding.UTF8))
                        {
                            newfile.Write(Chartemp[0]);
                        }
                        continue;
                    }
                    Chartemp[0] = Ralf.IndexOf(temp);

                    for (int i = 1; i < 3; i++)
                    {
                        if (file.Peek() != -1) 
                        {
                            temp = (char)file.Read();
                            if (Ralf.IndexOf(temp) == -1)
                            {
                                using (StreamWriter newfile = new StreamWriter(newpath, true, Encoding.UTF8))
                                {
                                    newfile.Write(temp);
                                }
                                i--;
                                continue;
                            }
                            Chartemp[i] = Ralf.IndexOf(temp);
                        }
                        else
                        {
                            if (i == 1)
                            {
                                Chartemp[i] = Chartemp[i-1];
                                Chartemp[i + 1] = Chartemp[i-1];
                                i++;
                            }
                            else
                            {
                                Chartemp[i] = Chartemp[i-1];
                            }
                        }
                    }
                    Vector<double> Char = Vector<double>.Build.DenseOfArray(Chartemp);
                    var newChar = (key * Char) % n;
                    Console.Write(newChar.ToString());

                    for(int i = 0; i < 3; i++) 
                        using (StreamWriter newfile = new StreamWriter(newpath, true, Encoding.UTF8))
                            newfile.Write(Ralf[(int)newChar[i]]);
                }
            }
        }
        private string GetNewPath(string path)
        {
            int index = path.LastIndexOf('\\');
            path = path.Remove(index + 1);
            return path;
        }
    }
}
