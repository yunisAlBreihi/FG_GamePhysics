using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Explosion : MonoBehaviour
{
    [SerializeField, Range(0.5f, 10.0f), Tooltip("The max size the explosion would get to before disappearing.")]
    private float maxExplosionSize = 5.0f;

    [SerializeField, Range(0.5f, 10.0f), Tooltip("How fast the explosion expands.")]
    private float expansionSpeed = 5.0f;

    private Rigidbody rigidBody = null;
    private SphereCollider sphereCollider = null;
    private GameManager gameManager = null;

    private float expansion = 0.1f;

    private void Awake()
    {
        //Find the game manager in the world.
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.Log(name + ": You need a Game Manager in the world!");

        //Find the rigid body on the object.
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
            Debug.LogError(name + " needs a Rigid Body!");
        else
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = false;
        }

        //Find the sphere collider on the object.
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
            Debug.LogError(name + " needs a Sphere Collider!");
        else
            sphereCollider.isTrigger = true;

        //Sets the scale of the object to zero at start. (expansion is 0).
        transform.localScale = Vector3.one * expansion;
    }

    void Update()
    {
        //Destroy if reached max size, otherwise expand.
        if (expansion >= maxExplosionSize)
            Destroy(gameObject);
        else
            Expand();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If hit a target, explode right away.
        TargetObject target = other.gameObject.GetComponent<TargetObject>();
        if (target != null)
        {
            target.Destroy();
            gameManager.ChangeWind();
        }
    }

    /// <summary>
    /// Expands the the object to simulate an explosion.
    /// </summary>
    /// <returns></returns>
    private void Expand()
    {
        transform.localScale = Vector3.one * expansion;
        expansion += Time.deltaTime * expansionSpeed;
    }
}
