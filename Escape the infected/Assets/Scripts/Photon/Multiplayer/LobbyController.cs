using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject rankedSearchButton;
    [SerializeField]
    private GameObject rankedCancelButton;
    [SerializeField]
    private GameObject quickStartButton;
    [SerializeField]
    private GameObject quickCancelButton;
    [SerializeField]
    private int RoomSize;

    private PhotonView pv;
    public GameObject player;

    private void Start()
    {
        pv = player.GetComponent<PhotonView>();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        rankedSearchButton.SetActive(true);
        quickStartButton.SetActive(true);
        
    }

    public void RankedStart()
    {
        rankedSearchButton.SetActive(false);
        rankedCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Ranked Start");
        MultiplayerSettings.mps.ranked = true;
        StartCoroutine(Test());    }

    public void QuickStart()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
        MultiplayerSettings.mps.ranked = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating Room");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom();
    }

    public void RankedCancel()
    {
        rankedCancelButton.SetActive(false);
        rankedSearchButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void QuickCancel()
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        MultiplayerSettings.mps.ranked = false;
    }
}
