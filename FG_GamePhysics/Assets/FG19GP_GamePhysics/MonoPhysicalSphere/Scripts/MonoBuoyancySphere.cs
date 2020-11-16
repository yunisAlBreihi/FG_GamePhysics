using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoBuoyancySphere : MonoPhysicalObject
    {
        [SerializeField]
        GameObject plane = null;

        protected override void FixedUpdateMethod()
        {
            //base.FixedUpdateMethod();

            ApplyBuoyancy();
        }

        void ApplyBuoyancy()
        {
            float planeHeight = plane.transform.position.y;
            float ballHeight = transform.position.y;

            float radius = transform.localScale.y * 0.5f;

            float h = radius - ballHeight + planeHeight;
            h = Mathf.Min(h, 2f * radius);

            Vector3 force = Vector3.zero;

            Debug.Log("h: " + h);

            if (h <= 0f)
            {
            }
            else if(h > 0f)
            {
                force = 
                    SubmergedVolume(h, radius) * GlobalPhysicsParameters.waterDensity * Mathf.Abs(Physics.gravity.y)
                    * Vector3.up + ViscosityForce();
                // f = v * RHOw * g
                // f = m * g
            }

            Debug.Log("f: " + force);
            ApplyForce(force);
        }

        /// <summary>
        /// molecular friction == dissipation of energy == absorbing themovment == stabelazing
        /// in opposit direction of movement
        /// </summary>
        /// <returns></returns>
        Vector3 ViscosityForce()
        {
            return -Velocity.normalized * GlobalPhysicsParameters.waterViscosity *
                Vector3.Dot(Velocity, Velocity);
        }

        float SubmergedVolume(float h, float radius)
        {
            float v = Mathf.PI * h * h * (radius - h / 3f);

            Debug.Log("v: " + v);

            return v;
        }
    }
}