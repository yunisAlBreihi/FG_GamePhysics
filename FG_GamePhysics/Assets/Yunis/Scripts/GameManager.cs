using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to a group object, with spawnPoints as children.")]
    private GameObject spawnPointGroup = null;

    private SpawnPoint[] spawnPoints = null;
    private WindHandler windHandler = null;
    private WindArrow windArrow = null;

    private int spawnPointIndex = 0;
    private int lastSpawnPointIndex = 0;

    private void Awake()
    {
        //Find the windArrow child.
        windArrow = GetComponentInChildren<WindArrow>();
        if (windArrow == null)
            Debug.LogError("You need a Wind Arrow as a child to the game manager!");

        //Find Wind handler in the world. If found, randomize the wind direction.
        windHandler = FindObjectOfType<WindHandler>();
        if (windHandler == null)
            Debug.LogError("You need a Wind Handler in the game!");
        else
            ChangeWind();

        //Check if theres a spawn group. If so, get the spawn points.
        if (spawnPointGroup == null)
            Debug.LogError("You need to assign a spawn point group!");
        else
            spawnPoints = spawnPointGroup.GetComponentsInChildren<SpawnPoint>();
    }

    /// <summary>
    /// Starts a new round. Changes the wind when doing so.
    /// </summary>
    /// <returns></returns>
    public void NewRound()
    {
        ChangeWind();
    }

    /// <summary>
    /// Randomizes the wind direction. Also applies that to the wind arrow indicator.
    /// </summary>
    /// <returns></returns>
    public void ChangeWind()
    {
        windHandler.RandomizeWindVelocity();
        windArrow.ChangeDirection(windHandler.WindVelocity);
    }

    /// <summary>
    /// Returns a new randomized spawn point position.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomSpawnPointPosition()
    {
        //If the same position was chosen as the last, keep re-doing until you get a new one.
        while (spawnPointIndex == lastSpawnPointIndex)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
        }
        lastSpawnPointIndex = spawnPointIndex;

        return spawnPoints[spawnPointIndex].transform.position;
    }
}
