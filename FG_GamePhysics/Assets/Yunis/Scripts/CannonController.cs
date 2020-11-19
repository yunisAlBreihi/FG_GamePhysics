using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField, Tooltip("The physics object to fire with the canon.")]
    private CustomPhysics physicsObjectToFirePrefab = null;

    [SerializeField, Range(5.0f, 85.0f), Tooltip("The minimum vertical canon rotation.")]
    private float minCanonRotation = 10.0f;

    [SerializeField, Range(5.0f, 85.0f), Tooltip("The maximum vertical canon rotation.")]
    private float maxCanonRotation = 85.0f;

    [SerializeField, Range(0.1f, 5.0f), Tooltip("The time before you can fire again.")]
    private float fireBulletCooldown = 1.0f;

    [SerializeField, Range(50.0f, 1000.0f), Tooltip("Rotation speed of the canon.")]
    private float rotationSpeed = 300.0f;

    [SerializeField, Range(1000.0f, 100000.0f), Tooltip("Max firing power of the canon, when fully charged.")]
    private float maxImpulseForce = 20000.0f;

    private SpriteRenderer fireMeter = null;
    private GameObject canonPivot = null;
    private GameObject canonPipe = null;
    private CustomPhysics bullet = null;

    //Const of inputs.
    private const string mouseX = "Mouse X";
    private const string mouseY = "Mouse Y";
    private const string fire = "Fire";

    private float fireBulletTimer = 1.0f;
    private bool holdingFire = false;

    //Canon fire power.
    private float firePercent = 0.0f;
    private float minFirePercent = 0.0f;
    private float maxFirePercent = 1.0f;

    private void Awake()
    {
        //Finds the meter for firing.
        fireMeter = GetComponentInChildren<SpriteRenderer>();
        if (fireMeter == null)
            Debug.LogError(fireMeter + ": You need a fire meter on this object!");

        //Find the cannon pivot child object
        foreach (Transform child in transform)
        {
            if (child.tag == "CanonPivot")
            {
                canonPivot = child.gameObject;
                break;
            }
        }
        if (canonPivot == null)
            Debug.LogWarning("Could not find a child object with CanonPivot tag!");

        //Find the canon pipe child object.
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
            Debug.LogWarning("Could not find a child object with CanonPipe tag!");

        //Checks to see if a bullet prefab has been set.
        if (physicsObjectToFirePrefab == null)
            Debug.LogError("You need to set a bullet prefab!");

        minFirePercent = maxImpulseForce / 3.0f;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            canonPivot.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis(mouseX) * rotationSpeed * Time.deltaTime, Vector3.up);
            canonPipe.transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis(mouseY) * rotationSpeed * Time.deltaTime, Vector3.right);
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse up");
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Clamp vertical rotation of the canon pipe.
        ClampRot(minCanonRotation, maxCanonRotation);

        if (fireBulletTimer >= fireBulletCooldown)
        {
            //Hold the fire button
            if (Input.GetAxisRaw(fire) > 0 && holdingFire == false)
            {
                holdingFire = true;
            }
            //When let go, fires a bullet.
            else if (Input.GetAxisRaw(fire) <= 0 && holdingFire == true)
            {
                float fireForce = 0.0f;

                //Sets a minimum firing velocity so the bullet doesn't spawn without velocity.
                if (maxImpulseForce * firePercent / maxFirePercent < minFirePercent)
                {
                    fireForce = minFirePercent;
                }
                else
                {
                    fireForce = maxImpulseForce * firePercent / maxFirePercent;
                }

                FireBullet(fireForce);
                ResetFire();
            }
            ChargeFire();
        }
        else
        {
            fireBulletTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Charges the fire meter.
    /// </summary>
    /// <returns></returns>
    private void ChargeFire()
    {
        if (holdingFire == true)
        {
            if (firePercent <= maxFirePercent)
            {
                firePercent += Time.deltaTime;
                fireMeter.transform.localScale = new Vector3(firePercent * 50.0f,
                                                             fireMeter.transform.localScale.y,
                                                             fireMeter.transform.localScale.z);
            }
        }
    }

    /// <summary>
    /// Fire a bullet forward form the canon.
    /// </summary>
    /// <returns></returns>
    private void FireBullet(float fireForce)
    {
        bullet = Instantiate(physicsObjectToFirePrefab, transform.position, Quaternion.identity);
        bullet.ApplyImpulse(canonPipe.transform.up * fireForce);
    }

    /// <summary>
    /// Reset the values of firing the canon.
    /// </summary>
    /// <returns></returns>
    private void ResetFire()
    {
        firePercent = 0.0f;
        fireBulletTimer = 0.0f;
        holdingFire = false;
    }

    /// <summary>
    /// Clamps rotations so you can't turn the canon pipe in impossible ways.
    /// </summary>
    /// <returns></returns>
    void ClampRot(float minAngle, float maxAngle)
    {
        canonPipe.transform.localRotation = Quaternion.Euler(Mathf.Clamp(canonPipe.transform.localRotation.eulerAngles.x, minAngle, maxAngle),
                                                                         0.0f,
                                                                         0.0f);
    }
}
