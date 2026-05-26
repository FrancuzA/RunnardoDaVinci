using System;
public static class RNG_Custom
{
    public static Random random;

    public static void Init(int seed = -1)
    {
        if (seed == -1)
            seed = DateTime.Now.Millisecond;

        random = new Random(seed);
    }

    public static float NextFloat(float minValue, float maxValue)
    {
        float difference = maxValue - minValue;

        double next = random.NextDouble();

        return (float)(minValue + difference * next);
    }

    public static int NextInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }
}