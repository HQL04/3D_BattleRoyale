using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Animator soldierAnimator;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpheight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Creating the moving vector
        Vector3 move = transform.right * x + transform.forward * z;//(right - red axis, foward - blue axis)

        //actually moving the  player
        controller.Move(move * speed * Time.deltaTime);

        //check if the player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }

        //falling down
        velocity.y += gravity * Time.deltaTime;

        //executing the jump 
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            //use later
        }
        else
        {
            isMoving = false;
            ///use later
        }
        if (soldierAnimator != null)
            soldierAnimator.SetBool("isMoving", isMoving);
        lastPosition = gameObject.transform.position;

    }
}
