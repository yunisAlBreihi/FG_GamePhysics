using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    [SerializeField] private float mass = 1.0f;
    [SerializeField] private bool useGravity = true;
    [SerializeField] private bool drawVectorForce = false;

    private Vector3 velocity = Vector3.zero;

    public Vector3 Velocity => velocity;

    // Update is called once per frame
    void Update()
    {
        if (velocity != Vector3.zero)
        {
            if (useGravity == true)
            {
                ApplyGravity();
            }
            transform.position = transform.position + velocity * Time.deltaTime;
        }
    }

    private void ApplyGravity()
    {
        velocity = velocity + (Physics.gravity * mass) * Time.deltaTime;
    }

    public void Reflect(Vector3 normal, float energyDissipation = 0.0f)
    {
        if (velocity.magnitude > 5.0f)
        {
            velocity = (velocity - 2.0f * Vector3.Dot(velocity, normal) * normal) * (1.0f - energyDissipation);
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    public void ApplyImpulse(Vector3 force) 
    {
        Vector3 acc = force / mass;
        velocity = velocity + acc * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (drawVectorForce == true)
        {
            Gizmos.DrawLine(transform.position, velocity);
        }
    }
}
