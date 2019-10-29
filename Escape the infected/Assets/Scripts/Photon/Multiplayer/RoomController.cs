using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviourPunCallbacks
{
    public static RoomController rm;
    [SerializeField]
    private int multiplayerSceneIndex;
    [SerializeField]
    private int waitingRoomSceneIndex;


    

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        if (MultiplayerSettings.mps.ranked)
        {
            SceneManager.LoadScene(waitingRoomSceneIndex);
        }
        else
            StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }

    

    

}
   