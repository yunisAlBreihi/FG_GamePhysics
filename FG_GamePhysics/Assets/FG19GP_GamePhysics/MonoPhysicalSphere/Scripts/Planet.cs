using FutureGamesLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class Planet : MonoPhysicalObject, ISphersCollidable
    {
        List<Planet> planets = new List<Planet>();

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

        protected override void StartMethod()
        {
            FindSpheres();
        }

        protected override void UpdateMethod()
        {
            InteractWithFriends();
        }

        private void InteractWithFriends()
        {
            for (int i = 0; i < planets.Count; i++)
            {
                if (planets[i] == this)
                    continue;

                GoToPlanet(planets[i]);
            }
        }

        public void BallFiredListener()
        {
            FindSpheres();
        }

        public void CleanSphersList()
        {
            for (int i = planets.Count - 1; i > -1; i--)
            {
                if (planets[i] != null)
                    continue;

                planets.RemoveAt(i);
            }
        }

        public void FindSpheres()
        {
            planets = new List<Planet>(FindObjectsOfType<Planet>());
        }

        public void SomeOneHasGone(string obj)
        {
            //if (obj == typeof(PlanetLifeTime).ToString())
            //{
            //    CleanSphersList();
            //}
        }
    
        void GoToPlanet(Planet other)
        {
            //float distance = Vector3.Distance(transform.position, other.transform.position);

            Vector3 otherToMe = transform.position - other.transform.position;

            float d2 = Vector3.Dot(otherToMe, otherToMe);

            float force = GlobalPhysicsParameters.G * mass * other.mass / d2;

            ApplyForce(force * (-otherToMe.normalized));
        }
    }
}