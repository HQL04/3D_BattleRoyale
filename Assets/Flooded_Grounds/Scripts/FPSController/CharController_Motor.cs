using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {
	[Header("Movement Settings")]
	public float speed = 10.0f;
	public float sensitivity = 30.0f;

	[Header("Jump & Gravity")]
    public float gravity      = -9.81f;   // gia tốc trọng lực
    public float jumpHeight   = 5.0f;     // độ cao nhảy
    private float yVelocity   = 0f;       // tốc độ theo trục Y hiện tại

	[Header("Water (optional)")]
	public float WaterHeight = 15.5f;
	private bool  isUnderWater;

	[Header("Underwater Jump")]
	public float waterJumpForce = 2.5f;     // lực đẩy lên khi nhảy dưới nước
	private bool  hasWaterJumped = false; // đã nhảy 1 lần chưa

	[Header("Rotation Acceleration")]
    public float rotAccelRate = 1.0f;          // tốc độ tăng hệ số mỗi giây
    public float maxRotSpeedMul = 3.0f;        // hệ số max
    private float currentRotMul = 1.0f;        // hệ số xoay hiện tại

	CharacterController character;
	public GameObject cam ;
	float pitch = 0f; 
	public bool webGLRightClickRotation = true;

	private int _healthpoints;

	void Awake() {
        if (cam == null && transform.childCount > 0) {
            cam = transform.GetChild(0).gameObject;
        }
		_healthpoints = 30;
    }

	void Start(){
		//LockCursor ();
		character = GetComponent<CharacterController> ();
		if (Application.isEditor) {
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
		}
	}

	void Update(){
		// Di chuyển bằng ←/→, ↑/↓
		float moveX = Input.GetAxis("MoveHorizontal") * speed;
		float moveZ = Input.GetAxis("MoveVertical")   * speed;

		// Xoay camera bằng A/D, W/S
		// float rotY = Input.GetAxis("CamHorizontal");
		// float rotX = Input.GetAxis("CamVertical");

		float inputYaw   = Input.GetAxis("CamHorizontal"); // -1..+1
        float inputPitch = Input.GetAxis("CamVertical");

		// 2) Cập nhật hệ số xoay
        if (Mathf.Abs(inputYaw) > 0.01f || Mathf.Abs(inputPitch) > 0.01f) {
            // đang giữ phím → tăng dần tới max
            currentRotMul += rotAccelRate * Time.deltaTime;
            currentRotMul = Mathf.Min(currentRotMul, maxRotSpeedMul);
        } else {
            // khi ngừng giữ → reset
            currentRotMul = 1.0f;
        }

		float rotY = inputYaw   * sensitivity * currentRotMul;
        float rotX = inputPitch * sensitivity * currentRotMul;

		// CheckForWaterHeight();
		isUnderWater = transform.position.y < WaterHeight;
		if (isUnderWater){
			// Ở dưới nước thì tắt trọng lực tự do
			// và chỉ cho phép nhảy 1 lần duy nhất
			if (Input.GetKeyDown(KeyCode.Space) && !hasWaterJumped){
				yVelocity = waterJumpForce;
				hasWaterJumped = true;
			} else {
				// khi không nhấn hoặc đã nhảy rồi, giữ yVelocity = 0
				yVelocity = 0f;
			}
			speed = 5f;
		} else {
			// ra khỏi nước → reset hasWaterJumped để lần sau vào nước có thể nhảy lại
			hasWaterJumped = false;
			speed = 10f;

			// nếu đang chạm đất, cho phép nhảy bình thường
			if (character.isGrounded){
				if (yVelocity < 0f) yVelocity = -2f;  
				if (Input.GetKeyDown(KeyCode.Space)){
					yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
				}
			}
			// nếu không chạm đất, áp dụng trọng lực rơi tự do
			yVelocity += gravity * Time.deltaTime;
		}

		float yaw = rotY * Time.deltaTime;
   		transform.Rotate(0f, yaw, 0f);

		pitch -= rotX * Time.deltaTime;
    	pitch  = Mathf.Clamp(pitch, -90f, +90f);
		cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

		// Vector di chuyển (local space)
		Vector3 movement = new Vector3(moveX, yVelocity, moveZ);
		
		// Chuyển sang world space và di chuyển
		movement = transform.rotation * movement;
		character.Move(movement * Time.deltaTime);
	}
	public bool TakeHit(int dame)
    {
        _healthpoints -= dame;
        bool isDead = _healthpoints <= 0;
        if (isDead) _Die();
        return isDead;
    }

	private void _Die()
    {
        Destroy(gameObject);
    }
}
