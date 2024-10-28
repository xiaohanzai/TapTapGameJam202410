using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    [SerializeField] private float playerLight;
    [SerializeField] private int playerMaxLight;
    [SerializeField] private int environmentLight;
    [SerializeField] private int environmentMaxLight;

    private int deltaLight;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image playerLightImage;

    [SerializeField] private Sprite cloudBackground;
    [SerializeField] private Sprite clearSkyBackground;

    private Color gray = new Color(0.2f, 0.2f, 0.2f, 1);
    private Color white = Color.white;

    public void SetUpParams(int playerInitLight, int envMaxLight)
    {
        playerMaxLight = playerInitLight;
        playerLight = playerMaxLight;
        environmentMaxLight = envMaxLight;
        environmentLight = 0;
        playerLightImage.color = white;
        backgroundImage.color = gray;
        backgroundImage.sprite = cloudBackground;
    }

    public bool CheckIfAuroraVisible(int round, int maxRounds)
    {
        if (round < 4) return false;

        float p = Random.Range(0, 1f);
        float threshold = (float)round / maxRounds - (playerLight / playerMaxLight) * 0.1f;
        if (p < threshold)
        {
            backgroundImage.sprite = clearSkyBackground;
            return true;
        }
        return false;
    }

    public void SharePlayerLightWithEnvironment()
    {
        if (playerLight >= playerMaxLight * 0.5f && environmentLight < environmentMaxLight)
        {
            playerLight -= 1;
            environmentLight++;
            deltaLight++;
            float t = (float)playerLight / playerMaxLight;
            playerLightImage.color = Color.Lerp(Color.black, white, t);
            t = (float)environmentLight / environmentMaxLight;
            backgroundImage.color = Color.Lerp(gray, white, -(1f - t) * (1f - t) + 1f);
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
        float t = (float)playerLight / playerMaxLight;
        playerLightImage.color = Color.Lerp(Color.black, white, t);
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
