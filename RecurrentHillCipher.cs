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
    class RecurrentHillCipher
    {
        int n = 37;
        string Ralf = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя,. !";
        public void Encrypt(string k1, string k2, string path)
        {
            string newpath = GetNewPath(path) + "cipher.txt";
            using (StreamWriter newfile = new StreamWriter(newpath, false, Encoding.UTF8))
                newfile.Close();

            double[] ktemp1 = new double[9];
            for (int j = 0; j < 9; j++)
            {
                ktemp1[j] = Ralf.IndexOf(k1[j]);
            }
            Matrix<double>  key1 = Matrix<double>.Build.DenseOfRowMajor(3, 3, ktemp1);
            double[] ktemp2 = new double[9];
            for (int j = 0; j < 9; j++)
            {
                ktemp2[j] = Ralf.IndexOf(k2[j]);
            }
            Matrix<double> key2 = Matrix<double>.Build.DenseOfRowMajor(3, 3, ktemp2);

            using (StreamReader file = new StreamReader(path, Encoding.UTF8))
            {
                int j = 0;

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
                    if (j >= 2)
                    {
                        Matrix<double> temp1 = key2 * key1;
                        key1 = key2;
                        key2 = temp1;
                    }
                    Vector<double> Char = Vector<double>.Build.DenseOfArray(Chartemp);
                    Vector<double> newChar = null;
                    if (j == 0) newChar = (key1 * Char) % n;
                    else if (j==1) newChar = (key2 * Char) % n;
                    else newChar = (key2 * Char) % n;

                    Console.Write(newChar.ToString());

                    for (int i = 0; i < 3; i++)
                        using (StreamWriter newfile = new StreamWriter(newpath, true, Encoding.UTF8))
                            newfile.Write(Ralf[(int)newChar[i]]);
                    j++;
                }
            }
        }
        public void Decrypt(string k1, string k2, string path)
        {
            string newpath = GetNewPath(path) + "opentext.txt";
            using (StreamWriter newfile = new StreamWriter(newpath, false, Encoding.UTF8))
                newfile.Close();

            double[] ktemp1 = new double[9];
            for (int j = 0; j < 9; j++)
            {
                ktemp1[j] = Ralf.IndexOf(k1[j]);
            }
            Matrix<double> key1 = Matrix<double>.Build.DenseOfRowMajor(3, 3, ktemp1);
            double[] ktemp2 = new double[9];
            for (int j = 0; j < 9; j++)
            {
                ktemp2[j] = Ralf.IndexOf(k2[j]);
            }
            Matrix<double> key2 = Matrix<double>.Build.DenseOfRowMajor(3, 3, ktemp2);

            Matrix<double> key_1 = InvMatrix(key1);
            Matrix<double> key_2 = InvMatrix(key2);

            using (StreamReader file = new StreamReader(path, Encoding.UTF8))
            {
                int j = 0;
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
                    if (j >= 2)
                    {
                        Matrix<double> temp1 = key_1 * key_2;
                        key_1 = key_2;
                        key_2 = temp1;
                    }
                    Vector<double> Char = Vector<double>.Build.DenseOfArray(Chartemp);
                    Vector<double> newChar = null;
                    if (j == 0) newChar = key_1 * Char;
                    else if (j == 1) newChar = key_2 * Char;
                    else newChar = key_2 * Char;
                    Console.Write(newChar.ToString());

                    for (int i = 0; i < 3; i++)
                        using (StreamWriter newfile = new StreamWriter(newpath, true, Encoding.UTF8))
                            newfile.Write(Ralf[Convert.ToInt32(Modulo(Convert.ToInt32(newChar[i]), n))]);
                    j++;
                }
            }
        }
        private string GetNewPath(string path)
        {
            int index = path.LastIndexOf('\\');
            path = path.Remove(index + 1);
            return path;
        }
        private Matrix<double> InvMatrix(Matrix<double> key)
        {
            int N = 3;
            Matrix<double> a = Matrix<double>.Build.Dense(2, 2);
            double[,] det = new double[3, 3];
            for (int i = 0; i < 3; i++)
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

                    det[(int)i, (int)j] = Modulo(Convert.ToInt32(Math.Pow(-1, i + j) * a.Determinant()), n);
                }
            }
            double det_1 = Euclid(Modulo(Convert.ToInt32(key.Determinant()),n));
            Matrix<double> det1 = Matrix<double>.Build.DenseOfArray(det);
            Matrix<double> key_1 = det_1 * det1.Transpose();
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j <3; j++)
                {
                    key_1[i, j] = Modulo(Convert.ToInt32(key_1[i, j]), n);
                }
            }

            return key_1;
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
    }
}
