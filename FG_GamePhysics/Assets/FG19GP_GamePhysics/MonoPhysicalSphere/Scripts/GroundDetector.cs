using UnityEngine;

namespace FutureGames.GamePhysics
{
    public class GroundDetector : MonoBehaviour
    {
        const float maxDistance = 5f;

        public bool Detect()
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * maxDistance, Color.cyan);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance) == false)
                return false;

            return hit.collider.GetComponent<MonoGround>() != null;
        }
    }
}