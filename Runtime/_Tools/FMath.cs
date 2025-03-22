using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FMath
{
    public static T LootTableGet<T>(Dictionary<T, float> lootTable)
    {
        float totalWeight = 0;
        foreach (var item in lootTable)
        {
            totalWeight += item.Value;
        }

        float randomValue = Random.Range(0f, totalWeight);

        // 权重累加检测
        float accumulated = 0;
        foreach (var item in lootTable)
        {
            accumulated += item.Value;
            if (randomValue <= accumulated)
            {
                return item.Key;
            }
        }

        return lootTable.First().Key;
    }


    // 针对List<T>的优化版本
    public static void Shuffle<T>(this IList<T> list, int seed)
    {
        System.Random _rng = new System.Random(seed);

        int n = list.Count;
        while (n > 1)
        {
            int k = _rng.Next(n--);
            (list[n], list[k]) = (list[k], list[n]); // 使用元组交换
        }
    }
}