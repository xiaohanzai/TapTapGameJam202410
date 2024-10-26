using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private int playerInitLight;
    [SerializeField] private int environmentMaxLight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfAuroraVisible(int round)
    {
        if (round < 3) return false;

        float p = Random.Range(0, 1f);
        if (p > 0.1f) return true;
        return false;
    }

    public void SharePlayerLightWithEnvironment()
    {
        playerInitLight--;
        environmentMaxLight++;
    }
}
