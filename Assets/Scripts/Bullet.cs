using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            print("hit " + collision.gameObject.name + " !");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Untagged"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
    }
    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(objectWeHit.gameObject.transform);
        Destroy(hole, 5f); // Tự hủy sau 5 giây
    }
}
