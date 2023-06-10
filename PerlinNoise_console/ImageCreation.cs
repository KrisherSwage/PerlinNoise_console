using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class ImageСreation
    {
        public void CreateImage(List<List<double>> mainMatrix)
        {
            var inputSize = mainMatrix.Count /** mainMatrix.Count*/;
            //int inputSize = Convert.ToInt32(Console.ReadLine());

            //var pathNewBMP = args[1];
            Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\monochrome");
            var pathNewBMP = $@"{Directory.GetCurrentDirectory()}\monochrome\Noise.bmp";

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
                            fstream.WriteByte(Convert.ToByte(Math.Round(mainMatrix[i - 1][j])));
                            fstream.WriteByte(Convert.ToByte(Math.Round(mainMatrix[i - 1][j])));
                            fstream.WriteByte(Convert.ToByte(Math.Round(mainMatrix[i - 1][j])));

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

        }
    }
}