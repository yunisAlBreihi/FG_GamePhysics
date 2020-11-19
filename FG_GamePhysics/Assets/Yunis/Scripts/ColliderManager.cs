using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    private List<CustomBoxCollider> collidersInWorld = new List<CustomBoxCollider>();

    public List<CustomBoxCollider> CollidersInWorld => collidersInWorld;

    /// <summary>
    /// Adds a Custom Box Collider to the manager.
    /// </summary>
    /// <returns></returns>
    public void AddColliderToList(CustomBoxCollider collider)
    {
        collidersInWorld.Add(collider);
    }

    /// <summary>
    /// Removes a Custom Box Collider to the manager.
    /// </summary>
    /// <returns></returns>
    public void RemoveColliderToList(CustomBoxCollider collider)
    {
        if (collidersInWorld.Contains(collider) == true)
        {
            collidersInWorld.Remove(collider);
        }
    }
}
