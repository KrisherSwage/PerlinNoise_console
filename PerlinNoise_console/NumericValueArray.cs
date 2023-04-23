using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class NumericValueArray
    {
        //0 - черный
        //255 - белый
        double leftBor = 0.0;
        double rightBor = 0.0;
        public NumericValueArray(double rb, double lb) //конструктор, для передачи параметров использовать создание экземпляра класса
        {
            leftBor = lb;
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

            for (int i = 0; i < sizeArr; i++)
            {
                for (int j = 0; j < sizeArr; j++)
                {
                    matrix[i][j] = rnd.NextDouble() * (rightBor - leftBor) + leftBor; //вот эта строка ответственна за общую гамму
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
                        mtxLarge[i * 2][j * 2] = mtxLarge[i * 2][j * 2] * mtxSmall[i][j];
                        mtxLarge[i * 2][j * 2 + 1] = mtxLarge[i * 2][j * 2 + 1] * mtxSmall[i][j];
                        mtxLarge[i * 2 + 1][j * 2] = mtxLarge[i * 2 + 1][j * 2] * mtxSmall[i][j];
                        mtxLarge[i * 2 + 1][j * 2 + 1] = mtxLarge[i * 2 + 1][j * 2 + 1] * mtxSmall[i][j];
                    }
                }

                nowGenSize *= 2;

                mtxSmall = mtxLarge;

                if (w != numsArrLayer - 1)
                    mtxLarge = ArrayInitialization(nowGenSize * 2);

            }

            Console.WriteLine("Compilation");
            //Console.WriteLine($"После создания {AverageNum(mtxSmall)}");

            //mtxSmall = ArrayDivMin(mtxSmall);
            mtxSmall = ArrayExponentiation(mtxSmall);
            mtxSmall = ArrayNormalization(mtxSmall);
            mtxSmall = ArrayBlurring(mtxSmall);

            return mtxSmall;
        }


        private List<List<double>> ArrayExponentiation(List<List<double>> matrix)
        {
            bool expon = true;

            if (expon == true)
            {
                for (int i = 0; i < matrix.Count; i++) //возведение в степень значений. При этом добавлении лучше менять левую границу
                {
                    for (int j = 0; j < matrix.Count; j++)
                    {
                        matrix[i][j] = Math.Pow(matrix[i][j], 2);
                    }
                }

                Console.WriteLine("Exp");
                //Console.WriteLine($"После возведения в степень {AverageNum(matrix)}");
            }

            return matrix;
        }

        private List<List<double>> ArrayBlurring(List<List<double>> doneMatrix) //сглаживание пикселей (дабавление градиента)
        {
            bool blur = true;
            bool blurVer = true;
            bool blurHor = true;

            if (blur == true) //нужно/не нужно сейчас сгладить изображение
            {
                if (blurVer == true) //обрабатываем по вертикале
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
                    Console.WriteLine("BlurVer");
                }

                if (blurHor == true) //обрабатываем по горизонтали
                {
                    for (int j = 1; j < doneMatrix.Count - 1; j++)
                    {
                        for (int i = 1; i < doneMatrix.Count - 1; i++)
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
                    Console.WriteLine("BlurHor");
                }

            }

            return doneMatrix;
        }

        #region badNormalization
        ////не совсем рабочий метод
        //public List<List<double>> ArrayNormalization(List<List<double>> doneMatrix) //нормализуем, чтобы впихнуть в графический файл
        //{
        //    //for (int i = 0; i < 1; i++) //чтобы несколько раз
        //    //{
        //    //    doneMatrix = ArrayBlurring(doneMatrix); //сглаживание
        //    //}

        //    double maxFromMatrix = FindMax(doneMatrix);

        //    double normalizationFactor = 255 / maxFromMatrix; //коэффициент для нормализации

        //    for (int i = 0; i < doneMatrix.Count; i++)
        //    {
        //        for (int j = 0; j < doneMatrix.Count; j++)
        //        {
        //            doneMatrix[i][j] = Math.Round(doneMatrix[i][j] * normalizationFactor, 2);
        //        }
        //    }

        //    Console.WriteLine("Normalization");
        //    Console.WriteLine($"После нормализации {AverageNum(doneMatrix)}");
        //    return doneMatrix;

        //    //не совсем рабочий метод
        //}
        #endregion

        public List<List<double>> ArrayNormalization(List<List<double>> matrix) //нормализуем, чтобы впихнуть в графический файл
        {
            double maxFromMatrix = FindMax(matrix);
            double minFrMatrix = FindMin(matrix);
            double delta = (maxFromMatrix - minFrMatrix) / 255.0; //хороший вопрос - не напутал ли я чего с общим диапазоном (0-255)
            double increasingDelta = minFrMatrix - delta;

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {

                    for (int k = 0; k < 256; k++) //каждое значение перебирается еще до 255 раз
                    {
                        increasingDelta += delta;
                        //уверен, есть и более эффективный алгоритм. Но пока так
                        //перебор чисел на принадлежность какому-то отрезку
                        if ((matrix[i][j] >= increasingDelta) && (matrix[i][j] <= increasingDelta + delta))
                        {
                            matrix[i][j] = k;
                            break;
                        }

                        if (k == 255)
                        {
                            Console.WriteLine("есть проблемка");
                        }
                    }

                    increasingDelta = minFrMatrix - delta;
                }
            }

            Console.WriteLine("Normalization");
            //Console.WriteLine($"После нормализации {AverageNum(matrix)}");
            return matrix;
        }

        public List<List<double>> ArrayDivMin(List<List<double>> doneMatrix) //разделим на минимум
        {
            //не знаю точно, пригодится ли это, но пусть будет
            double minFromMatrix = FindMin(doneMatrix);

            for (int i = 0; i < doneMatrix.Count; i++)
            {
                for (int j = 0; j < doneMatrix.Count; j++)
                {
                    doneMatrix[i][j] = Math.Round(doneMatrix[i][j] / minFromMatrix, 2);
                }
            }

            Console.WriteLine("Normalization");
            //Console.WriteLine($"После нормализации {AverageNum(doneMatrix)}");
            return doneMatrix;
        }


        private long AverageNum(List<List<double>> matrix) //среднее значение в массиве
        {
            long result = 0;
            long strSum = 0;

            for (int i = 0; i < matrix.Count; i++)
            {
                strSum = 0;
                for (int j = 0; j < matrix.Count; j++)
                {
                    strSum += Convert.ToInt64(Math.Round(matrix[i][j]));
                }
                strSum /= matrix.Count;
                result += strSum;
            }
            result /= matrix.Count;

            Console.WriteLine("Aver num");
            return result;
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
            Console.WriteLine("max");
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
            Console.WriteLine("min");
            return mixFromMatrix;
        }
    }
}
