using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView pv;
    public int characterValue;
    public GameObject myCharacter;

    public int playerHealth;
    public int playerDamge;

    public Camera myCamera;
    public AudioListener myAL;

public string INFECTED = "";
    // Start is called before the first frame update
public int RandomNumber(int min, int max)  
    {  
        System.Random random = new System.Random();  
        return random.Next(min, max);  
    } 
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.pi.selectedCharacter);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAL);
        }

        if(pv.Owner.NickName == "luks" && pv.IsMine)
        {
            Debug.Log("LUKAS JA DU");
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, 1);
        }
        else
        {
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, 0);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int randomCharacter)
    {
        myCharacter = Instantiate(PlayerInfo.pi.allCharacters[randomCharacter], transform.position, transform.rotation, transform);
    }
}
