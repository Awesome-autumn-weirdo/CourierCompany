using System;

public static class RandomUtils
{
    private static readonly Random Random = new Random();

    public static double GenerateUniformRandom(double min, double max)
        => Random.NextDouble() * (max - min) + min;

    public static double GenerateExponentialRandom(double lambda)
    {
        double u = Random.NextDouble();
        return -Math.Log(1 - u) / lambda;
    }

    public static double GenerateNormalRandom(double mean, double stdDev)
    {
        double u1 = Random.NextDouble();
        double u2 = Random.NextDouble();
        double z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
        return mean + stdDev * z;
    }
}
