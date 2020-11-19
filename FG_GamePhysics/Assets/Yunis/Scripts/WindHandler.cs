using System.Collections.Generic;
using UnityEngine;

public class WindHandler : MonoBehaviour
{
    [SerializeField, Range(-1.0f, 1.0f), Tooltip("The minimum wind velocity. Keep this close to zero since it is pretty strong.")]
    private float minWindStrength = -0.1f;

    [SerializeField, Range(-1.0f, 1.0f), Tooltip("The minimum wind velocity. Keep this close to zero since it is pretty strong.")]
    private float maxWindStrength = 0.1f;

    private List<CustomPhysics> physicsObjectsInWorld = new List<CustomPhysics>();
    private Vector3 windVelocity = Vector3.zero;

    public Vector3 WindVelocity => windVelocity;

    private void Update()
    {
        //Adds wind velocity to the physics objects in the world.
        foreach (var physicsObject in physicsObjectsInWorld)
        {
            physicsObject.AddVelocity(windVelocity);
        }
    }

    /// <summary>
    /// Randomizes the wind velocity. only in the x and z values.
    /// </summary>
    /// <returns></returns>
    public void RandomizeWindVelocity()
    {
        windVelocity = new Vector3(Random.Range(minWindStrength, maxWindStrength), 0.0f, Random.Range(minWindStrength, maxWindStrength));
    }

    /// <summary>
    /// Adds a physics object to the manager list of objects.
    /// </summary>
    /// <returns></returns>
    public void AddPhysicsObjectToList(CustomPhysics physicsObject)
    {
        physicsObjectsInWorld.Add(physicsObject);
    }

    /// <summary>
    /// Removes a physics object to the manager list of objects.
    /// </summary>
    /// <returns></returns>
    public void RemovePhysicsObjectToList(CustomPhysics physicsObject)
    {
        if (physicsObjectsInWorld.Contains(physicsObject) == true)
        {
            physicsObjectsInWorld.Remove(physicsObject);
        }
    }
}
