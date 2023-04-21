using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class NumericValueArray
    {
        double leftBor = 2.25;
        double rightBor = 3.0;
        public NumericValueArray(double rb) //конструктор, для передачи параметров использовать создание экземпляра класса
        {
            rightBor = rb;
        }

        static Random rnd = new Random();

        private List<List<double>> ArrayInitialization(int sizeArr)
        {
            List<List<double>> matrix = new List<List<double>>();// начало кода создания двумерного динамического массива
            for (int i = 0; i < sizeArr; i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < sizeArr; j++)
                {
                    row.Add(0);  //определение длины вторичного списка                                         
                }

                matrix.Add(row); //определение длины списка списков (главного)
            }
            //matrix[columns][lines]

            //чем больше "слоёв", тем больше плавность и размер
            //1000*1000 - уже норм, но чтобы с запасом...
            //2^10 = 1024... 11 = 2048... 12 = 4096
            //пока что 12 слооев сделать верхней границей

            //ЗНАЧИТ, что перемножений всего  штук

            for (int i = 0; i < sizeArr; i++)
            {
                for (int j = 0; j < sizeArr; j++)
                {
                    matrix[i][j] = rnd.NextDouble() * (rightBor - leftBor) + leftBor;
                    //3^11 = 177147
                }
            }

            return matrix;
        }

        public List<List<double>> CompilationArray(int numsArrLayer) //основная математика по образованию шума
        {
            int nowGenSize = 2;
            var mtxSmall = ArrayInitialization(nowGenSize); //через генерирование один раз создается маленький
            var mtxLarge = ArrayInitialization(nowGenSize * 2);

            Console.WriteLine(mtxSmall.Count);
            Console.WriteLine(mtxLarge.Count);

            for (int w = 1; w < numsArrLayer; w++)
            {



                for (int i = 0; i < nowGenSize; i++) //перемножение чисел
                {
                    for (int j = 0; j < nowGenSize; j++) //перебор придумал сам
                    {
                        //Console.WriteLine($"i-{i * 2} j-{j * 2} ngs-{nowGenSize}");
                        //Console.WriteLine(mtxLarge[i * 2][j * 2]);
                        mtxLarge[i * 2][j * 2] = mtxLarge[i * 2][j * 2] * mtxSmall[i][j];
                        mtxLarge[i * 2][j * 2 + 1] = mtxLarge[i * 2][j * 2 + 1] * mtxSmall[i][j];
                        mtxLarge[i * 2 + 1][j * 2] = mtxLarge[i * 2 + 1][j * 2] * mtxSmall[i][j];
                        mtxLarge[i * 2 + 1][j * 2 + 1] = mtxLarge[i * 2 + 1][j * 2 + 1] * mtxSmall[i][j];
                    }
                }

                nowGenSize *= 2;

                mtxSmall = mtxLarge;

                //Console.WriteLine(nowGenSize);
                if (w != numsArrLayer - 1)
                    mtxLarge = ArrayInitialization(nowGenSize * 2);
                //Console.WriteLine($"S-{mtxSmall.Count} L-{mtxLarge.Count}");
            }

            for (int i = 0; i < mtxSmall.Count; i++) //возведение в квадрат. При этом добавлении лучше менять левую границу
            {
                for (int j = 0; j < mtxSmall.Count; j++)
                {
                    mtxSmall[i][j] = mtxSmall[i][j] * mtxSmall[i][j];

                }
            }


            return mtxSmall;
        }

        public List<List<double>> ArrayInterpolation(List<List<double>> doneMatrix) //сглаживание пикселей (дабавление градиента)
        {


            for (int i = 1; i < doneMatrix.Count - 1; i++)
            {
                for (int j = 1; j < doneMatrix.Count - 1; j++)
                {
                    double sumOfNine =
                        doneMatrix[i][j] +
                        doneMatrix[i - 1][j - 1] +
                        doneMatrix[i - 1][j] +
                        doneMatrix[i][j - 1] +
                        doneMatrix[i - 1][j + 1] +
                        doneMatrix[i + 1][j - 1] +
                        doneMatrix[i + 1][j] +
                        doneMatrix[i][j + 1] +
                        doneMatrix[i + 1][j + 1];

                    double averageOfNine = sumOfNine / 9;

                    doneMatrix[i][j] = averageOfNine;
                    doneMatrix[i - 1][j - 1] = averageOfNine;
                    doneMatrix[i - 1][j] = averageOfNine;
                    doneMatrix[i][j - 1] = averageOfNine;
                    doneMatrix[i - 1][j + 1] = averageOfNine;
                    doneMatrix[i + 1][j - 1] = averageOfNine;
                    doneMatrix[i + 1][j] = averageOfNine;
                    doneMatrix[i][j + 1] = averageOfNine;
                    doneMatrix[i + 1][j + 1] = averageOfNine;

                }
            }


            return doneMatrix;
        }

        public List<List<double>> ArrayNormalization(List<List<double>> doneMatrix) //нормализуем, чтобы впихнуть в графический файл
        {
            for (int i = 0; i < 1; i++)
            {
                doneMatrix = ArrayInterpolation(doneMatrix); //сглаживание

            }


            double maxFromMatrix = 0;
            for (int i = 0; i < doneMatrix.Count; i++)
            {
                for (int j = 0; j < doneMatrix.Count; j++)
                {
                    //Console.Write($"{Math.Round(notNormList[i][j], 1)} ");

                    //doneList[i][j] = Math.Round(doneList[i][j] / teorMax, 2);
                    if (doneMatrix[i][j] > maxFromMatrix)
                    {
                        maxFromMatrix = doneMatrix[i][j];
                    }

                }
            }

            double normalizationFactor = 255 / maxFromMatrix;

            for (int i = 0; i < doneMatrix.Count; i++)
            {
                for (int j = 0; j < doneMatrix.Count; j++)
                {
                    doneMatrix[i][j] = Math.Round(doneMatrix[i][j] * normalizationFactor, 2);
                }
            }

            return doneMatrix;
        }
    }
}
