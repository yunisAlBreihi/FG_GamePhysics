using System;
using UnityEngine;

public class PhysicsSphere : MonoBehaviour
{
    [SerializeField]
    Vector3 velocity = Vector3.zero;
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    
    [SerializeField]
    float mass = 1f;

    public float Mass { get => mass; set => mass = value; }

    float drag = 0.1f;

    public bool isVerlet = true;

    [SerializeField]
    bool useGravity = true;

    public MonoPhysPlane plane = null;

    [SerializeField]
    float chockEnergyDissapation = 0.05f;
    public float Radius => transform.localScale.x * 0.5f;


    // Update is called once per frame
    void Update()
    {
        ApplyForce(new Vector3(0f,0f,0f));

        Vector3 hitPoint = plane.Projection(this);
        bool isColliding = plane.isColliding(this);

        Debug.DrawLine(transform.position, plane.Projection(this), Color.red);

        plane.Chock(this, chockEnergyDissapation);

        if (Input.GetKey(KeyCode.W))
        {
            ApplyForce(new Vector3(10.0f, 0f, 0f));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ApplyForce(new Vector3(-10.0f, 0f, 0f));
        }
    }
    void CorrectPosition(bool isColliding, Vector3 hitPoint)
    {
        if (isColliding == false)
            return;

        Vector3 correctedPosition = plane.CorrectedPosition(this);
        Debug.DrawLine(hitPoint, correctedPosition, Color.green);

        transform.position = correctedPosition;
    }

    /// <summary>
    /// Euler integration
    /// </summary>
    /// <param name="force"></param>
    public void ApplyForce(Vector3 force)
    {
        Vector3 totalForce = useGravity ? force + mass * Physics.gravity : force;

        //f = m * a
        //a = f / m 
        Vector3 acc = totalForce / mass;

        Integrate(acc, isVerlet);
    }

    void Integrate(Vector3 acc, bool isVerlet = false)
    {
        if (isVerlet == false)
        {
            //v1 = v0 + a * deltatime
            velocity = velocity + acc * Time.deltaTime;

            //p1 = p0 + v * deltatime
            transform.position = transform.position + velocity * Time.deltaTime;
        }
        else //use verlete
        {
            transform.position += velocity * Time.deltaTime + acc * Time.deltaTime * Time.deltaTime * 0.5f;
            velocity += acc * Time.deltaTime ;

     
        }
    }

    /// <summary>
    /// Assuming the ground is at y = 0 and the intitial velocity is 0
    /// </summary>
    /// <returns></returns>
    public float VelocityOnGround()
    {
        return Mathf.Sqrt(2f * Physics.gravity.magnitude * (transform.position.y - plane.transform.position.y));
    }
    public float ErrorVelocityOnGround()
    {
        return Mathf.Abs(velocity.magnitude - VelocityOnGround());
    }
}
