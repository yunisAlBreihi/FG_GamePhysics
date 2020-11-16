using System;
using UnityEngine;

namespace FutureGamesLib
{
    public class LifeTime : MonoBehaviour
    {
        public static Action<string> IAmGone = delegate { };

        [SerializeField]
        float life = 1f;

        void Start()
        {
            Invoke("DestroyMe", life);
        }

        protected virtual void DestroyMe()
        {
            DestroyImmediate(gameObject);
        }
    }
}