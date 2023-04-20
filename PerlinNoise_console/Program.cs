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
            int rightBor = 3;
            int layerСount = 12;
            var mainNumArr = new NumericValueArray(Convert.ToDouble(rightBor));

            var myDoneMatrix = mainNumArr.CompilationArray(layerСount);
            Console.WriteLine(myDoneMatrix.Count); //количество значений в столбце/строке (из-за квадратности)

            //for (int i = 0; i < myDoneMatrix.Count; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < myDoneMatrix.Count; j++)
            //    {
            //        Console.Write($"{Math.Round(myDoneMatrix[i][j], 1)} ");
            //    }
            //}

            var myNormDoneMatrix = mainNumArr.ArrayNormalization(myDoneMatrix, layerСount);
            //for (int i = 0; i < myNormDoneMatrix.Count; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < myNormDoneMatrix.Count; j++)
            //    {
            //        Console.Write($"{Math.Round(myNormDoneMatrix[i][j], 1)} ");
            //    }
            //}


            //ImageInConsole(myNormDoneMatrix);

            var crIm = new ImageСreation();
            crIm.CreateImage(myNormDoneMatrix);

            Console.ReadLine();
        }

        static public void ImageInConsole(List<List<double>> doneMatrix) //подавать нормализованную матрицу
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
