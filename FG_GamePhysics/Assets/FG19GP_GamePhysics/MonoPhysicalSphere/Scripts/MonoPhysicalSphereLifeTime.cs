using FutureGamesLib;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoPhysicalSphereLifeTime : LifeTime
    {
        protected override void DestroyMe()
        {
            base.DestroyMe();

            IAmGone(GetType().ToString());

            //Debug.Log(GetType().ToString());
        }
    }
}