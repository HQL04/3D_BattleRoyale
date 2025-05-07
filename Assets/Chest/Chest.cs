using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false;
    private float timer = 0f;
    private bool startCountdown = false;
    private Animator itemAnimator;
    public bool isLightning = true;
    // public static bool IsClicked = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // if (isLightning) 
        isLightning = Random.value > 0.5f;
        Transform itemTransform = transform.Find(isLightning ? "Lightning" : "Heart");

        if (itemTransform != null)
        {
            itemAnimator = itemTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Không tìm thấy Item trong Chest!");
        }
    }

    void Update()
    {
        var cam = Camera.main;
        if(cam == null) return;     // chưa có camera thì bỏ qua
        // Kiểm tra xem có click chuột và đối tượng có được chọn (dùng Raycast)
        if (Input.GetMouseButtonDown(0) && !isOpened)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Tạo ray từ vị trí chuột
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Kiểm tra nếu ray va chạm với vật thể
            {
                if (hit.collider.gameObject == gameObject) // Nếu đối tượng được click là Chest
                {
                    animator.SetTrigger("ClickTrigger"); // Kích hoạt animation mở
                    isOpened = true;
                    // animator.SetBool("IsClicked", true);
                    // IsClicked = true;
                    startCountdown = true; // Bắt đầu đếm giờ

                    if (itemAnimator != null)
                    {
                        itemAnimator.SetTrigger("Unhide"); // <<< Kích hoạt Lightning
                        Debug.Log("Đã set trigger Unhide cho Item");
                    }
                }
            }
        }

        if (startCountdown)
        {
            timer += Time.deltaTime;
            if(timer >= 13f){
                if (itemAnimator != null)
                {
                    itemAnimator.SetTrigger("Close"); // <<< Kích hoạt Lightning
                    Debug.Log("Đã set trigger Close cho Item");
                }
            }
            if (timer >= 15f) // Sau 15 giây
            {
                gameObject.SetActive(false); // Ẩn toàn bộ chest
                startCountdown = false; // Dừng đếm
            }
        }
    }
}
