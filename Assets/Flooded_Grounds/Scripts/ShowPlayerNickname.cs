using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class ShowPlayerNickname : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Text>().text = photonView.Owner.NickName.Split('-')[0];
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = (Camera)FindObjectOfType(typeof(Camera));
        if (camera){
            transform.LookAt(camera.gameObject.transform);
        }
    }
}
