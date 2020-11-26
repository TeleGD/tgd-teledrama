using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TGDUtils
{
    public static int[] RandomIntegers(int n, int max)
    {
        if (n > max)
            return null;

        List<int> randomList = new List<int>();
        for (int i = 0; i < n; i++)
        {
            int x;
            do
            {
                x = Random.Range(0, max);
            }
            while (randomList.Contains(x));
            randomList.Add(x);
        }
        return randomList.ToArray();
    }
}
