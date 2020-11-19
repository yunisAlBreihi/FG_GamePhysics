using UnityEngine;

[RequireComponent(typeof(CustomBoxCollider), typeof(SphereCollider), typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("specify the explosion script prefab.")]
    Explosion explosionPrefab;

    [SerializeField, Range(1, 10), Tooltip("How many times the ball bounces before exploding")]
    private int bouncesBeforeExplode = 4;

    private Rigidbody rigidBody = null;
    private SphereCollider sphereCollider = null;
    private int bounces = 0;

    private void Awake()
    {
        //Find Rigid body. If found, turn off gravity and physics on it.
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
            Debug.LogError(name + " needs a Rigid Body!");
        else
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = false;
        }

        //Find sphere collider
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
            Debug.LogError(name + " needs a Sphere Collider!");
        else
            sphereCollider.isTrigger = true;
    }

    /// <summary>
    /// Uses the sphere collider as a trigger to find target.
    /// </summary>
    /// <returns></returns>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TargetObject>())
            Explode();
    }

    /// <summary>
    /// Adds bounces to the bullet, to count towards exploding it.
    /// </summary>
    /// <returns></returns>
    public void Bounce()
    {
        bounces++;

        if (bounces >= bouncesBeforeExplode)
            Explode();
    }

    /// <summary>
    /// Instantiates an explosion and than destroys itself.
    /// </summary>
    /// <returns></returns>
    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
