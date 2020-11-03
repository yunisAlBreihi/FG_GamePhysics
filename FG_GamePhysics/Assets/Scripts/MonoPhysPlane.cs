using System;
using UnityEngine;

public class MonoPhysPlane : MonoBehaviour
{
    Vector3 Normal => transform.up;
    Vector3 Position => transform.position;
    public float Distance(PhysicsSphere sphere)
    {
        Vector3 sphereToPlane = Position - sphere.transform.position;

        return Vector3.Dot(sphereToPlane, Normal);
    }
        
    public Vector3 Projection(PhysicsSphere sphere)
    {
        Vector3 sphereToProjection = Distance(sphere) * Normal;

        return sphere.transform.position + sphereToProjection;
    }

    public bool isColliding(PhysicsSphere sphere)
    {
        if (WillBeCollision(sphere) == false)
        {
            return false;
        }
        return Distance(sphere)>=0 || Mathf.Abs(Distance(sphere)) <= sphere.Radius;
    }

    private bool WillBeCollision(PhysicsSphere sphere)
    {
        return Vector3.Dot(sphere.Velocity, Normal) < 0;
    }

    public Vector3 CorrectedPosition(PhysicsSphere sphere)
    {
        return Projection(sphere) + Normal * sphere.Radius;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sphere"></param>
    /// <param name="energyDissipation" the impact of the energy dissapation on the reflected velocity></param>
    public void Chock(PhysicsSphere sphere, float energyDissipation =0)
    {
        if (isColliding(sphere) == false)
        {
            return;
        }

        //Debug.Log("Velocity Error: isVerlet " + sphere.isVerlet + " " + sphere.ErrorVelocityOnGround());

        if (IsSphereStatic(sphere))
        {
            sphere.transform.position = CorrectedPosition(sphere);
            sphere.ApplyForce(-sphere.Mass * Physics.gravity);
        }
        else //sphere is dynamic 
        {
            sphere.Velocity = Reflect(sphere.Velocity, energyDissipation);
        }
    }

    private Vector3 Reflect(Vector3 velocity, float energyDissipation = 0)
    {
        return (velocity - 2f * Vector3.Dot(velocity, Normal) * Normal) * (1f-energyDissipation);
    }

    bool IsSphereStatic(PhysicsSphere sphere)
    {
        bool lowVelocity = sphere.Velocity.magnitude < 0.2f;
        bool touchingThePlane = (CorrectedPosition(sphere) - sphere.transform.position).magnitude < 0.02f;
        return lowVelocity && touchingThePlane;
    }
}
