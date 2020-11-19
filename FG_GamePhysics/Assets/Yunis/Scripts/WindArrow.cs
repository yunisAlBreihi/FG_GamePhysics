using UnityEngine;

public class WindArrow : MonoBehaviour
{
    [SerializeField, Tooltip("Text to display the velocity of the wind on.")]
    TextMesh windText = null;

    private void Awake()
    {
        //Check if the wind text reference has been set.
        if (windText == null)
            Debug.Log("No Wind Text connected!");
    }

    /// <summary>
    /// Changes the velocity values displayed on the text object.
    /// </summary>
    /// <returns></returns>
    private void ChangeText(Vector3 direction)
    {
        windText.text = direction.ToString("F2");
    }

    /// <summary>
    /// Changes the direction of the arrow based on the wind velocity.
    /// </summary>
    /// <returns></returns>
    public void ChangeDirection(Vector3 direction)
    {
        transform.localRotation = Quaternion.LookRotation(Vector3.down, direction.normalized);
        ChangeText(direction);
    }
}