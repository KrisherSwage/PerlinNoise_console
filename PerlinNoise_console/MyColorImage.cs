using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class MyColorImage
    {
        double maxMatrix = 0;
        double minMatrix = 0;

        List<double> step = new List<double>();

        int numOfSteps = 16;

        List<List<byte>> byteCol = new List<List<byte>>()
        {
           new List<byte>{ 7, 1, 120 }, new List<byte>{ 10, 2, 172 }, new List<byte>{ 13, 66, 190 }, new List<byte>{ 10, 136, 190 }, new List<byte>{ 65, 211, 204 },
           new List<byte>{ 242, 210, 82 }, new List<byte>{ 241, 208, 41 },
           new List<byte>{ 23, 165, 5 }, new List<byte>{ 20, 142, 4 }, new List<byte>{ 56, 115, 34 }, new List<byte>{ 72,99 ,37  }, new List<byte>{91 ,99 , 59 },
           new List<byte>{ 99 , 99, 99 }, new List<byte>{ 132, 132, 132 }, new List<byte>{ 213, 213, 213 }, new List<byte>{ 255, 255, 255 }
        };


        public MyColorImage(List<List<double>> matrix)
        {
            maxMatrix = FindMax(matrix);
            minMatrix = FindMin(matrix);

            //List<double> step = new List<double>();
            for (int i = 0; i < numOfSteps; i++)
            {
                step.Add((1.0 / numOfSteps) * (i + 1.0));
            }

        }


        public void CreateImage(List<List<double>> mainMatrix, string fullName)
        {
            var inputSize = mainMatrix.Count /** mainMatrix.Count*/;
            //int inputSize = Convert.ToInt32(Console.ReadLine());

            //var pathNewBMP = args[1];
            Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\MyColor");
            var pathNewBMP = $@"{Directory.GetCurrentDirectory()}\MyColor\MyColorNois_{fullName}.bmp";

            if (inputSize < 0)
            {
                inputSize *= -1;
            }
            if (inputSize == 0)
            {
                inputSize = 1;
            }
            int inpSi1 = inputSize & 0xFF; //для ширины и высоты
            int inpSi2 = (inputSize & 0xFF00) / 0x100;
            int inpSi3 = (inputSize & 0xFF0000) / 0x10000;
            int inpSi4 = (int)((inputSize & 0xFF000000) / 0x1000000);

            int sizeOfCan = (inputSize * inputSize) * 3; //*3 т.к. 3 байт/пиксель
            int siOfCa1 = sizeOfCan & 0xFF; //для размера изображения
            int siOfCa2 = (sizeOfCan & 0xFF00) / 0x100;
            int siOfCa3 = (sizeOfCan & 0xFF0000) / 0x10000;
            int siOfCa4 = (int)((sizeOfCan & 0xFF000000) / 0x1000000);

            int bfSize = 54 + sizeOfCan;
            int bfSi1 = bfSize & 0xFF; //для размера файла
            int bfSi2 = (bfSize & 0xFF00) / 0x100;
            int bfSi3 = (bfSize & 0xFF0000) / 0x10000;
            int bfSi4 = (int)((bfSize & 0xFF000000) / 0x1000000);

            int[] header = new int[]
            {
                66, 77, //тип файла
                bfSi1, bfSi2, bfSi3, bfSi4, //-размер файла
                0, 0, //зарезервировано
                0, 0, //зарезервировано
                54, 0, 0, 0, //смещение от начала до данных
                40, 0, 0, 0, //размер заголовка
                inpSi1, inpSi2, inpSi3, inpSi4, //-ширина
                inpSi1, inpSi2, inpSi3, inpSi4, //-высота
                1, 0, //кол-во цвет-х плоск-й
                24, 0, //бит/пиксель
                0, 0, 0, 0, //тип сжатия
                siOfCa1, siOfCa2, siOfCa3, siOfCa4, //-размер данных изображения

                0, 0, 0, 0, //"const"
                0, 0, 0, 0, //"const"
                0, 0, 0, 0, //"const"
                0, 0, 0, 0, //"const"
            };

            using (FileStream fstream = new FileStream(pathNewBMP, FileMode.Create))
            {
                byte[] buffer = new byte[header.Length]; //блок записи заголовка
                for (int i = 0; i < header.Length; i++)
                {
                    buffer[i] = Convert.ToByte(header[i]);
                    fstream.WriteByte(buffer[i]);
                }

                if (inputSize % 8 == 0)
                {
                    int multipleOfFour = (3 * inputSize) % 4;

                    for (int i = 1; i <= mainMatrix.Count; i++)
                    {
                        for (int j = 0; j < mainMatrix.Count; j++)
                        {
                            //Console.WriteLine(mainMatrix[i - 1][j]);
                            //fstream.WriteByte(Convert.ToByte(Math.Round(mainMatrix[i - 1][j])));
                            //fstream.WriteByte(Convert.ToByte(Math.Round(mainMatrix[i - 1][j])));
                            //fstream.WriteByte(Convert.ToByte(Math.Round(mainMatrix[i - 1][j])));

                            List<byte> threeBytes = ColorCalculation(mainMatrix[i - 1][j]);
                            fstream.WriteByte(threeBytes[2]);
                            fstream.WriteByte(threeBytes[1]);
                            fstream.WriteByte(threeBytes[0]);

                            //блок дописания байтов для кратности на 4
                            if ((i + j % (3 * inputSize) == 0) && (multipleOfFour != 0))
                            {
                                for (int k = 0; k < (4 - multipleOfFour); k++)
                                {
                                    fstream.WriteByte(0);
                                }
                            } //я не понимаю, нужен ли вообще этот блок...
                        }


                    }
                }
            }

            Console.WriteLine("Готова моя цветная картинка");
        }

        private List<byte> ColorCalculation(double num)
        {
            int numOfSteps = 16;
            num = (num - minMatrix) / (maxMatrix - minMatrix); //это значит, что можно подать не нормированную матрицу

            //List<double> step = new List<double>();
            //for (int i = 0; i < numOfSteps; i++)
            //{
            //    step[i] = (1.0 / numOfSteps) * (i + 1.0);
            //}

            //List<List<byte>> byteCol = new List<List<byte>>()
            //{
            //    new List<byte>{ 7, 1, 120 }, new List<byte>{ 10, 2, 172 }, new List<byte>{ 13, 66, 190 }, new List<byte>{ 10, 136, 190 }, new List<byte>{ 65, 211, 204 },
            //    new List<byte>{ 242, 190, 0 }, new List<byte>{ 244, 179, 0 },
            //    new List<byte>{ 23, 165, 5 }, new List<byte>{ 20, 142, 4 }, new List<byte>{ 56, 115, 34 }, new List<byte>{ 72,99 ,37  }, new List<byte>{91 ,99 , 59 },
            //    new List<byte>{ 99 , 99, 99 }, new List<byte>{ 132, 132, 132 }, new List<byte>{ 213, 213, 213 }, new List<byte>{ 255, 255, 255 }
            //};

            if (num >= 0 && (num < step[0])) //1 (от 0 до 1/numOfSteps)
            {
                //Console.WriteLine("вышли");
                return byteCol[0];
            }

            for (int i = 0; i < numOfSteps - 1; i++)
            {
                //Console.WriteLine(step[i]);
                if (num >= step[i] && num < step[i + 1]) // по порядку
                {
                    //Console.WriteLine("вышли");

                    return byteCol[i];
                }
            }

            if (num >= step[numOfSteps - 1] && num <= 1) //numOfSteps (от numOfSteps-1/numOfSteps до numOfSteps/numOfSteps)
            {
                //Console.WriteLine("вышли");

                return byteCol[numOfSteps - 1];
            }

            Console.WriteLine(num);
            Console.WriteLine($"min = {minMatrix}, max = {maxMatrix}");
            throw new Exception("Проблема с моим цветным \n");
        }


        private double FindMax(List<List<double>> matrix) //поиск максимального
        {
            double maxFromMatrix = 0;
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {

                    if (matrix[i][j] > maxFromMatrix)
                    {
                        maxFromMatrix = matrix[i][j]; //поиск максимума
                    }

                }
            }
            return maxFromMatrix;
        }

        private double FindMin(List<List<double>> matrix) //поиск минимального
        {
            double mixFromMatrix = matrix[0][0];

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {

                    if (matrix[i][j] < mixFromMatrix)
                    {
                        mixFromMatrix = matrix[i][j]; //поиск минимума
                    }

                }
            }
            return mixFromMatrix;
        }
    }
}
