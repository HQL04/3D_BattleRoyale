using UnityEngine;

public class Active : StateMachineBehaviour
{
    public float rotationSpeed = 180f; // Tốc độ xoay độ/giây
    private float delay = 1f;           // Thời gian chờ trước khi hiện
    private float timer = 0f;
    private Renderer renderer;
    private bool hasEnabled = false;
    private Vector3 targetScale ;//= new Vector3(0.2f,0.2f,0.2f); // Scale đích (1,1,1)
    private float scaleSpeed = 2f; // Tốc độ scale (bự lên nhanh hay chậm)
    private Transform objTransform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        renderer = animator.gameObject.GetComponent<Renderer>();
        objTransform = animator.transform;
        string objName = animator.gameObject.name;
        Debug.Log(objName);
        targetScale = (objName == "Lightning") ? new Vector3(0.2f,0.2f,0.2f) : new Vector3(2f,2f,2f);
        
        if (renderer != null)
        {
            renderer.enabled = false; // Tắt trước
        }

        if (objTransform != null)
        {
            objTransform.localScale = Vector3.zero; // Scale về 0 trước
        }

        timer = 0f;
        hasEnabled = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (!hasEnabled && timer >= delay)
        {
            if (renderer != null)
            {
                renderer.enabled = true; // Bật Renderer sau delay
                Debug.Log("Đã bật Renderer sau delay");
            }
            hasEnabled = true;
        }

        if (hasEnabled)
        {
            // Xoay liên tục
            objTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Phóng to dần
            objTransform.localScale = Vector3.Lerp(
                objTransform.localScale,
                targetScale,
                Time.deltaTime * scaleSpeed
            );
        }
    }
}

