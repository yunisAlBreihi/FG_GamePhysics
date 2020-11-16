using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    [SerializeField] private float mass = 1.0f;
    [SerializeField] private bool gravity = true;

    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (gravity == true)
        {
            ApplyGravity();
        }
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void ApplyGravity() 
    {
        velocity = velocity + (Physics.gravity * mass) * Time.deltaTime;
    }

    public void ApplyImpulse(Vector3 force) 
    {
        Vector3 acc = force / mass;
        velocity = velocity + acc * Time.deltaTime;
    }
}
