using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

using UnityEngine.SceneManagement;
using Photon.Pun;
public class Soldier : MonoBehaviourPunCallbacks
{
    
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sensitivity = 30.0f;

    [Header("Jump & Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 5.0f;
    private float yVelocity = 0f;

    [Header("Water (optional)")]
    public float WaterHeight = 15.5f;
    private bool isUnderWater;

    [Header("Underwater Jump")]
    public float waterJumpForce = 2.5f;
    private bool hasWaterJumped = false;

    [Header("Rotation Acceleration")]
    public float rotAccelRate = 1.0f;
    public float maxRotSpeedMul = 3.0f;
    private float currentRotMul = 1.0f;

    CharacterController character;
    public GameObject cam;
    float pitch = 0f;
    public bool webGLRightClickRotation = true;
    private int _healthpoints;

    // 👇 THÊM DÒNG NÀY
    private Animator animator;

    void Awake()
    {
        
         // 1) Luôn gán cam (dù là local hay remote)
        if (cam == null)
            cam = transform.Find("Camera").gameObject;
        // 2) Nếu không phải local player, disable camera + audio listener
        if (!photonView.IsMine){
            cam.SetActive(false);
            var al = cam.GetComponent<AudioListener>();
            if (al) al.enabled = false;    
            var bulletLeft = transform.Find("bulletLeft").gameObject;
            bulletLeft.SetActive(false);
            return;
        }
        // 3) Chỉ local mới chạy tiếp (gán _healthpoints, v.v.)
        _healthpoints = 30;
    }

    void Start()
    {
        if (!photonView.IsMine) return;
        character = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); // 👈 lấy Animator từ model con
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (!photonView.IsMine) return;
        float moveX = Input.GetAxis("MoveHorizontal") * speed;
        float moveZ = Input.GetAxis("MoveVertical") * speed;

        float inputYaw = Input.GetAxis("CamHorizontal");
        float inputPitch = Input.GetAxis("CamVertical");

        if (Mathf.Abs(inputYaw) > 0.01f || Mathf.Abs(inputPitch) > 0.01f)
        {
            currentRotMul += rotAccelRate * Time.deltaTime;
            currentRotMul = Mathf.Min(currentRotMul, maxRotSpeedMul);
        }
        else
        {
            currentRotMul = 1.0f;
        }

        float rotY = inputYaw * sensitivity * currentRotMul;
        float rotX = inputPitch * sensitivity * currentRotMul;

        isUnderWater = transform.position.y < WaterHeight;
        if (isUnderWater)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !hasWaterJumped)
            {
                yVelocity = waterJumpForce;
                hasWaterJumped = true;
            }
            else
            {
                yVelocity = 0f;
            }
            speed = 5f;
        }
        else
        {
            hasWaterJumped = false;
            speed = 10f;

            if (character.isGrounded)
            {
                if (yVelocity < 0f) yVelocity = -2f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            yVelocity += gravity * Time.deltaTime;
        }

        float yaw = rotY * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);

        pitch -= rotX * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, +90f);
        cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        Vector3 movement = new Vector3(moveX, yVelocity, moveZ);
        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);

        // 👇 Gọi animation khi di chuyển
        bool isMoving = moveX != 0f || moveZ != 0f;
        animator.SetTrigger("IsRunning");

        // 👇 Gọi animation khi bắn
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("IsShooting");
        }
    }

    public bool TakeHit(int dame)
    {
        if (!photonView.IsMine) return false;
        Debug.Log("Hit");
        _healthpoints -= dame;
        bool isDead = _healthpoints <= 0;
        if (isDead) _Die();
        return isDead;
    }

	private void _Die()
    {
        if (!photonView.IsMine) return;
        Cursor.lockState = CursorLockMode.None;  
        Cursor.visible   = true;               
        PhotonNetwork.Destroy(gameObject);
        SimpleLauncher.Instance.DieScene.SetActive(true);
    }

    public void OnTriggerEnter(Collider col){
        if (col.CompareTag("Bullet")){
            if (photonView.IsMine){
                TakeHit(2);
            }
        }
    }
}
