using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{
	public static SimpleLauncher Instance { get; private set; }
	public GameObject DieScene;
	public InputField playerNickname;
	public GameObject Canvas;
    public PhotonView playerPrefab;
    public GameObject spawnPoint;

    // Start is called before the first frame update
	void Awake(){
		if (Instance != null && Instance != this)
        {
            // Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
	}
    void Start()
    {
        // PhotonNetwork.AutomaticallySyncScene = true;
		DieScene.SetActive(false);
    }

	public override void OnConnectedToMaster(){
		Debug.Log("Connected to Master");
		RoomOptions opts = new RoomOptions{ MaxPlayers = 4 };
  		PhotonNetwork.JoinOrCreateRoom("MainRoom", opts, TypedLobby.Default);
		
	}
	
	public override void OnJoinedRoom(){
		Debug.Log($"Joined a room. Room ID: {PhotonNetwork.CurrentRoom.Name}");
		PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
	}

	public void StartTheGamePVP(){
        if (playerNickname.text.Length < 1){
		    PhotonNetwork.NickName = "Nameless";
        }
		else PhotonNetwork.NickName = playerNickname.text;

		PhotonNetwork.ConnectUsingSettings();
		Canvas.SetActive(false);
        

	}

	public void StartTheGamePVE(){
		if (playerNickname.text.Length < 1){
		    PhotonNetwork.NickName = "Nameless";
        }
		else PhotonNetwork.NickName = playerNickname.text;
        SceneManager.LoadSceneAsync("Scene_A");
	}
	
	public override void OnJoinRoomFailed(short returnCode, string message)
    {
        
        Debug.LogWarning($"Join failed: {message}");
    }
	
	public void PlayAgain(){
		DieScene.SetActive(false);
		PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
        

	}
}