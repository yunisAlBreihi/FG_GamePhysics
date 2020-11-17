using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxCollider : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] private float roughness = 0.2f;
    [SerializeField] private bool showCollider = true;

    private Vector3 position = Vector3.zero;
    private Vector3 scale = Vector3.zero;

    private ColliderManager manager = null;
    private CustomBoxCollider otherCollider = null;

    private CustomPhysics physics = null;

    public Vector3 Position => position;
    public Vector3 Scale => scale;

    private void Awake()
    {
        manager = FindObjectOfType<ColliderManager>();
        if (manager == null)
        {
            Debug.LogError("You need to have a collision manager in the scene!");
        }
        else
        {
            manager.AddColliderToList(this);
        }

        physics = GetComponent<CustomPhysics>();

        position = transform.localPosition;
        scale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (otherCollider == null)
        {
            OnCollisionBegin(CheckCollision());
        }
        else if (CheckCollision() == null)
        {
            OnCollisionEnd();
        }

        if (position != transform.position)
        {
            UpdatePosition();
        }
        if (scale != transform.localScale)
        {
            UpdateScale();
        }
    }

    private void UpdatePosition()
    {
        position = transform.position;
    }

    private void UpdateScale()
    {
        scale = transform.localScale;
    }

    public CustomBoxCollider CheckCollision()
    {
        foreach (var collider in manager.CollidersInWorld)
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

    public void OnCollisionBegin(CustomBoxCollider other)
    {

        if (otherCollider != other)
        {
            if (physics != null)
            {
                physics.Reflect(transform.up, roughness);
            }
            otherCollider = other;
        }
    }

    private void OnCollisionEnd()
    {
        if (otherCollider != null)
        {
            otherCollider = null;
        }
    }

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

    public bool DebugContains(Vector3 otherPos, Vector3 otherSize)
    {
        bool top = otherPos.y - otherSize.y / 2.0f < transform.position.y + transform.localScale.y / 2.0f;
        bool bot = otherPos.y + otherSize.y / 2.0f > transform.position.y - transform.localScale.y / 2.0f;
        bool left = otherPos.x - otherSize.x / 2.0f < transform.position.x + transform.localScale.x / 2.0f;
        bool right = otherPos.x + otherSize.x / 2.0f > transform.position.x - transform.localScale.x / 2.0f;
        bool front = otherPos.z - otherSize.z / 2.0f < transform.position.z + transform.localScale.z / 2.0f;
        bool back = otherPos.z + otherSize.z / 2.0f > transform.position.z - transform.localScale.z / 2.0f;

        if (top && bot && left && right && front && back)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (showCollider == true)
        {
            if (otherCollider != null && Contains(otherCollider.transform))
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
