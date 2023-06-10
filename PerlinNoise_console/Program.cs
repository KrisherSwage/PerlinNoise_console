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
            try
            {
                //0 - черный - вода
                //255 - белый - горы
                double leftBor = 2.3; //левая граница для генерации случаных чисел
                double rightBor = 3.0; //правая граница для генерации случаных чисел
                int layerСount = 8; //количество "слоёв" и размер изображения (2^layerCount размер стороны)

                var mainNumArr = new NumericValueArray(rightBor, leftBor, layerСount); //экземпляр класса

                //var myDoneMatrix = mainNumArr.CompilationArray(layerСount);
                var myDoneMatrix = mainNumArr.CreateMatrix();
                Console.WriteLine(myDoneMatrix.Count); //количество значений в столбце/строке (из-за квадратности)

                var crIm = new ImageСreation(); //экземпляр класса
                crIm.CreateImage(myDoneMatrix); //создание ЧБ изображения

                var colorImage = new ColorImage();
                colorImage.CreateColorImage(myDoneMatrix);

                Console.WriteLine($"\nDone!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Чел, ну ты чел {ex}");
                Console.ReadKey();
            }
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
