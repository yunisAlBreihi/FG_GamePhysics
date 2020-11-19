using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    [SerializeField, Range(0.1f, 50.0f), Tooltip("The mass of the object.")]
    private float mass = 1.0f;

    [SerializeField, Range(0.0f, 1.0f), Tooltip("How much energy the object keeps when bouncing.")]
    private float bounciness = 0.8f;

    [SerializeField, Range(0.0f, 1.0f), Tooltip("How much the object slows down when hiting ground.")]
    private float roughness = 0.05f;

    [SerializeField, Tooltip("Is the object affected by gravity?")]
    private bool useGravity = true;

    private WindHandler windHandler = null;

    private Vector3 velocity = Vector3.zero;
    private bool grounded = false;

    public Vector3 Velocity => velocity;
    public bool Grounded { get { return grounded; } set { grounded = value; } }

    private void Awake()
    {
        //Find the Wind handler in the world. If found, add physics object to the wind handler.
        windHandler = FindObjectOfType<WindHandler>();
        if (windHandler == null)
            Debug.LogError("You need a wind handler in the world!");
        else
            windHandler.AddPhysicsObjectToList(this);
    }

    private void Update()
    {
        if (grounded == true)
            ApplyDrag();
        else
        {
            if (useGravity == true)
                ApplyGravity();
        }

        //Changes the position depending on the velocity.
        transform.position = transform.position + (velocity * Time.deltaTime);
    }

    private void OnDestroy()
    {
        windHandler.RemovePhysicsObjectToList(this);
    }

    /// <summary>
    /// Removes velocity depending on how rough the object is.
    /// </summary>
    /// <returns></returns>
    private void ApplyDrag()
    {
        velocity = velocity * (1.0f - roughness);
    }

    /// <summary>
    /// Applies gravity to the physics object.
    /// </summary>
    /// <returns></returns>
    private void ApplyGravity()
    {
        velocity = velocity + (Physics.gravity * mass) * Time.deltaTime;
    }

    /// <summary>
    /// Reflects the velocity with a dissipation from the bounciness.
    /// If the reflected force is small enough, will stop the velocity.
    /// Also returns the reflect force for comparison purposes.
    /// </summary>
    /// <returns></returns>
    public Vector3 Reflect(Vector3 normal)
    {
        Vector3 reflectForce = (velocity - 2.0f * Vector3.Dot(velocity, normal) * normal) * bounciness;
        if (reflectForce.y <= 3.0f)
        {
            reflectForce = new Vector3(reflectForce.x, 0.0f, reflectForce.z);
        }
        velocity = reflectForce;
        return reflectForce;
    }

    /// <summary>
    /// Reflects the velocity with a dissipation from the bounciness.
    /// If the reflected force is small enough, will stop the velocity.
    /// Also returns the reflect force for comparison purposes.
    /// </summary>
    /// <returns></returns>
    public void CorrectPosition(CustomBoxCollider hitCollider)
    {
        transform.position = new Vector3(transform.position.x,
                                         hitCollider.Position.y + (hitCollider.Scale.y * 0.5f) + (transform.localScale.y * 0.5f),
                                         transform.position.z);
    }

    /// <summary>
    /// Adds an impulse force to the velocity.
    /// </summary>
    /// <returns></returns>
    public void ApplyImpulse(Vector3 force)
    {
        Vector3 acc = force / mass;
        velocity = velocity + acc * Time.deltaTime;
    }

    /// <summary>
    /// Adds velocity.
    /// </summary>
    /// <returns></returns>
    public void AddVelocity(Vector3 velocity)
    {
        this.velocity += velocity;
    }
}
