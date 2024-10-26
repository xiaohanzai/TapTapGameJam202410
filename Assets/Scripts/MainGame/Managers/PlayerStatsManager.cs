using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public int GenerateRandomHPBack(int minHP, int maxHP)
    {
        return Random.Range(minHP, maxHP + 1);
    }
}
