using System;

namespace RaytracingInOneWeekend
{
    public class Camera
    {
        private Vec3 _lowerLeftCorner;
        private Vec3 _horizontalVector;
        private Vec3 _verticalVector;
        private Vec3 _origin;

        public Camera()
        {
            //move to config:
            var aspectRatio = 16.0 / 9.0;
            var focalLength = 1;
            var viewportHeight = 2.0;
            var viewportWidth = viewportHeight * aspectRatio;
            _origin = new Vec3(0, 0, 0);
            
            _horizontalVector = new Vec3(viewportWidth, 0, 0);
            _verticalVector = new Vec3(0, viewportHeight, 0);
            _lowerLeftCorner = _origin - _horizontalVector / 2.0 - _verticalVector / 2.0 - new Vec3(0, 0, focalLength);
        }

        public Ray GetRay(double u, double v)
        {
            return new(_origin, _lowerLeftCorner + u * _horizontalVector + v * _verticalVector - _origin);
        }
    }
}