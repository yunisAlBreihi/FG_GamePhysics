using FutureGamesLib;
using FutureGamesLib.Physics;
using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class MonoTestPID : MonoBehaviour
    {
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
        float maxError = 10f;

        PIDController pID = null;

        [SerializeField]
        float targetY = 2f;

        private void Start()
        {
            pID = new PIDController((10f-gainP)/10f, -gainI, gainD/1000f, maxError);
        }

        private void FixedUpdate()
        {
            transform.position = transform.position.With(
                y: CorrectY());
        }

        float CorrectY()
        {
            return targetY + pID.Compute(transform.position.y - targetY);
        }
    }
}