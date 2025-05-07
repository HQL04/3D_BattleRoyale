using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;
    public GameObject spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
		PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
