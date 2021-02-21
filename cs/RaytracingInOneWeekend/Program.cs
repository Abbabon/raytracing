using System;
using System.IO;
using RayTracingInOneWeekend;

namespace RaytracingInOneWeekend
{
    static class Program
    {    
        static readonly Vec3 UnitVector = new Vec3(1, 1, 1);
        static readonly Vec3 ZeroVector = new Vec3(0, 0, 0);
        static readonly Vec3 Background = new Vec3(0.5, 0.7, 1);
        
        static Vec3 RayColor(Ray ray, HitableItems world)
        {
            var record = new HitRecord();
            if (world.Hit(ray, 0.001, double.MaxValue, ref record))
            {
                return 0.5 * (record.Normal + UnitVector);
            }
            
            var unitDirection = Vec3.UnitVector(ray.Direction);
            var t = 0.5 * (unitDirection.Y + 1);
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
                return (- half_b - Math.Sqrt(discriminant)) / (2.0 * a);
            }
        }

        static void Main()
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            var stderr = new StreamWriter(Console.OpenStandardError()) {AutoFlush = true};
            
            // Image:
            var aspectRatio = 16.0 / 9.0;
            var imageWidth = 400;
            var imageHeight = imageWidth / aspectRatio;
            var samplesPerPixel = 50;
            
            // World:
            var world = new HitableItems(new Hitable[]
            {
                new Sphere(new Vec3(0,0,-1), 0.5),
                new Sphere(new Vec3(0,-100.5,-1), 100)
            });
            
            // Camera: 

            var camera = new Camera();

            // Render:
            stdout.WriteLine($"P3\n{imageWidth} {imageHeight}\n255\n");

            for (int y = (int)imageHeight-1; y > 0; --y)
            {
                stderr.Write($"Scanlines remaining: {y} \r");
                for (int x = 0; x < imageWidth; x++)
                {
                    var pixelColor = new Vec3(0, 0, 0);
                    for (int sample = 0; sample < samplesPerPixel; sample++)
                    {
                        var u = (x + Utils.RandomDouble()) / (imageWidth-1);
                        var v = (y + Utils.RandomDouble()) / (imageHeight-1);
                        var ray = camera.GetRay(u, v);
                        var rayColor = RayColor(ray, world);
                        pixelColor += rayColor;
                    }
                    
                    WriteColor(stdout, pixelColor, samplesPerPixel);
                }    
            }

            static void WriteColor(StreamWriter stdout, Vec3 color, int samplesPerPixel)
            {
                var r = color.R;
                var g = color.G;
                var b = color.B;
                
                // Divide the color by the number of samples.
                var scale = 1.0 / samplesPerPixel;
                r *= scale;
                g *= scale;
                b *= scale;

                stdout.WriteLine(
                    $"{ComputePixel(r)} {ComputePixel(g)} {ComputePixel(b)}");
            }

            static int ComputePixel(double channelValue)
            {
                return (int) (256 * Math.Clamp(channelValue, 0, 0.999));
            }
            
            stdout.Close();
            stderr.Close();
        }
    }
}