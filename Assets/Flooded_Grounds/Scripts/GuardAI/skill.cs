using UnityEngine;

public class TeleportSkill : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float cooldown    = 5f;     
    public float behindDist  = 1.5f;   

    private float _lastTime = -Mathf.Infinity;

    public bool IsReady => Time.time >= _lastTime + cooldown;

    public void TeleportBehind(Transform target)
    {
        Vector3 dirToAI = (transform.position - target.position).normalized;
        Vector3 dest = target.position + dirToAI * behindDist;
        transform.position = dest;
        _lastTime = Time.time;
    }
}