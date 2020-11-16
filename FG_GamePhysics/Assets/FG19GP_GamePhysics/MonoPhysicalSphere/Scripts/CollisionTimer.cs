using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class CollisionTimer
    {
        const float delay = 0.1f;
        float delayCounter = 0f;

        MonoPhysicalObject a = null;
        MonoPhysicalObject b = null;

        public CollisionTimer(MonoPhysicalObject a, MonoPhysicalObject b)
        {
            this.a = a;
            this.b = b;
        }

        public void Run()
        {
            delayCounter += Time.deltaTime;

            if (delayCounter >= delay)
            {
                delayCounter = 0f;
            }
        }
    }
}