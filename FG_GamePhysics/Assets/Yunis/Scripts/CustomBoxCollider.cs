using UnityEngine;

public class CustomBoxCollider : MonoBehaviour
{
    [SerializeField, Tooltip("Shows the boundaries of the collider")]
    private bool showCollider = true;

    private Vector3 position = Vector3.zero;
    private Vector3 scale = Vector3.zero;

    private ColliderManager colliderManager = null;
    private CustomBoxCollider hitCollider = null;

    private CustomPhysics physics = null;

    public Vector3 Position => position;
    public Vector3 Scale => scale;
    public CustomBoxCollider HitCollider => hitCollider;

    private void Awake()
    {
        //Finds a collision manager in the world. If found, adds this collider to the manager list.
        colliderManager = FindObjectOfType<ColliderManager>();
        if (colliderManager == null)
            Debug.LogError("You need to have a collision manager in the scene!");
        else
            colliderManager.AddColliderToList(this);

        //Finds a Custom Physics component on this object.
        physics = GetComponent<CustomPhysics>();

        //Sets the transform and scale of the collider the same size as the object.
        position = transform.localPosition;
        scale = transform.localScale;
    }

    private void Update()
    {
        //Checks if there is any collisions
        if (hitCollider == null)
        {
            OnCollisionBegin(CheckCollision());
        }
        else if (CheckCollision() == null)
        {
            OnCollisionEnd();
        }

        //Updates the colliders position and scale to match the object.
        if (position != transform.position)
        {
            UpdatePosition();
        }
        if (scale != transform.localScale)
        {
            UpdateScale();
        }
    }

    private void OnDestroy()
    {
        colliderManager.RemoveColliderToList(this);
    }

    /// <summary>
    /// Updates the colliders position to be the same as the objects position.
    /// </summary>
    /// <returns></returns>
    private void UpdatePosition()
    {
        position = transform.position;
    }

    /// <summary>
    /// Updates the colliders scale to be the same as the objects scale.
    /// </summary>
    /// <returns></returns>
    private void UpdateScale()
    {
        scale = transform.localScale;
    }

    /// <summary>
    /// Checks if there are any collisions between the colliders in the world.
    /// </summary>
    /// <returns></returns>
    public CustomBoxCollider CheckCollision()
    {
        foreach (var collider in colliderManager.CollidersInWorld)
        {
            if (collider == this)
            {
                continue;
            }

            if (Contains(collider.transform))
            {
                return collider;
            }
        }
        return null;
    }

    /// <summary>
    /// When a collider enters another collider.
    /// </summary>
    /// <returns></returns>
    public void OnCollisionBegin(CustomBoxCollider other)
    {
        if (hitCollider != other)
        {
            CorrectPhysicsObject(other);
            hitCollider = other;
        }
    }

    /// <summary>
    /// When a collider exits another collider.
    /// </summary>
    /// <returns></returns>
    private void OnCollisionEnd()
    {
        if (hitCollider != null)
        {
            hitCollider = null;

            //Removes the grounded status of the physics object.
            if (physics != null)
            {
                physics.Grounded = false;
            }
        }
    }

    /// <summary>
    /// Applies corresponding physics of the object holding this collider.
    /// </summary>
    /// <returns></returns>
    private void CorrectPhysicsObject(CustomBoxCollider other)
    {
        if (physics != null)
        {
            //Reflects the force of the physics so the object bounces.
            physics.Grounded = true;
            physics.CorrectPosition(other);
            physics.Reflect(transform.up);

            //Add bounces to the bullet class if it has any.
            Bullet bullet = physics.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Bounce();
            }
        }
    }

    /// <summary>
    /// Is another object inside the collider?
    /// </summary>
    /// <returns></returns>
    public bool Contains(Transform other)
    {
        bool top = other.position.y - other.localScale.y / 2.0f < position.y + scale.y / 2.0f;
        bool bot = other.position.y + other.localScale.y / 2.0f > position.y - scale.y / 2.0f;
        bool left = other.position.x - other.localScale.x / 2.0f < position.x + scale.x / 2.0f;
        bool right = other.position.x + other.localScale.x / 2.0f > position.x - scale.x / 2.0f;
        bool front = other.position.z - other.localScale.z / 2.0f < position.z + scale.z / 2.0f;
        bool back = other.position.z + other.localScale.z / 2.0f > position.z - scale.z / 2.0f;

        if (top && bot && left && right && front && back)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //Shows a wire cube of the collider. Green if is colliding, red if is not colliding with anything.
        if (showCollider == true)
        {
            if (hitCollider != null && Contains(hitCollider.transform))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
