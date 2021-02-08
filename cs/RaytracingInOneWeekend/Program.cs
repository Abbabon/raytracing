using System;
using System.IO;

namespace RaytracingInOneWeekend
{
    static class Program
    {
        static void Main()
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            var stderr = new StreamWriter(Console.OpenStandardError()) {AutoFlush = true};
            const int imageWidth = 256;
            const int imageHeight = 256;

            stdout.WriteLine($"P3\n{imageWidth} {imageHeight}\n255\n");

            for (int y = 0; y < imageHeight; y++)
            {
                stderr.Write($"Scanlines remaining: {y} \r");
                for (int x = 0; x < imageWidth; x++)
                {
                    var r = (double) x / (imageWidth-1);
                    var g = (double) y / (imageHeight-1);
                    var b = 0.25f;

                    var intR = (int) (255.99 * r);
                    var intG = (int) (255.99 * g);
                    var intB = (int) (255.99 * b);
                    
                    stdout.WriteLine($"{intR} {intG} {intB}");
                }    
            }
            
            stdout.Close();
            stderr.Close();
        }
    }
}