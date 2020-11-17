using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    private List<CustomBoxCollider> collidersInWorld = new List<CustomBoxCollider>();

    public List<CustomBoxCollider> CollidersInWorld => collidersInWorld;

    public void AddColliderToList(CustomBoxCollider collider) 
    {
        collidersInWorld.Add(collider);
    }
}
