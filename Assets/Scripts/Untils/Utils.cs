using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    private const string randomString = "0123456789abcdefghijklmnopqrstuvwxyz";
    public static string GetRandomString(int length)
    {
        string str = "";

        for (int i = 0; i < length; i++)
        {
            str += randomString[(UnityEngine.Random.Range(0, randomString.Length))];
        }
        return str;
    }
    public static long TimeStemp(DateTime time)
    {
        long unixTimestamp = (long)(time.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        return unixTimestamp;
    }
    public static string TimeString(long time)
    {
        string s = getT(time % 60);
        string m = getT(time / 60);
        return $"{m}:{s}";
    }
    private static string getT(long number)
    {
        if (number < 0)
            return "00";
        if (number < 10)
            return $"0{number}";
        else
            return $"{number}";
    }
    public static string FormatNumber1(long num)
    {
        if (num >= 1000000000000)
        {
            return (num / 1000000000000D).ToString("0.L");
        }
        if (num >= 1000000000)
        {
            return (num / 1000000000D).ToString("0.B");
        }
        if (num >= 1000000)
        {
            return (num / 1000000D).ToString("0.M");
        }
        if (num >= 10000)
        {
            return (num / 1000D).ToString("0.K");
        }

        return num.ToString("0.");
    }
    public static string FormatNumber(long num)
    {
        if (num >= 1000000000000)
        {
            return (num / 1000000000000D).ToString("0.#L");
        }
        if (num >= 1000000000)
        {
            return (num / 1000000000D).ToString("0.#B");
        }
        if (num >= 100000000)
        {
            return (num / 1000000D).ToString("0.#M");
        }
        if (num >= 1000000)
        {
            return (num / 1000000D).ToString("0.#M");
        }
        if (num >= 100000)
        {
            return (num / 1000D).ToString("0.#K");
        }
        if (num >= 10000)
        {
            return (num / 1000D).ToString("0.#K");
        }

        return num.ToString("0.#");
    }
    public static Color GetColor(MapType mapType, long amount)
    {
        switch (mapType)
        {
            case MapType.Nome:
                {
                    switch (amount)
                    {
                        case 2:
                            return new Color32(255, 255, 255, 255);
                        case 4:
                            return new Color32(255, 255, 255, 255);
                        case 8:
                            return new Color32(255, 255, 255, 255);
                        case 16:
                            return new Color32(255, 255, 255, 255);
                        case 32:
                            return new Color32(255, 255, 255, 255);
                        case 64:
                            return new Color32(255, 255, 255, 255);
                        case 128:
                            return new Color32(255, 255, 255, 255);
                        case 256:
                            return new Color32(255, 255, 255, 255);
                        case 512:
                            return new Color32(255, 255, 255, 255);
                        case 1024:
                            return new Color32(255, 255, 255, 255);
                        case 2048:
                            return new Color32(255, 255, 255, 255);
                        case 4096:
                            return new Color32(255, 255, 255, 255);
                        case 8192:
                            return new Color32(255, 255, 255, 255);
                        default:
                            return new Color32(255, 255, 255, 255);
                    }
                }
            case MapType.Light:
                {
                    switch (amount)
                    {
                        case 2:
                            return new Color32(255, 255, 255, 255);
                        case 4:
                            return new Color32(255, 255, 255, 255);
                        case 8:
                            return new Color32(255, 255, 255, 255);
                        case 16:
                            return new Color32(255, 255, 255, 255);
                        case 32:
                            return new Color32(255, 255, 255, 255);
                        case 64:
                            return new Color32(255, 255, 255, 255);
                        case 128:
                            return new Color32(255, 255, 255, 255);
                        case 256:
                            return new Color32(255, 255, 255, 255);
                        case 512:
                            return new Color32(255, 255, 255, 255);
                        case 1024:
                            return new Color32(255, 255, 255, 255);
                        case 2048:
                            return new Color32(255, 255, 255, 255);
                        case 4096:
                            return new Color32(255, 255, 255, 255);
                        case 8192:
                            return new Color32(255, 255, 255, 255);
                        default:
                            return new Color32(255, 255, 255, 255);
                    }
                }
            case MapType.Dask:
                {
                    switch (amount)
                    {
                        case 2:
                            return new Color32(255, 255, 255, 255);
                        case 4:
                            return new Color32(255, 255, 255, 255);
                        case 8:
                            return new Color32(255, 255, 255, 255);
                        case 16:
                            return new Color32(255, 255, 255, 255);
                        case 32:
                            return new Color32(255, 255, 255, 255);
                        case 64:
                            return new Color32(255, 255, 255, 255);
                        case 128:
                            return new Color32(255, 255, 255, 255);
                        case 256:
                            return new Color32(255, 255, 255, 255);
                        case 512:
                            return new Color32(255, 255, 255, 255);
                        case 1024:
                            return new Color32(255, 255, 255, 255);
                        case 2048:
                            return new Color32(255, 255, 255, 255);
                        case 4096:
                            return new Color32(255, 255, 255, 255);
                        case 8192:
                            return new Color32(255, 255, 255, 255);
                        default:
                            return new Color32(255, 255, 255, 255);
                    }
                }
            default:
                return new Color32(255, 255, 255, 255);
        }

    }
}
