using UnityEngine;

public class Close : StateMachineBehaviour
{
    public float rotationSpeed = 180f; // Tốc độ xoay độ/giây
    private Vector3 targetScale = new Vector3(0f, 0f, 0f); // Scale đích (0, 0, 0)
    private float scaleSpeed = 2f; // Tốc độ scale (thu nhỏ nhanh hay chậm)
    private Transform objTransform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        objTransform    = animator.transform;
        string objName = animator.gameObject.name;
        // Debug.Log(objName);

        if (objTransform != null)
        {
            // objTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Đặt scale ban đầu là (1, 1, 1)
            objTransform.localScale = (objName == "Lightning") ? new Vector3(0.2f,0.2f,0.2f) : new Vector3(2f,2f,2f);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (objTransform != null)
        {
            // Xoay liên tục
            objTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Thu nhỏ đối tượng từ (1, 1, 1) đến (0, 0, 0)
            objTransform.localScale = Vector3.Lerp(
                objTransform.localScale,
                targetScale,
                Time.deltaTime * scaleSpeed
            );
        }
    }
}
