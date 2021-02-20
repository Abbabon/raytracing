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
    }
}