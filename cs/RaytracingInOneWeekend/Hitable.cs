using System;
using RaytracingInOneWeekend;

namespace RayTracingInOneWeekend
{
    struct HitRecord
    {
        public double T;
        public Vec3 PointOfIntersection;
        public Vec3 Normal;
        public bool FrontFace;

        public void SetFaceNormal(Ray ray, Vec3 outwardNormal)
        {
            FrontFace = Vec3.Dot(ray.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : outwardNormal * -1;
        }
    }

    abstract class Hitable
    {
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord record);
    }
    
    class HitableItems : Hitable
    {
        readonly Hitable[] _hitables;

        public HitableItems(Hitable[] hitables)
        {
            _hitables = hitables;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord record)
        {
            var hitAnything = false;
            var closestSoFar = tMax;

            foreach (var hitable in _hitables)
            {
                if (!hitable.Hit(ray, tMin, closestSoFar, ref record))
                    continue;
                
                hitAnything = true;
                closestSoFar = record.T;
            }

            return hitAnything;
        }
    }
    
    class Sphere : Hitable
    {
        readonly Vec3 _center;
        readonly double _radius;

        public Sphere(Vec3 center, double radius)
        {
            _center = center;
            _radius = radius;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord record)
        {
            var originMinusCenter = ray.Origin - _center;
            var a = Vec3.Dot(ray.Direction, ray.Direction);
            var b = Vec3.Dot(originMinusCenter, ray.Direction);
            var c = Vec3.Dot(originMinusCenter, originMinusCenter) - _radius * _radius;
            var discriminant = b * b - a * c;
            
            if (discriminant < 0)
            {
                return false;
            }
                
            var sqrtDiscriminant = Math.Sqrt(discriminant);
            var root = (-b - sqrtDiscriminant) / a;
            if (root < tMin || tMax < root)
            {
                return false;
            }

            record.T = root;
            record.PointOfIntersection = ray.PointAt(record.T);
            Vec3 outwardNormal = (record.PointOfIntersection - _center) / _radius;
            record.SetFaceNormal(ray, outwardNormal);
            
            return true;
        }
    }
}