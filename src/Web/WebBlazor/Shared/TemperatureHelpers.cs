using System;

namespace WebBlazor.Shared
{
    public static class TemperatureHelpers
    {
        public static double? ToCelsius(double? farenheit)
        {
            if (!farenheit.HasValue)
            {
                return null;
            }

            return Math.Round((double) (farenheit - 32) * 5 / 9);
        }
    }
}