using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainGameBGM;
    [SerializeField] private AudioSource fairyBGM;
    [SerializeField] private AudioSource battleBGM;

    public void PlayMainGameBGM()
    {
        mainGameBGM.Play();
        fairyBGM.Stop();
        battleBGM.Stop();
    }

    public void PlayFairyBGM()
    {
        mainGameBGM.Stop();
        fairyBGM.Play();
        battleBGM.Stop();
    }

    public void PlayBattleBGM()
    {
        mainGameBGM.Stop();
        fairyBGM.Stop();
        battleBGM.Play();
    }
}
