using UnityEngine;

public class Hide : StateMachineBehaviour
{
    // Hàm này sẽ được gọi ngay khi bắt đầu vào state Hide
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ẩn đối tượng bằng cách disable Renderer
        Renderer renderer = animator.gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }

}
