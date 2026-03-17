using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool contactDestruct = true;
    void Start()
    {
        Invoke("Destruct", lifetime);
    }

    void Destruct()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (contactDestruct)
        {
            Health health = collision.gameObject.GetComponentInParent<Health>();

            if (health != null)
            {
                health.ApplyDamage(damage, gameObject);
            }
        }

        Destruct();
    }
}

