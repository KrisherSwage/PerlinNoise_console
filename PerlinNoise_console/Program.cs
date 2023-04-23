using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double leftBor = 9; //левая граница для генерации случаных чисел
            double rightBor = 10.1; //правая граница для генерации случаных чисел
            int layerСount = 10; //количество "слоёв" и размер изображения (2^layerCount размер стороны)

            var mainNumArr = new NumericValueArray(rightBor, leftBor); //экземпляр класса

            var myDoneMatrix = mainNumArr.CompilationArray(layerСount);
            Console.WriteLine(myDoneMatrix.Count); //количество значений в столбце/строке (из-за квадратности)

            var crIm = new ImageСreation(); //экземпляр класса
            crIm.CreateImage(myDoneMatrix); //создание ЧБ изображения

            Console.WriteLine($"\nDone!");
            Console.ReadLine();
        }

        static public void ImageInConsole(List<List<double>> doneMatrix) //интерпретация изображения в консоль
        {
            var gradient = ".:!/r(l1Z4H9W8$@"; //16

            for (int i = 0; i < doneMatrix.Count; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < doneMatrix.Count; j++)
                {
                    //Console.Write($"{Math.Round(doneMatrix[i][j], 1)} ");

                    int lB = -16;
                    int rB = 0;
                    for (int h = 0; h < 16; h++)
                    {
                        lB += 16;
                        rB += 16;
                        if (doneMatrix[i][j] >= lB && doneMatrix[i][j] < rB)
                        {
                            Console.Write(gradient[h]); //отображение в консоль
                            break;
                        }
                    }
                }
            }

        }
    }
}
