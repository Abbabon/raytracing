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
            var sphereCenter = new Vec3(0, 0, -1);
            var t = HitSphere(sphereCenter, 0.5, ray);
            if (t > 0)
            {
                var N = Vec3.UnitVector(ray.PointAt(t) - sphereCenter);
                return 0.5 * new Vec3(N.X+1, N.Y+1, N.Z+1);
            }
            
            var unitDirection = Vec3.UnitVector(ray.Direction);
            t = 0.5 * (unitDirection.Y + 1);
            return (1.0 - t) * UnitVector + t * Background;
        }

        private static double HitSphere(Vec3 sphereCenter, double radius, Ray ray)
        {
            Vec3 originMinusCenter = ray.Origin - sphereCenter;
            var a = ray.Direction.SquaredLength();
            var half_b = Vec3.Dot(originMinusCenter, ray.Direction);
            var c = originMinusCenter.SquaredLength() - radius*radius;
            
            var discriminant = half_b * half_b - a * c;
            if (discriminant < 0)
            {
                return -1.0;
            }
            else
            {
                return (-b - Math.Sqrt(discriminant)) / (2.0 * a);
            }
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
            var origin = new Vec3(0, 0, 0);
            var horizontalVector = new Vec3(viewportWidth, 0, 0);
            var verticalVector = new Vec3(0, viewportHeight, 0);
            var lowerLeftCorner = origin - horizontalVector/2.0 - verticalVector/2.0 - new Vec3(0, 0, focalLength);
            
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