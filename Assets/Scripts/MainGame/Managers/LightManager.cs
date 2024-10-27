using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    private float playerLight;
    private int playerMaxLight;
    private int environmentLight;
    private int environmentMaxLight;

    private int deltaLight;

    [SerializeField] private Image forestImage;
    [SerializeField] private Image playerLightImage;

    public void SetUpParams(int playerInitLight, int envMaxLight)
    {
        playerMaxLight = playerInitLight;
        playerLight = playerMaxLight;
        environmentMaxLight = envMaxLight;
        environmentLight = 0;
        playerLightImage.color = Color.white;
        forestImage.color = Color.black;
    }

    public bool CheckIfAuroraVisible(int round, int maxRounds)
    {
        if (round < 4) return false;

        float p = Random.Range(0, 1f);
        float threshold = (float)round / maxRounds - (playerLight / playerMaxLight) * 0.1f;
        if (p < threshold) return true;
        return false;
    }

    public void SharePlayerLightWithEnvironment()
    {
        if (playerLight >= playerMaxLight * 0.5f && environmentLight < environmentMaxLight)
        {
            playerLight -= 1;
            environmentLight++;
            deltaLight++;
            playerLightImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, playerLight / playerMaxLight);
            forestImage.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), (float)environmentLight / environmentMaxLight);
        }
    }

    public bool IsFairySeeable(int round)
    {
        if (round < 3) return true;
        float p = Random.Range(0f, 1f);
        if (p < (float)environmentLight / environmentMaxLight) return true;
        else return false;
    }

    public bool IsEnemySeeable(int round)
    {
        if (round != 1 && round != 3 && round != 5 && round != 7) return false;
        //Debug.Log(round + " " + playerLight);
        if (playerLight < playerMaxLight / 2f) return false;
        else return true;
    }

    public void SetPayerCurrentLight(float light)
    {
        playerLight = light;
        deltaLight = 0;
    }

    public int GetDeltaLight()
    {
        return deltaLight;
    }

    public float GetEnvironmentLight()
    {
        return environmentLight;
    }
}
