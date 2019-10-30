using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NetworkPlayer"), Vector3.zero, Quaternion.identity);
    }

    public static GameSetupController gs;

    public Transform[] spawnPoints;


    private void OnEnable()
    {
        if (GameSetupController.gs == null)
        {
            GameSetupController.gs = this;
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
            SceneManager.LoadScene(0);
        }
    }
}
