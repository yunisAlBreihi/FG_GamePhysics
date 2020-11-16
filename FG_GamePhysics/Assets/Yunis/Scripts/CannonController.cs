using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private CustomPhysics bulletPrefab = null;
    [SerializeField] private float fireBulletCooldown = 1.0f;
    [SerializeField] private float rotationSpeed = 300.0f;
    [SerializeField] private float maxImpulseForce = 10000.0f;

    private GameObject canonPivot = null;
    private GameObject canonPipe = null;
    private CustomPhysics bullet = null;

    private const string mouseX = "Mouse X";
    private const string mouseY = "Mouse Y";
    private const string fire = "Fire";

    float fireBulletTimer = 1.0f;

    private bool holdingFire = false;

    private float firePercent = 0.0f;
    private float maxFirePower = 1.0f;

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
            canonPipe.transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis(mouseY) * rotationSpeed * Time.deltaTime, Vector3.right);
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
        }
        ClampRot(10.0f, 85.0f);

        if (fireBulletTimer >= fireBulletCooldown)
        {
            if (Input.GetAxisRaw(fire) > 0 && holdingFire == false)
            {
                holdingFire = true;
            }
            else if (Input.GetAxisRaw(fire) <= 0 && holdingFire == true)
            {
                float fireForce = 0.0f;
                float minFirePower = maxImpulseForce / 3.0f;

                if (maxImpulseForce * firePercent / maxFirePower < minFirePower)
                {
                    fireForce = minFirePower;
                }
                else
                {
                    fireForce = maxImpulseForce * firePercent / maxFirePower;
                }

                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.ApplyImpulse(canonPipe.transform.up * fireForce);
                firePercent = 0.0f;
                fireBulletTimer = 0.0f;
                holdingFire = false;
            }

            if (holdingFire == true)
            {
                if (firePercent <= maxFirePower)
                {
                    firePercent += Time.deltaTime;
                }
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
