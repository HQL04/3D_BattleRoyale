using UnityEngine;
using Photon.Pun;

public class MouseMovement : MonoBehaviourPunCallbacks
{

    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float topClamp = 90f;
    public float bottomClamp = -90f;
    public GameObject BulletPoint;
    public GameObject BulletPrefab;
    public GameObject muzzleEffect;

    
    void Start()
    {
        // locking the cursor to the middle of the screen  and making it invisible
        if (!photonView.IsMine) return;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        //getting mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotation  around  the x axis (look up and down)
        xRotation -= mouseY;

        //clamp to rotation
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);
        // rotation  around  the y axis (look left and right)
        yRotation += mouseX;

        //apply rotations to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Shoot
        
        if (Input.GetButtonDown("Fire1") && Weapon.Instance.bulletsLeft > 0){
            Debug.Log("Shoot");
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            PhotonNetwork.Instantiate("Bullet", BulletPoint.transform.position, BulletPoint.transform.rotation);
        }
    }
}
