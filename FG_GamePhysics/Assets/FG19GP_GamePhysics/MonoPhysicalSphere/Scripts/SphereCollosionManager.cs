using FutureGamesLib;
using System.Collections.Generic;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class SphereCollosionManager : MonoBehaviour, ISphersCollidable
    {
        List<MonoPhysicalSphere> spheres = new List<MonoPhysicalSphere>();

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

        public void BallFiredListener()
        {
            FindSpheres();
        }

        private void Start()
        {
            FindSpheres();
        }

        private void Update()
        {
            ManageCollision();

            //Debug.Log(spheres.Count);
        }

        private void ManageCollision()
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                MonoPhysicalSphere a = spheres[i];
                for (int j = 0; j < spheres.Count; j++)
                {
                    MonoPhysicalSphere b = spheres[j];
                    if (a == b)
                        continue;

                    Collide(a, b);
                }
            }
        }

        private void Collide(MonoPhysicalSphere a, MonoPhysicalSphere b)
        {
            //if (a == null || b == null)
            //    return;

            if (a.IsLastColliderEqual(b) || b.IsLastColliderEqual(a))
                return;

            a.SetLastCollider(b);
            b.SetLastCollider(a);

            float dot = Vector3.Dot(a.Velocity, b.Velocity);
            float dist = Vector3.Distance(a.transform.position, b.transform.position);
            float radiusSum = a.Radius + b.Radius;
            if (dist > radiusSum)
                return;

            //Debug.Log("Collision");

            Vector3 aToB = (b.transform.position - a.transform.position).normalized;

            Vector3 pushFromA = radiusSum * aToB;

            // position correction
            b.transform.position = a.transform.position + 1f * pushFromA;
            //a.transform.position = b.transform.position - 0.5f * pushFromA;

            TwoDimensionReflection(a, b);
        }

        private void TwoDimensionReflection(MonoPhysicalSphere a, MonoPhysicalSphere b)
        {
            float massSum = a.mass + b.mass;
            float doubleMassA = a.mass * 2f;
            float doubleMassB = b.mass * 2f;

            float doubleAOverSum = doubleMassA / massSum;
            float doubleBOverSum = doubleMassB / massSum;

            Vector3 vAMinusVB = a.Velocity - b.Velocity;
            Vector3 vBMinusVA = -vAMinusVB;

            Vector3 posAMinusPosB = a.transform.position - b.transform.position;
            Vector3 posBMinusPosA = -posAMinusPosB;

            float posesSquare = Vector3.Dot(posAMinusPosB, posAMinusPosB);

            float dotA = Vector3.Dot(vAMinusVB, posAMinusPosB);
            float dotB = Vector3.Dot(vBMinusVA, posBMinusPosA);

            a.Velocity -= doubleBOverSum * (dotA / posesSquare) * posAMinusPosB;
            b.Velocity -= doubleAOverSum * (dotB / posesSquare) * posBMinusPosA;
        }

        void OnDimensionReflection(MonoPhysicalSphere a, MonoPhysicalSphere b)
        {
            float massDiff = a.mass - b.mass;
            float massSum = a.mass + b.mass;
            float doubleMassA = a.mass * 2f;
            float doubleMassB = b.mass * 2f;

            float diffOverSum = massDiff / massSum;
            float doubleAOverSum = doubleMassA / massSum;
            float doubleBOverSum = doubleMassB / massSum;

            Vector3 diffOverSumTimesVa = diffOverSum * a.Velocity;
            Vector3 diffOverSumTimesVb = -diffOverSum * b.Velocity;

            Vector3 doubleAOverSumTimesVa = doubleAOverSum * a.Velocity;
            Vector3 doubleBOverSumTimesVb = doubleBOverSum * b.Velocity;

            a.Velocity = diffOverSumTimesVa + doubleBOverSumTimesVb;
            b.Velocity = doubleAOverSumTimesVa + diffOverSumTimesVb;
        }

        public void FindSpheres()
        {
            spheres = new List<MonoPhysicalSphere>(FindObjectsOfType<MonoPhysicalSphere>());
        }

        public void SomeOneHasGone(string obj)
        {
            //Debug.Log(typeof(MonoPhysicalSphereLifeTime).ToString());
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
    }
}