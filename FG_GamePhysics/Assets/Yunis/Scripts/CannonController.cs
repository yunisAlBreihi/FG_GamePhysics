using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private CustomPhysics bulletPrefab = null;
    [SerializeField] private float fireBulletMaxTime = 1.0f;
    [SerializeField] private float rotationSpeed = 300.0f;

    private GameObject canonPivot = null;
    private GameObject canonPipe = null;

    private CustomPhysics bullet = null;

    const string mouseX = "Mouse X";
    const string mouseY = "Mouse Y";
    const string fire = "Fire";

    float fireBulletTimer = 1.0f;

    private void Awake()
    {
        //Find the cannon child object
        foreach (Transform child in transform)
        {
            if (child.tag == "CanonPivot")
            {
                canonPivot = child.gameObject;
                break;
            }
        }
        if (canonPivot == null)
        {
            Debug.LogWarning("Could not find a child object with CanonPivot tag!");
        }

        if (canonPivot != null)
        {
            foreach (Transform childOfChild in canonPivot.transform)
            {
                if (childOfChild.tag == "CanonPipe")
                {
                    canonPipe = childOfChild.gameObject;
                    break;
                }
            }
        }
        if (canonPipe == null)
        {
            Debug.LogWarning("Could not find a child object with CanonPipe tag!");
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("You need to set a bullet prefab!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            canonPivot.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis(mouseX) * rotationSpeed * Time.deltaTime, Vector3.up);
            canonPipe.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis(mouseY) * rotationSpeed * Time.deltaTime, Vector3.right);
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
        }
        ClampRot(10.0f, 85.0f);

        if (fireBulletTimer >= fireBulletMaxTime)
        {
            if (Input.GetAxisRaw(fire) > 0)
            {
                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.ApplyImpulse(canonPipe.transform.up * 2000.0f);

                fireBulletTimer = 0.0f;
            }
        }
        else
        {
            fireBulletTimer += Time.deltaTime;
        }
    }

    void ClampRot(float minAngle, float maxAngle)
    {
        canonPipe.transform.localRotation = Quaternion.Euler(Mathf.Clamp(canonPipe.transform.localRotation.eulerAngles.x, minAngle, maxAngle),
                                                                         0.0f,
                                                                         0.0f);
    }
}
