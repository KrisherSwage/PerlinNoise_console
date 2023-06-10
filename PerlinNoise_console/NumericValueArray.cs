using Aspose.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class NumericValueArray
    {
        //0 - черный - вода
        //255 - белый - горы
        double leftBor = 0.0;
        double rightBor = 0.0;
        int numLay = 0;

        static Random rnd = new Random(); //900 для одинаковости во время разработки

        List<List<double>> matrixAll = new List<List<double>>();
        List<List<double>> rawFullMatrix = new List<List<double>>();

        public NumericValueArray(double rb, double lb, int nL) //конструктор, для передачи параметров использовать создание экземпляра класса
        {
            leftBor = lb;
            rightBor = rb;
            numLay = nL;
        }

        public List<List<double>> CreateMatrix() //управление различными преобразованиями
        {
            matrixAll = CompilationArray(numLay);

            //matrixAll = ArrayRemainderOfDivision(matrixAll, 255);
            //matrixAll = ArrayBlurring(matrixAll);

            //matrixAll = ArrayExponentiation(matrixAll, 4);
            matrixAll = ArrayRemainderOfDivision(matrixAll, 255);
            //matrixAll = ArrayBlurring(matrixAll);
            //matrixAll = ArrayBlurring(matrixAll);
            ///////matrixAll = ArrayBlurring(matrixAll);


            matrixAll = ArrayExponentiation(matrixAll, 6);
            //matrixAll = ArrayMultiplArray(matrixAll, rawFullMatrix);
            //matrixAll = ArrayExponentiation(matrixAll, 4);


            //matrixAll = ArrayInterpolation(matrixAll);
            //matrixAll = ArrayNormalization(matrixAll);
            matrixAll = ArrayDifferentMathTest(matrixAll);

            //matrixAll = ArrayBlurring(matrixAll);
            //matrixAll = ArrayBlurring(matrixAll);

            matrixAll = ArrayBlurring(matrixAll);
            matrixAll = ArrayNormalization(matrixAll); //нормализацию после всего остального!!!

            return matrixAll;
        }




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

            rawFullMatrix = mtxSmall;
            Console.WriteLine($"\nMaxRaw = {FindMax(mtxSmall)}; MinRaw = {FindMin(mtxSmall)}\n");
            //Console.WriteLine($"% 255 = {FindMax(mtxSmall) % 255}  {FindMin(mtxSmall) % 255}\n");
            if (FindMax(mtxSmall) > Math.Pow(rightBor, numLay))
            {
                throw new Exception("больше возведения в степень");
            }
            Console.WriteLine("Compilation");
            ////Console.WriteLine($"После создания {AverageNum(mtxSmall)}");

            return mtxSmall;
        }

        private List<List<double>> ArrayDifferentMathTest(List<List<double>> matrix)
        {
            double maxFromMatrix = FindMax(matrix);
            double minFrMatrix = FindMin(matrix);
            double delta = (maxFromMatrix - minFrMatrix) / 100.0; //хороший вопрос - не напутал ли я чего с общим диапазоном (0-255)

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    if (matrix[i][j] >= maxFromMatrix - delta * 90)
                    {
                        matrix[i][j] *= matrix[i][j];
                    }
                }
            }
            Console.WriteLine("Math");
            return matrix;
        }

        private List<List<double>> ArrayMultiplArray(List<List<double>> matrixDone, List<List<double>> matrixRaw) //a*b
        {
            //как будто, мало что даёт
            for (int i = 0; i < matrixRaw.Count; i++)
            {
                for (int j = 0; j < matrixRaw.Count; j++)
                {
                    matrixDone[i][j] = matrixRaw[i][j] * matrixDone[i][j];
                }
            }
            Console.WriteLine("Multip 2 arr");
            return matrixDone;
        }

        private List<List<double>> ArrayExponentiation(List<List<double>> matrix, double exp) //a^b
        {
            bool expon = true;

            if (expon == true)
            {
                for (int i = 0; i < matrix.Count; i++) //возведение в степень значений
                {
                    for (int j = 0; j < matrix.Count; j++)
                    {
                        matrix[i][j] = Math.Pow(matrix[i][j], exp);
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
            bool blurBorders = true;

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

                if (blurBorders == true) //обработка границ
                {
                    int len = doneMatrix.Count;
                    //doneMatrix[len][len] = 0;

                    for (int i = 1; i < doneMatrix.Count - 1; i++)
                    {
                        //верхняя
                        double sumOfNine =
                                doneMatrix[i][0] +
                                doneMatrix[i - 1][0] +
                                doneMatrix[i - 1][1] +
                                doneMatrix[i + 1][0] +
                                doneMatrix[i][1] +
                                doneMatrix[i + 1][1];

                        double averageOfNine = sumOfNine / 6;

                        doneMatrix[i][0] = averageOfNine;
                        doneMatrix[i - 1][0] = averageOfNine;
                        doneMatrix[i - 1][1] = averageOfNine;
                        doneMatrix[i + 1][0] = averageOfNine;
                        doneMatrix[i][1] = averageOfNine;
                        doneMatrix[i + 1][1] = averageOfNine;

                        //нижняя
                        sumOfNine =
                                doneMatrix[i][len - 1] +
                                doneMatrix[i - 1][len - 2] +
                                doneMatrix[i - 1][len - 1] +
                                doneMatrix[i][len - 2] +
                                doneMatrix[i + 1][len - 2] +
                                doneMatrix[i + 1][len - 1];

                        averageOfNine = sumOfNine / 6;

                        doneMatrix[i][len - 1] = averageOfNine;
                        doneMatrix[i - 1][len - 2] = averageOfNine;
                        doneMatrix[i - 1][len - 1] = averageOfNine;
                        doneMatrix[i][len - 2] = averageOfNine;
                        doneMatrix[i + 1][len - 2] = averageOfNine;
                        doneMatrix[i + 1][len - 1] = averageOfNine;

                        //левая
                        sumOfNine =
                                doneMatrix[0][i] +
                                doneMatrix[0][i - 1] +
                                doneMatrix[1][i - 1] +
                                doneMatrix[1][i] +
                                doneMatrix[0][i + 1] +
                                doneMatrix[1][i + 1];

                        averageOfNine = sumOfNine / 6;

                        doneMatrix[0][i] = averageOfNine;
                        doneMatrix[0][i - 1] = averageOfNine;
                        doneMatrix[1][i - 1] = averageOfNine;
                        doneMatrix[1][i] = averageOfNine;
                        doneMatrix[0][i + 1] = averageOfNine;
                        doneMatrix[1][i + 1] = averageOfNine;

                        //правая
                        sumOfNine =
                                doneMatrix[len - 1][i] +
                                doneMatrix[len - 2][i - 1] +
                                doneMatrix[len - 2][i] +
                                doneMatrix[len - 1][i - 1] +
                                doneMatrix[len - 2][i + 1] +
                                doneMatrix[len - 1][i + 1];

                        averageOfNine = sumOfNine / 6;

                        doneMatrix[len - 1][i] = averageOfNine;
                        doneMatrix[len - 2][i - 1] = averageOfNine;
                        doneMatrix[len - 2][i] = averageOfNine;
                        doneMatrix[len - 1][i - 1] = averageOfNine;
                        doneMatrix[len - 2][i + 1] = averageOfNine;
                        doneMatrix[len - 1][i + 1] = averageOfNine;

                    }


                }

            }

            return doneMatrix;
        }

        private List<List<double>> ArrayNormalization(List<List<double>> matrix) //нормализуем, чтобы впихнуть в графический файл
        {
            double maxFromMatrix = FindMax(matrix);
            double minFrMatrix = FindMin(matrix);
            Console.WriteLine($"Мин до норм = {minFrMatrix}. Макс до норм = {maxFromMatrix}");
            double delta = (maxFromMatrix - minFrMatrix) / 255.0; //хороший вопрос - не напутал ли я чего с общим диапазоном (0-255)


            double increasingDelta = minFrMatrix/* - delta*/;
            Console.WriteLine($"Increm del {Math.Ceiling(increasingDelta)}");

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    for (int k = 0; k < 256; k++) //каждое значение перебирается еще до 255 раз
                    {
                        //уверен, есть и более эффективный алгоритм. Но пока так
                        //перебор чисел на принадлежность какому-то промежутку
                        if ((matrix[i][j] >= Math.Floor(increasingDelta)) && (matrix[i][j] < Math.Ceiling(increasingDelta + delta)))
                        {
                            increasingDelta += delta;
                            matrix[i][j] = k;
                            break;
                        }

                        if (k == 255)
                        {
                            //Console.WriteLine($"{Math.Floor(increasingDelta)} против {matrix[i][j]} против {Math.Ceiling(increasingDelta + delta)}");
                            Console.WriteLine("есть проблемка");

                            if ((matrix[i][j] >= maxFromMatrix - delta) && (matrix[i][j] <= maxFromMatrix))
                            {
                                matrix[i][j] = k;
                                break;
                            }

                            k -= 1;
                        }

                        increasingDelta += delta;
                    }

                    increasingDelta = minFrMatrix - delta;
                }
                increasingDelta = minFrMatrix - delta;
            }

            Console.WriteLine($"\nMaxNorm = {FindMax(matrix)}; MinNorm = {FindMin(matrix)}\n");
            if (FindMax(matrix) == 0)
            {
                throw new Exception("меньше 255\n");
            }
            //Console.WriteLine($"После нормализации {AverageNum(matrix)}");
            Console.WriteLine("Normalization");
            return matrix;
        }

        private List<List<double>> ArrayRemainderOfDivision(List<List<double>> matrix, int dentor) //нормализуем, чтобы впихнуть в графический файл
        {
            //просто не нравятся некоротые числа

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    matrix[i][j] = Math.Round(matrix[i][j]) % dentor; //как альтернатива
                }
            }

            Console.WriteLine("Rem of div");
            return matrix;
        }

        private List<List<double>> ArrayDivMin(List<List<double>> matrix) //разделим на минимум
        {
            //не знаю точно, пригодится ли это, но пусть будет
            double minFromMatrix = FindMin(matrix);

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    matrix[i][j] = Math.Round(matrix[i][j] / minFromMatrix, 2);
                }
            }

            Console.WriteLine("DivMin");
            //Console.WriteLine($"После нормализации {AverageNum(doneMatrix)}");
            return matrix;
        }

        private List<List<double>> ArrayInterpolation(List<List<double>> matrix) //если я правильно понял статью с википедии... но...
        {
            double kX1 = 0.5;
            double kX2 = 0.5;
            double kY1 = 0.5;
            double kY2 = 0.5;

            for (int i = 1; i < matrix.Count - 1; i++)
            {
                for (int j = 1; j < matrix.Count - 1; j++)
                {
                    //double kX1 = ((i + 1) - (i)) / ((i + 1) - (i - 1.0));
                    //double kX2 = (i - (i - 1)) / ((i + 1) - (i - 1.0));
                    //double kY1 = ((j + 1) - (j)) / ((j + 1) - (j - 1.0));
                    //double kY2 = (j - (j - 1)) / ((j + 1) - (j - 1.0));
                    //но в данном случае они все равны 0,5...
                    //.0 для единицы... почему это надо ставить, хотя тип double

                    double r1 = kX1 * matrix[i - 1][j] + kX2 * matrix[i + 1][j];
                    double r2 = kX1 * matrix[i][j - 1] + kX2 * matrix[i][j + 1];

                    double interp = kY1 * r1 + kY2 * r2;

                    matrix[i][j] = interp;
                }
            }

            return matrix;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------//
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
            //Console.WriteLine("max");
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
            //Console.WriteLine("min");
            return mixFromMatrix;
        }
    }
}
