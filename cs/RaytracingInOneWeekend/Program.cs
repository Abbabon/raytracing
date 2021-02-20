using System;
using System.IO;

namespace RaytracingInOneWeekend
{
    static class Program
    {    
        static readonly Vec3 UnitVector = new Vec3(1, 1, 1);
        static readonly Vec3 ZeroVector = new Vec3(0, 0, 0);
        static readonly Vec3 Background = new Vec3(0.5, 0.7, 1);
        
        static Vec3 RayColor(Ray ray)
        {
            var unitDirection = Vec3.UnitVector(ray.Direction);
            
            var t = 0.5 * (unitDirection.Y + 1);
            
            return (1.0 - t) * UnitVector + t * Background;
        }

        static void Main()
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            var stderr = new StreamWriter(Console.OpenStandardError()) {AutoFlush = true};
            
            //image:
            var aspectRatio = 16.0 / 9.0;
            var imageWidth = 400;
            var imageHeight = imageWidth / aspectRatio;
            
            //camera:
            
            var viewportHeight = 2.0f;
            var viewportWidth = aspectRatio * viewportHeight;
            var focalLength = 1.0;
            stderr.WriteLine($"{viewportWidth}, {viewportHeight}, {focalLength}");
            
            var origin = new Vec3(0, 0, 0);
            stderr.WriteLine($"{origin.X}, {origin.Y}, {origin.Z}");
            var horizontalVector = new Vec3(viewportWidth, 0, 0);
            stderr.WriteLine($"{horizontalVector.X}, {horizontalVector.Y}, {horizontalVector.Z}");
            var verticalVector = new Vec3(0, viewportHeight, 0);
            stderr.WriteLine($"{verticalVector.X}, {verticalVector.Y}, {verticalVector.Z}");
            var lowerLeftCorner = origin - horizontalVector/2.0 - verticalVector/2.0 - new Vec3(0, 0, focalLength);
            stderr.WriteLine($"{lowerLeftCorner.X}, {lowerLeftCorner.Y}, {lowerLeftCorner.Z}");
            
            //render:

            stdout.WriteLine($"P3\n{imageWidth} {imageHeight}\n255\n");

            for (int y = (int)imageHeight-1; y > 0; --y)
            {
                stderr.Write($"Scanlines remaining: {y} \r");
                for (int x = 0; x < imageWidth; x++)
                {
                    //// 1.0 gradients
                    // var r = (double) x / (imageWidth-1);
                    // var g = (double) y / (imageHeight-1);
                    // var b = 0.25f;
                    //
                    // var intR = (int) (255.99 * r);
                    // var intG = (int) (255.99 * g);
                    // var intB = (int) (255.99 * b);
                    
                    //stdout.WriteLine($"{intR} {intG} {intB}");
                    
                    // 2.0 ray gradients
                    var u = (double) x / (imageWidth-1);
                    var v = (double) y / (imageHeight-1);
                    var ray = new Ray(origin, lowerLeftCorner + u*horizontalVector + v*verticalVector - origin);
                    var color = RayColor(ray);
                    WriteColor(stdout, color);
                }    
            }

            static void WriteColor(StreamWriter stdout, Vec3 color)
            {
                var intR = (int) (255.99 * color.R);
                var intG = (int) (255.99 * color.G);
                var intB = (int) (255.99 * color.B);
                stdout.WriteLine($"{intR} {intG} {intB}");
            }
            
            stdout.Close();
            stderr.Close();
        }
    }
}