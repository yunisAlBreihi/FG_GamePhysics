using UnityEngine;

namespace FutureGamesLib.Physics
{
    public class PIDController
    {
        float errorOld = 0f;
        float errorOld2 = 0f;

        float errorSum = 0f;
        //float errorSum2 = 0f;

        // coefficiens
        float gainP = 1f;
        float gainI = 1f;
        float gainD = 1f;

        float maxError = 10f;

        public PIDController(float gainP, float gainI, float gainD, float maxError)
        {
            this.gainP = gainP;
            this.gainI = gainI;
            this.gainD = gainD;

            this.maxError = maxError;
        }

        public float Compute(float error)
        {
            float r = 0f;

            // P
            r += gainP * error;

            // I
            errorSum += Time.fixedDeltaTime * error;

            //errorSum = Mathf.Clamp(errorSum, -maxError, maxError);

            r += gainI * errorSum;

            // D
            float dError_Dt = (error - errorOld) / Time.fixedDeltaTime;

            errorOld2 = errorOld;
            errorOld = error;

            r += gainD * dError_Dt;

            return r;
        }
    }
}