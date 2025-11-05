using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float velocity = 20f;
    [SerializeField] private GameObject barrelEnd;
    [SerializeField] private GameObject projectile;
    private float timeStamp;

    void FireWeapon()
    {
        if (Time.time > timeStamp + fireRate)
        {
            GameObject instantiatedObject = Instantiate(projectile, barrelEnd.transform.position, barrelEnd.transform.rotation);

            Rigidbody rbody = instantiatedObject.GetComponent<Rigidbody>();

            Physics.IgnoreCollision(transform.GetComponentInChildren<Collider>(), instantiatedObject.GetComponentInChildren<Collider>());

            if (rbody != null)
            {
                rbody.linearVelocity = barrelEnd.transform.forward * velocity;
                timeStamp = Time.time;
            }
        }
    }
}
