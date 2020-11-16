using FutureGamesLib;
using FutureGamesLib.Physics;
using System;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoCar : MonoPhysicalObject
    {
        public static Action ballFired = delegate { };

        [Header("Car")]
        [SerializeField]
        float thrust = 10f;

        [SerializeField]
        float steering = 120f;

        [SerializeField]
        float steeringForceCoef = 10f;

        public float ballPush = 5f;

        [SerializeField]
        bool pushSphereFromCenter = false;

        GroundDetector[] groundDetectors = new GroundDetector[0];

        [Header("PID")]
        [SerializeField]
        [Range(0f, 10f)]
        float gainP = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        float gainI = 1f;

        [SerializeField]
        [Range(0f, 15f)]
        float gainD = 1f;

        [SerializeField]
        float maxPidError = 10f;

        PIDController pID = null;

        [SerializeField]
        float targetShiftY = 2f;

        Transform ground = null;
        //bool grounded = false;

        float ForwardInput()
        {
            return Input.GetAxis("Vertical");
        }

        float UpInput()
        {
            return Input.GetKey(KeyCode.Q) ? 1f : Input.GetKey(KeyCode.E) ? -1f : 0f;
        }

        float SteeringInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        [SerializeField]
        float jumpForce = 10f;

        bool jumping = false;

        [Header("Jump Curve")]
        [SerializeField]
        bool useJumpCurve = true;
        [SerializeField]
        AnimationCurve jumpCurve = null;
        [SerializeField]
        float jumpTime = 0.3f;
        float jumpTimeCounter = 0f;
        [SerializeField]
        float jumpHeight = 2f;

        float heightWhenJumpByCurveStart = 0f;

        [Header("Prefabs")]
        [SerializeField]
        GameObject shootBall = null;

        [SerializeField]
        float shootBallVelocity = 10f;

        [SerializeField]
        GameObject muzzel = null;

        protected override void StartMethod()
        {
            groundDetectors = transform.GetComponentsInChildren<GroundDetector>();

            ground = FindObjectOfType<MonoGround>().transform;

            //targetShiftY = transform.position.y - ground.position.y;

            pID = new PIDController((10f - gainP) / 10f, -gainI, gainD / 1000f, maxPidError);

            jumpTimeCounter = jumpTime;
        }

        protected override void UpdateMethod()
        {
            transform.Rotate(transform.up * SteeringInput() * steering * Time.deltaTime);

            //grounded = DetectGround();

            Jump();

            Shoot();
        }

        private void Shoot()
        {
            if (Input.GetMouseButtonDown(0) == false)
                return;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) == false)
                return;

            MonoPhysicalSphere physicalSphere =
                Instantiate(shootBall, muzzel.transform.position, muzzel.transform.rotation).
                GetComponent<MonoPhysicalSphere>();

            physicalSphere.Velocity = (hit.point - muzzel.transform.position).normalized * shootBallVelocity;

            ballFired();
        }

        protected override void FixedUpdateMethod()
        {
            base.FixedUpdateMethod();

            //ApplyForce(transform.forward * ForwardInput() * thrust);
            Velocity = Velocity.With(
                x: (transform.forward * ForwardInput() * thrust).x,
                z: (transform.forward * ForwardInput() * thrust).z);

            ApplyForce(transform.right * SteeringInput() * steering * steeringForceCoef * Velocity.magnitude);

            //ApplyForce(Vector3.zero.With(y:))
            //CorrectToGround();
        }

        private void Jump()
        {
            if (useJumpCurve)
            {
                JumpUsingCurve();
            }
            else // jumping force
            {
                //if (grounded == false)
                //{
                //    jumping = false;
                //    return;
                //}

                if (Input.GetKey(KeyCode.Space) == false)
                {
                    jumping = false;
                    return;
                }

                //Velocity += Vector3.zero.With(y: jumpVelocity);

                ApplyForce(Vector3.zero.With(y: jumpForce));
                jumping = true;
                Debug.Log(Velocity);
            }
        }

        private void JumpUsingCurve()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                StartJumpUsingCurve();
            }

            //StartCoroutine(JumpUsingCurveRoutine());

            if (jumping)
            {
                jumpTimeCounter -= Time.deltaTime;
                if (jumpTimeCounter <= 0f)
                {
                    FinsishJumpUsingCurve();
                }
                else // jump time not elapsed yet
                {
                    ProcessJumpUsingCurve();
                }
            }
        }

        private void StartJumpUsingCurve()
        {
            heightWhenJumpByCurveStart = transform.position.y;
            jumping = true;
        }

        private void ProcessJumpUsingCurve()
        {
            //Debug.Log("jumping curve process");

            float curveOut = jumpCurve.Evaluate((jumpTime - jumpTimeCounter) / jumpTime);

            transform.position = transform.position.With(
                y: heightWhenJumpByCurveStart + curveOut * jumpHeight);
        }

        private void FinsishJumpUsingCurve()
        {
            jumpTimeCounter = jumpTime;
            jumping = false;

            transform.position = transform.position.With(y: heightWhenJumpByCurveStart);
        }

        //private IEnumerator JumpUsingCurveRoutine()
        //{

        //    Debug.Log("jump curve routine");

        //    yield return new WaitForSeconds(Time.deltaTime);
        //}

        //private void CorrectToGround()
        //{
        //    if (grounded == false)
        //        return;

        //    //Debug.Log(ground.position.y + targetShiftY);
        //    //transform.position = transform.position.With(y: CorrectY(TargetY()));

        //    if (jumping == false)
        //    {
        //        // facking the upward reaction force from the ground
        //        Velocity = Velocity.With(y: 0f);
        //    }
        //    transform.position = transform.position.With(y: TargetY());
        //}

        float TargetY()
        {
            return ground.position.y + targetShiftY;
        }

        float CorrectY(float targetY)
        {
            return targetY + pID.Compute(transform.position.y - targetY);
        }

        public Vector3 ContactPointToPushSphere(Vector3 point)
        {
            return pushSphereFromCenter ? transform.position : ClosestPointOnCollider(point);
        }

        //bool DetectGround()
        //{
        //    foreach (GroundDetector t in groundDetectors)
        //    {
        //        if (t.Detect())
        //            return true;
        //    }

        //    return false;
        //}
    }
}