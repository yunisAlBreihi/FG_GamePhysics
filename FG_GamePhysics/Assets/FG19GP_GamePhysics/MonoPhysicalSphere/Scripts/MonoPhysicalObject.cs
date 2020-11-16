using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoPhysicalObject : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField]
        Vector3 velocity = Vector3.zero;
        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = new Vector3(
                constrainMove_X ? 0f : value.x,
                constrainMove_Y ? 0f : value.y,
                constrainMove_Z ? 0f : value.z);
        }

        [SerializeField]
        float maxVelocity = 40f;

        [SerializeField]
        bool constrainMove_X = false;
        [SerializeField]
        bool constrainMove_Y = false;
        [SerializeField]
        bool constrainMove_Z = false;

        public float mass = 1f;

        [SerializeField]
        bool useGravity = false;

        public bool isVerlet = true;

        [SerializeField]
        float dynamicDracCoef = 1.2f;

        /// <summary>
        /// Euler integration
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector3 force)
        {
            Vector3 totalForce = useGravity ? force + mass * Physics.gravity : force;

            // f = m * a
            // a = f / m
            Vector3 acc = totalForce / mass;

            Integrate(acc, isVerlet);
        }

        void Integrate(Vector3 acc, bool isVerlet = false)
        {
            if (isVerlet == false) // use Euler
            {
                // v1 = v0 + a*detaTime
                Velocity = Velocity + acc * Time.fixedDeltaTime;

                // p1 = p0 + v*deltatime
                transform.position = transform.position + Velocity * Time.fixedDeltaTime;
            }
            else // use Verlet
            {
                transform.position +=
                    Velocity * Time.fixedDeltaTime +
                    acc * Time.fixedDeltaTime * Time.fixedDeltaTime * 0.5f;

                Velocity += acc * Time.fixedDeltaTime * 0.5f; // ??
            }
        }

        MonoPhysicalObject lastCollider = null;
        const float lastCollisionDelay = 0.01f;
        float lastCollisionDelayCounter = lastCollisionDelay;

        Collider myCollider = null;
        public Collider Collider
        {
            get
            {
                if (myCollider == null)
                    myCollider = GetComponent<Collider>();
                return myCollider;
            }
        }

        private void Start()
        {
            StartMethod();
        }

        protected virtual void StartMethod()
        {
        }

        private void Update()
        {
            UpdateMethod();
        }

        protected virtual void UpdateMethod()
        {
            HandleLastCollosion();
        }

        private void HandleLastCollosion()
        {
            if (lastCollider != null)
            {
                lastCollisionDelayCounter -= Time.deltaTime;
                if (lastCollisionDelayCounter <= 0f)
                {
                    lastCollisionDelayCounter = lastCollisionDelay;
                    lastCollider = null;
                }
            }
        }

        public bool IsLastColliderEqual(MonoPhysicalObject other)
        {
            if (lastCollider == null)
                return false;

            return other == lastCollider;
        }

        public void SetLastCollider(MonoPhysicalObject other)
        {
            lastCollisionDelayCounter = lastCollisionDelay;
            lastCollider = other;
        }

        private void FixedUpdate()
        {
            FixedUpdateMethod();
        }

        protected virtual void FixedUpdateMethod()
        {
            ApplyForce(new Vector3(0f, 0f, 0f));
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterMethod(other);
        }

        protected virtual void OnTriggerEnterMethod(Collider other)
        {

        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayMethod(other);
        }

        protected virtual void OnTriggerStayMethod(Collider other)
        {

        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitMethod(other);
        }

        protected virtual void OnTriggerExitMethod(Collider other)
        {
        }

        protected void LimitVelocity()
        {
            Velocity = Velocity.normalized * Mathf.Min(Velocity.magnitude, maxVelocity);
        }

        protected void ApplyDynamicDrag()
        {
            Vector3 dragForce = -DynamicDragAmount() * Velocity;

            ApplyForce(dragForce);
        }

        private float DynamicDragAmount()
        {
            return Vector3.Dot(Velocity, Velocity) * dynamicDracCoef / 1000f;
        }

        protected virtual Vector3 RelativeVelocity(MonoPhysicalObject other)
        {
            return other.Velocity - Velocity;
        }

        public Vector3 ClosestPointOnCollider(Vector3 point)
        {
            return Collider.ClosestPoint(point);
        }
    }
}