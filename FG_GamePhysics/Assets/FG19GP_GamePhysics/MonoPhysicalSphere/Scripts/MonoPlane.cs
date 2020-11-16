using FutureGamesLib;
using System.Collections.Generic;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoPlane : MonoBehaviour, ISphersCollidable
    {
        Vector3 Normal => transform.up;
        Vector3 Position => transform.position;

        List<MonoPhysicalSphere> spheres = new List<MonoPhysicalSphere>();

        const float staticVelocityLimit = 0.2f;
        const float deltaMoveCoef = 0.2f;
        const float correctedPostionCoef = 5f;

        MonoPhysicalObject ParentPhysicalObject => GetComponentInParent<MonoPhysicalObject>();

        Vector3 ParentVelocity => ParentPhysicalObject == null ? Vector3.zero : ParentPhysicalObject.Velocity;

        private void OnEnable()
        {
            MonoCar.ballFired += BallFiredListener;
            LifeTime.IAmGone += SomeOneHasGone;
        }

        private void OnDisable()
        {
            MonoCar.ballFired -= BallFiredListener;
            LifeTime.IAmGone -= SomeOneHasGone;
        }

        private void Start()
        {
            FindSpheres();
        }

        private void FixedUpdate()
        {
            UpdateSpheres();
        }

        private void UpdateSpheres()
        {
            foreach (MonoPhysicalSphere t in spheres)
            {
                if (t.onPlane == this)
                    Choc(t, GlobalPhysicsParameters.chocEnergyDissipation);
            }
        }

        float Distance(MonoPhysicalSphere sphere)
        {
            Vector3 sphereToPlane = Position - sphere.transform.position;

            //Vector3 sphereToPlane = sphere.transform.position - Position;

            return Vector3.Dot(sphereToPlane, Normal);
        }

        Vector3 Projection(MonoPhysicalSphere sphere)
        {
            Vector3 sphereToProjection = Distance(sphere) * Normal;

            return sphere.transform.position + sphereToProjection;
        }

        bool IsColliding(MonoPhysicalSphere sphere)
        {
            if (WillBeCollision(sphere) == false)
                return false;

            return Distance(sphere) >= 0f || Mathf.Abs(Distance(sphere)) <= sphere.Radius;

            // for dynamic ball the WillBeCollision is preventing the multi-collision;
            //return WillBeCollision(sphere) && TouchingThePlane(sphere);
        }

        bool WillBeCollision(MonoPhysicalSphere sphere)
        {
            //Debug.Log("WillBeCollision: " + name);
            //return Vector3.Dot(sphere.Velocity, Normal) < 0f;
            return Vector3.Dot(RelativeVelocity(sphere), Normal) < 0f;
        }

        Vector3 CorrectedPosition(MonoPhysicalSphere sphere)
        {
            return Projection(sphere) + Normal * sphere.Radius;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="energyDissipation" the impact of the energy dissipation on the reflected velocity></param>
        public void Choc(MonoPhysicalSphere sphere, float energyDissipation = 0f)
        {
            //Debug.Log("choc: " + name);
            if (IsColliding(sphere) == false)
                return;

            //Debug.Log("Sphere plane collision on: " + name);
            //Debug.Log("Velocity Error: isVerlet: " + sphere.isVerlet + " " + sphere.ErrorVelocityOnTheGround());

            //sphere.Velocity = Vector3.Reflect(sphere.Velocity, Normal);

            if (IsSphereStaticOnPlane(sphere))
            {
                sphere.transform.position = CorrectedPosition(sphere);
                sphere.ApplyForce(-sphere.mass * Physics.gravity);
            }
            else // sphere is dynamic
            {
                sphere.transform.position = CorrectedPosition(sphere);
                InverseRelativeVelocity(sphere, Reflect(RelativeVelocity(sphere), energyDissipation));
                //sphere.Velocity = Reflect(RelativeVelocity(sphere), energyDissipation);
            }
        }

        Vector3 Reflect(Vector3 v, float energyDissipation = 0f)
        {
            Vector3 r = (v - 2f * Vector3.Dot(v, Normal) * Normal) * (1f - energyDissipation);

            //Debug.Log(r.y);
            return r;
        }

        bool IsSphereStaticOnPlane(MonoPhysicalSphere sphere)
        {
            bool lowVelocity = RelativeVelocity(sphere).magnitude < staticVelocityLimit;

            return lowVelocity && TouchingThePlane(sphere);
        }

        bool TouchingThePlane(MonoPhysicalSphere sphere)
        {
            // sphere speed
            // deltaTime
            float deltaMove = Mathf.Max(deltaMoveCoef * sphere.Radius, RelativeVelocity(sphere).magnitude * Time.fixedDeltaTime);
            return (CorrectedPosition(sphere) - sphere.transform.position).magnitude <= correctedPostionCoef * deltaMove;
        }

        Vector3 RelativeVelocity(MonoPhysicalObject other)
        {
            //Debug.Log("Sphere Vel: " + other.Velocity + " Car Vel: " + ParentVelocity);
            return other.Velocity - ParentVelocity;
        }

        void InverseRelativeVelocity(MonoPhysicalObject other, Vector3 vel)
        {
            other.Velocity = vel + 2f * ParentVelocity;
        }

        public void FindSpheres()
        {
            spheres = new List<MonoPhysicalSphere>(FindObjectsOfType<MonoPhysicalSphere>());
        }

        public void SomeOneHasGone(string obj)
        {
            if (obj == typeof(MonoPhysicalSphereLifeTime).ToString())
            {
                CleanSphersList();
            }
        }

        public void CleanSphersList()
        {
            for (int i = spheres.Count - 1; i > -1; i--)
            {
                if (spheres[i] != null)
                    continue;

                spheres.RemoveAt(i);
            }
        }

        public void BallFiredListener()
        {
            FindSpheres();
        }
    }
}