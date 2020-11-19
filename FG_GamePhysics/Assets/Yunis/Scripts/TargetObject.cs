using UnityEngine;

public class TargetObject : MonoBehaviour
{
    private GameManager gameManager = null;

    private void Awake()
    {
        //Find game manager in the world.
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.Log(name + ": You need a Game Manager in the world!");
    }

    private void Start()
    {
        //Sets a random position at start
        ChangePosition(gameManager.GetRandomSpawnPointPosition());
    }

    /// <summary>
    /// Changes the transform.position to a specified new position.
    /// </summary>
    /// <returns></returns>
    public void ChangePosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// When Target is destroyed, it will get a new spawn point position.
    /// </summary>
    /// <returns></returns>
    public void Destroy()
    {
        ChangePosition(gameManager.GetRandomSpawnPointPosition());
        gameManager.NewRound();
    }
}
