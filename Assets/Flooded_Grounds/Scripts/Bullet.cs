using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bulletSpeed = 10f;
    void Start()
    {
        StartCoroutine(DestroyMe());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, bulletSpeed * Time.deltaTime);
    }
    IEnumerator DestroyMe(){
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

}
