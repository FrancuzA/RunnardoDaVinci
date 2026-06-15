using System;
public static class RNG_Custom
{
    public static Random random;

    public static void Init(int seed = -1)
    {
        if (seed == -1)
            seed = Environment.TickCount;

        random = new Random(seed);
    }

    private static void EnsureInitialized()
    {
        if (random == null)
            Init();
    }

    public static float NextFloat(float minValue, float maxValue)
    {
        EnsureInitialized();
        float difference = maxValue - minValue;
        double next = random.NextDouble();
        return (float)(minValue + difference * next);
    }

    public static int NextInt(int minValue, int maxValue)
    {
        EnsureInitialized();
        return random.Next(minValue, maxValue);
    }
}