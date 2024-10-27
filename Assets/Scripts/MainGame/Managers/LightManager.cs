using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private int playerLight;
    private int playerMaxLight;
    [SerializeField] private int environmentLight;
    private int environmentMaxLight;

    public void SetUpParams(int playerInitLight, int playerMaxLight, int envMaxLight)
    {
        playerLight = playerInitLight;
        this.playerMaxLight = playerMaxLight;
        environmentMaxLight = envMaxLight;
        environmentLight = 0;
    }

    public bool CheckIfAuroraVisible(int round, int maxRounds)
    {
        if (round < 3) return false;

        float p = Random.Range(0, 1f);
        float threshold = ((float)round / maxRounds) * ((float)environmentLight / environmentMaxLight);
        if (p < threshold) return true;
        return false;
    }

    public void SharePlayerLightWithEnvironment()
    {
        if (playerLight > 1 && environmentLight < environmentMaxLight)
        {
            playerLight--;
            environmentLight++;
        }
    }

    public int GetPlayerLight()
    {
        return playerLight;
    }

    public bool IsFairySeeable()
    {
        float p = Random.Range(0, 1f);
        if (p < (float)environmentLight / environmentMaxLight) return true;
        else return false;
    }

    public bool IsEnemySeeable()
    {
        float p = Random.Range(0, 1f);
        if (p < (float)playerLight / playerMaxLight) return true;
        else return false;
    }
}
