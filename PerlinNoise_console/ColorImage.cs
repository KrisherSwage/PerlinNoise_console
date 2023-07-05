using Aspose;
using Aspose.Imaging;
using Aspose.Imaging.ImageOptions;
using Aspose.Imaging.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise_console
{
    internal class ColorImage
    {

        public void CreateColorImage(List<List<double>> mainMatrix)
        {
            var inputSize = mainMatrix.Count;
            //int inputSize = Convert.ToInt32(Console.ReadLine());

            Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\colored");
            var pathNewBMP = $@"{Directory.GetCurrentDirectory()}\colored\";



            BmpOptions ImageOptions = new BmpOptions();
            ImageOptions.BitsPerPixel = 24;

            //Создайте экземпляр FileCreateSource и присвойте его свойству Source
            ImageOptions.Source = new FileCreateSource(pathNewBMP + "ColoredNoise.bmp", false);

            //Создайте экземпляр RasterImage и получите пиксели изображения, указав область как границу изображения
            using (RasterImage rasterImage = (RasterImage)Image.Create(ImageOptions, inputSize, inputSize))
            {
                Color[] pixels = rasterImage.LoadPixels(rasterImage.Bounds);
                int iterator = 0;


                for (int i = 0; i < mainMatrix.Count; i++)
                {
                    for (int j = 0; j < mainMatrix.Count; j++)
                    {
                        int colorPix = Convert.ToInt32(mainMatrix[i][j]);

                        //pixels[iterator] = Color.FromArgb(colorPix, colorPix, colorPix); //red, green, blue

                        //if (colorPix >= 0 && colorPix < 32) //1
                        //    pixels[iterator] = Color.DarkBlue;
                        //if (colorPix >= 32 && colorPix < 50) //2
                        //    pixels[iterator] = Color.Blue;
                        //if (colorPix >= 50 && colorPix < 64) //3
                        //    pixels[iterator] = Color.DeepSkyBlue;
                        //if (colorPix >= 64 && colorPix < 84) //4
                        //    pixels[iterator] = Color.SandyBrown;
                        //if (colorPix >= 84 && colorPix < 120) //5
                        //    pixels[iterator] = Color.Green;
                        //if (colorPix >= 120 && colorPix < 145) //6
                        //    pixels[iterator] = Color.Olive;
                        //if (colorPix >= 145 && colorPix < 160) //7
                        //    pixels[iterator] = Color.ForestGreen;
                        //if (colorPix >= 160 && colorPix < 170) //8
                        //    pixels[iterator] = Color.DarkOliveGreen;
                        //if (colorPix >= 170 && colorPix < 224) //9
                        //    pixels[iterator] = Color.Gray;
                        //if (colorPix >= 224 && colorPix < 240) //10
                        //    pixels[iterator] = Color.DarkGray;
                        //if (colorPix >= 240 && colorPix <= 255) //11
                        //    pixels[iterator] = Color.White;

                        if (colorPix >= 0 && colorPix < 22)
                            pixels[iterator] = Color.FromArgb(7, 1, 120); //0
                        if (colorPix >= 22 && colorPix < 45)
                            pixels[iterator] = Color.FromArgb(10, 2, 172); //1
                        if (colorPix >= 45 && colorPix < 66)
                            pixels[iterator] = Color.FromArgb(13, 66, 190); //2
                        if (colorPix >= 66 && colorPix < 89)
                            pixels[iterator] = Color.FromArgb(65, 211, 204); //3 //вода

                        if (colorPix >= 89 && colorPix < 96)
                            pixels[iterator] = Color.FromArgb(242, 190, 0); //4
                        if (colorPix >= 96 && colorPix < 102)
                            pixels[iterator] = Color.FromArgb(244, 179, 0); //5 //пляж

                        if (colorPix >= 102 && colorPix < 130)
                            pixels[iterator] = Color.FromArgb(23, 165, 5); //6
                        if (colorPix >= 130 && colorPix < 159)
                            pixels[iterator] = Color.FromArgb(20, 142, 4); //7
                        if (colorPix >= 159 && colorPix < 188)
                            pixels[iterator] = Color.FromArgb(56, 115, 34); //8
                        if (colorPix >= 188 && colorPix < 216)
                            pixels[iterator] = Color.FromArgb(72, 99, 37); //9
                        if (colorPix >= 216 && colorPix < 229)
                            pixels[iterator] = Color.FromArgb(91, 99, 59); //10 //земля

                        if (colorPix >= 229 && colorPix < 235)
                            pixels[iterator] = Color.FromArgb(99, 99, 99); //11
                        if (colorPix >= 235 && colorPix < 242)
                            pixels[iterator] = Color.FromArgb(132, 132, 132); //12
                        if (colorPix >= 242 && colorPix < 248)
                            pixels[iterator] = Color.FromArgb(213, 213, 213); //13
                        if (colorPix >= 248 && colorPix <= 255)
                            pixels[iterator] = Color.FromArgb(255, 255, 255); //14

                        iterator++;
                    }
                }

                //Примените изменения пикселей к изображению и сохраните все изменения
                rasterImage.SavePixels(rasterImage.Bounds, pixels);
                rasterImage.Save();
            }

            Console.WriteLine("Цветная картинка");
        }
    }
}