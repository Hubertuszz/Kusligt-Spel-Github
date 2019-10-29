using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings mps;

    public bool ranked;
    public bool customgame;
    

    private void Awake()
    {
        if(MultiplayerSettings.mps == null)
        {
            MultiplayerSettings.mps = this;
        }
        else
        {
            if(MultiplayerSettings.mps != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

}

