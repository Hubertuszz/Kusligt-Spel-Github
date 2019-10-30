using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{

    private PhotonView pv;
    public GameObject myAvatar;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetupController.gs.spawnPoints.Length);
        if (pv.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerAvatar"),
                GameSetupController.gs.spawnPoints[spawnPicker].position, GameSetupController.gs.spawnPoints[spawnPicker].rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
