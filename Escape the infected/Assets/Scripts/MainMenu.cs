using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button QuickPlay;
    public Text loading;
    public Button Cube;
    public Button Infected;

    void Start()
    {
        loading.gameObject.SetActive(true);
    }

    void Update()
    {
        if(QuickPlay.IsActive())
        {
            loading.gameObject.SetActive(false);
            Infected.gameObject.SetActive(true);
            Cube.gameObject.SetActive(true);
        }
    }
}
