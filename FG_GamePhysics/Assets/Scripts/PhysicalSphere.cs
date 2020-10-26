using UnityEngine;

public class PhysicalSphere : MonoBehaviour
{
    Vector3 position = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    float mass = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        AddForce(Vector3.up * 150.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void AddForce(Vector3 force)
    {
        Vector3 acc = force * (1 / mass);

        velocity += (acc * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 gravityForce = mass * Physics.gravity;
        transform.position += (velocity + gravityForce) * Time.deltaTime;
    }
}