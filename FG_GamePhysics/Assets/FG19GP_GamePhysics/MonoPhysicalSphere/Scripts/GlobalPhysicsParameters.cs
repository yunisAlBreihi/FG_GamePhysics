using UnityEngine;

namespace FutureGames.GamePhysics
{
    public static class GlobalPhysicsParameters
    {
        /// <summary>
        /// The effect of the dissipation on the velocity:
        /// if enter velocity = 1, reflected velocity = 0.95
        /// </summary>
        public const float chocEnergyDissipation = 0f;

        /// <summary>
        /// Universal gravity constant
        /// </summary>
        public const float G = 0.00000000006674f;

        public const float waterDensity = 1000f; // Kg/m3

        public const float massOfDiameterOneSphereSameWaterDEnsity = 523.5988f;

        public const float waterViscosity = 800f;
    }
}