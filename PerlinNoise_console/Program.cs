using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                int incfordiv = 101; //теперь не влияет
                bool flagManyPic = true;
                while (flagManyPic)
                {
                    //0 - черный - вода
                    //255 - белый - горы
                    Console.SetWindowPosition(0, 0);
                    var sw = new Stopwatch();
                    //sw.Start();

                    string fullName = "";

                    double lefttBor = /*0*/ 2.0; //левая граница для генерации случаных чисел
                    double rightBor = /*1*/ 3.0; //правая граница для генерации случаных чисел
                    int layerСount = 10; //количество "слоёв" и размер изображения (2^layerCount размер стороны)

                    fullName += $"LB-{lefttBor}, RB-{rightBor}, SZ-2^{layerСount}, ";

                    var mainNumArr = new NumericValueArray(rightBor, lefttBor, layerСount); //экземпляр класса с параметрами для конструктора
                    var myDoneMatrix = mainNumArr.CreateMatrix(incfordiv); //создаем!!!
                    //Console.WriteLine(myDoneMatrix.Count); //количество значений в столбце/строке (из-за квадратности)

                    fullName += mainNumArr.fullName; //имя из параметров при создании

                    //var crIm = new ImageСreation(); //экземпляр класса
                    //crIm.CreateImage(myDoneMatrix, fullName); //создание ЧБ изображения

                    //var colorImage = new ColorImage(); //уже не надо
                    //colorImage.CreateColorImage(myDoneMatrix);

                    var myColIm = new MyColorImage(myDoneMatrix);
                    myColIm.CreateImage(myDoneMatrix, fullName);

                    Console.WriteLine($"\nDone!");
                    //sw.Stop();
                    //Console.WriteLine(sw.Elapsed); // Здесь логируем
                    //Console.ReadKey();
                    incfordiv++;

                    flagManyPic = false;
                }
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