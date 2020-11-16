using System;
//using Unity.Rendering.HybridV2;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoPhysicalSphere : MonoPhysicalObject
    {
        public MonoPlane onPlane = null;

        public float Radius => transform.localScale.x * 0.5f;

        protected override void FixedUpdateMethod()
        {
            base.FixedUpdateMethod();

            //Vector3 hitPoint = plane.Projection(this);
            //bool isColliding = plane.IsColliding(this);

            //Debug.DrawLine(transform.position, hitPoint, isColliding ? Color.red : Color.blue);

            //CorrectPosition(isColliding, hitPoint);

            ApplyDynamicDrag();
        }

        //void CorrectPosition(bool isColliding, Vector3 hitPoint, MonoPlane plane)
        //{
        //    if (isColliding == false)
        //        return;

        //    Vector3 correctedPosition = plane.CorrectedPosition(this);
        //    Debug.DrawLine(hitPoint, correctedPosition, Color.green);

        //    transform.position = correctedPosition;
        //}


        /// <summary>
        /// Assuming the initial velocity is 0
        /// </summary>
        /// <returns></returns>
        public float VelocityOnGround(MonoPlane plane)
        {
            return Mathf.Sqrt(2f * Physics.gravity.magnitude * (transform.position.y - plane.transform.position.y));
        }

        //public float ErrorVelocityOnTheGround()
        //{
        //    return Mathf.Abs(velocity.magnitude - VelocityOnGround());
        //}

        protected override void OnTriggerEnterMethod(Collider other)
        {
            UpdateOnPlaneWhenEnter(other);

            DealWithCar(other);
        }

        private void DealWithCar(Collider other)
        {
            MonoCar car = other.GetComponent<MonoCar>();
            if (car == null)
                return;



            Vector3 pointToSphere = (transform.position - car.ContactPointToPushSphere(transform.position)).normalized;
            float dot = Vector3.Dot(pointToSphere, RelativeVelocity(car));

            Velocity = pointToSphere * dot * car.ballPush;
        }

        private void UpdateOnPlaneWhenEnter(Collider other)
        {
            onPlane = other.GetComponent<MonoPlane>();
        }

        protected override void OnTriggerStayMethod(Collider other)
        {
            UpdateOnPlaneWhenStay(other);
        }

        private void UpdateOnPlaneWhenStay(Collider other)
        {
            onPlane = other.GetComponent<MonoPlane>();
        }

        protected override void OnTriggerExitMethod(Collider other)
        {
            UpdateOnPlaneWhenExit(other);
        }

        private void UpdateOnPlaneWhenExit(Collider other)
        {
            MonoPlane plane = other.GetComponent<MonoPlane>();
            if (plane != null)
                onPlane = null;
        }
    }
}