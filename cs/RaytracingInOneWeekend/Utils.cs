using System;

namespace RaytracingInOneWeekend
{
    public static class Utils
    {
        //180deg == pi*1rad ==> 1deg == pi/180 * 1rad 
        public static double DegressToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }
        
        static readonly Random Randomizer = new Random();

        public static double RandomDouble()
        {
            // Returns a random real in [0,1).
            return Randomizer.NextDouble();
        }
        
        public static double RandomDouble(double min, double max)
        {
            // Returns a random real in [0,1).
            return min + (max - min) * RandomDouble();
        }
    }
}