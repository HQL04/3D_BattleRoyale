using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    public float lifetime = 4.6f;
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
            var motor = other.GetComponent<Soldier>();
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