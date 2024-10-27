using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    private int playerLight;
    private int playerMaxLight;
    private int environmentLight;
    private int environmentMaxLight;

    [SerializeField] private Image forestImage;
    [SerializeField] private Image playerLightImage;

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
            playerLightImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (float)playerLight / playerMaxLight);
            forestImage.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), (float)environmentLight / environmentMaxLight);
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
