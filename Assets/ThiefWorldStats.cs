using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ThiefWorldStats : NetworkBehaviour
{
    public ThiefStatistics privateStats;
    [SyncVar]public float exposeValue;
    [SyncVar]public float escapeValue;
    [Header("World Sliders")]
    public Slider worldDetect;
    public Slider worldEscape;

    private void Awake()
    {
        worldEscape.maxValue = privateStats.maxEscape;
        worldDetect.maxValue = privateStats.maxDetect;
    }

    // Update is called once per frame
    void Update()
    {
        exposeValue = privateStats.detectValue;
        escapeValue = privateStats.escapeValue;
        worldDetect.value = exposeValue;
        worldEscape.value = escapeValue;

        if (worldDetect.value <= 0)
        {
            worldDetect.gameObject.SetActive(false);
        }

        if (worldDetect.value >= 1)
        {
            worldDetect.gameObject.SetActive(true);
        }

        if (worldEscape.value <= 0)
        {
            worldEscape.gameObject.SetActive(false);
        }

        if (worldEscape.value >= 1)
        {
            worldEscape.gameObject.SetActive(true);
        }
    }

    [ClientRpc]
    public void RpcRefreshStat()
    {
        
    }
}
