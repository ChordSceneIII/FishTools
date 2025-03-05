using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FMath
{
    public static T LootTableGet<T>(Dictionary<T, float> lootTable, int seed)
    {
        Random.InitState(seed);
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
}