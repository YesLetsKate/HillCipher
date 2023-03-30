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
        public void Encrypt(string k, string path)
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
        public void Decrypt(string k, string path)
        {
            string newpath = GetNewPath(path) + "opentext.txt";
            using (StreamWriter newfile = new StreamWriter(newpath, false, Encoding.UTF8))
                newfile.Close();

            double[] ktemp = new double[9];
            for (int j = 0; j < 9; j++)
            {
                ktemp[j] = Ralf.IndexOf(k[j]);
            }

            Matrix<double> key = Matrix<double>.Build.DenseOfRowMajor(3, 3, ktemp);
            Matrix<double> key_1 = InvMatrix(key);

            using (StreamReader file = new StreamReader(path, Encoding.UTF8))
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
                                Chartemp[i] = Chartemp[i - 1];
                                Chartemp[i + 1] = Chartemp[i - 1];
                                i++;
                            }
                            else
                            {
                                Chartemp[i] = Chartemp[i - 1];
                            }
                        }
                    }
                    Vector<double> Char = Vector<double>.Build.DenseOfArray(Chartemp);
                    var newChar = key_1 * Char;
                    Console.Write(newChar.ToString());

                    for (int i = 0; i < 3; i++)
                        using (StreamWriter newfile = new StreamWriter(newpath, true, Encoding.UTF8))
                            newfile.Write(Ralf[Modulo((int)newChar[i], n)]);
                }
            }
        }
        private Matrix<double> InvMatrix(Matrix<double> key)
        {
            int N = 3;
            Matrix<double> a = Matrix<double>.Build.Dense(2, 2);
            double[,] det = new double[3, 3];
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i + 1 == 2)
                    {
                        if (j + 1 == 2)
                        {
                            a[0, 0] = key[Modulo(i + 2, N), Modulo(j + 2, N)];
                            a[0, 1] = key[Modulo(i + 2, N), Modulo(j + 1, N)];
                            a[1, 0] = key[Modulo(i + 1, N), Modulo(j + 2, N)];
                            a[1, 1] = key[Modulo(i + 1, N), Modulo(j + 1, N)];
                        }
                        else
                        {
                            a[0, 0] = key[Modulo(i + 2, N), Modulo(j + 1, N)];
                            a[0, 1] = key[Modulo(i + 2, N), Modulo(j + 2, N)];
                            a[1, 0] = key[Modulo(i + 1, N), Modulo(j + 1, N)];
                            a[1, 1] = key[Modulo(i + 1, N), Modulo(j + 2, N)];
                        }
                    }
                    else
                    {
                        if (j + 1 == 2)
                        {
                            a[0, 0] = key[Modulo(i + 1, N), Modulo(j + 2, N)];
                            a[0, 1] = key[Modulo(i + 1, N), Modulo(j + 1, N)];
                            a[1, 0] = key[Modulo(i + 2, N), Modulo(j + 2, N)];
                            a[1, 1] = key[Modulo(i + 2, N), Modulo(j + 1, N)];
                        }
                        else
                        {
                            a[0, 0] = key[Modulo(i + 1, N), Modulo(j + 1, N)];
                            a[0, 1] = key[Modulo(i + 1, N), Modulo(j + 2, N)];
                            a[1, 0] = key[Modulo(i + 2, N), Modulo(j + 1, N)];
                            a[1, 1] = key[Modulo(i + 2, N), Modulo(j + 2, N)];
                        }
                    }

                    det[i, j] = Modulo(Convert.ToInt32(Math.Pow(-1, i + j) * a.Determinant()),n);
                }
            }
            int det_1 = Euclid(Convert.ToInt32(key.Determinant()));
            Matrix<double> det1 = Matrix<double>.Build.DenseOfArray(det);
            Matrix<double> key_1 = det_1 * det1.Transpose();

            return key_1;
        }
        private int Euclid(int a)
        {
            int q = 0;
            int y = 0;
            int n = this.n;
            int y2 = 0;
            int y1 = 1;
            int r = 1;
            while (r != 0)
            {
                q = n / a;
                r = n % a;
                y = y2 - q * y1;
                n = a;
                a = r;
                y2 = y1;
                y1 = y;
            }
            return Modulo(y2, this.n);
        }
        private int Modulo(int a, int n)
        {
            if (a >= n || a < 0)
            {
                if (a < 0)
                {
                    a = a % n;
                    a = a + n;
                }
                else a = a % n;
            }
            return a;
        }
        private string GetNewPath(string path)
        {
            int index = path.LastIndexOf('\\');
            path = path.Remove(index + 1);
            return path;
        }
    }
}
