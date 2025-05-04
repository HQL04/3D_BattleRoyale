using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifetime = 5f;
    public GameObject impactEffectPrefab; 

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Đạn chạm Player");
            var motor = other.GetComponent<CharController_Motor>();
            if (motor != null)
            {
                motor.TakeHit(damage);
            }
        }

        Vector3 deathPos = transform.position;
        Debug.Log($"Bullet died at {deathPos}");

        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}