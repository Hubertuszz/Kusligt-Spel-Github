using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	

	

    public void OnClickCharacterPick(int randomCharacter)
    {
        if (PlayerInfo.pi != null)
        {
            PlayerInfo.pi.selectedCharacter = randomCharacter;
            PlayerPrefs.SetInt("MyCharacter", randomCharacter);
        }
    }

}