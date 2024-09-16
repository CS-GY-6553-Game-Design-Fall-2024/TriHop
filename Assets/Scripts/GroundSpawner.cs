using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner Instance { get; private set; }

    [Header("=== References ===")]
    public GameObject groundPrefab;
    public GameObject obstaclePrefab;
    public Sprite[] m_groundTiles;

    [Header("=== Spawner Settings ===")]
    public float gameDuration = 180f;
    public float initialSpeed = 2f;
    public float maxSpeed = 10f;
    public float speedIncreaseRate = 0.06f; // 0.06 reaches speed 10 in about 2 mins
    [SerializeField] private List<GameObject> m_groundObjects;

    [SerializeField] private float elapsedTime = 0f;
    public float elapsedFraction => Mathf.Clamp(elapsedTime / gameDuration, 0f, 1f);
    public bool GameEnded = false;
    private int leftCounter = 0;
	private int rightCounter = 0;

    // Because this script can be activated and deactivate by `TempleJump`, we want to control this scripts on and off state via `OnEnable()` and `OnDisable()`.
    private void OnEnable() {
        // Initialize a new list of ground objects
        m_groundObjects = new List<GameObject>();

        // Set the ground movement to the initial speed + Add a listener to the OnRowDeleted event
        elapsedTime = 0f;
        GroundMovement.SetSpeed(initialSpeed);
        GroundMovement.OnRowDeleted += HandleRowDeleted;

        // Start spawning ground objects
        for (int i = -10; i < 20; i++) {   
            SpawnGroundRow(-2f, i);
            SpawnGroundRow(2f, i);
        }
    }

    // When this script is disabled by `TempleJump` upon switching to the Menu state, we should unhook any events and delete all ground objects
    private void OnDisable() {
        GroundMovement.OnRowDeleted -= HandleRowDeleted;
        while(m_groundObjects.Count > 0) {
            Destroy(m_groundObjects[0]);
            m_groundObjects.RemoveAt(0);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        // Debug.Log(elapsedTime);
        if (elapsedTime >= gameDuration)
        {
            EndGame();
        }
        else
        {
            float currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, elapsedTime / gameDuration);
            GroundMovement.SetSpeed(currentSpeed);
        }
    }

    void SpawnGroundRow(float x, float y)
    {
		if( x < 0 )
		{
			leftCounter++;
		}
		else
		{
			rightCounter++;
		}
        GameObject ground = Instantiate(groundPrefab, new Vector3(x, y, 0), Quaternion.identity);
        m_groundObjects.Add(ground);
		if ( y > 3)
		{
			if (x<0)
			{
				ObstacleGenerator.SpawnObstacle(ground, obstaclePrefab, new Vector3(x, y, 0), leftCounter);
			}
			else
			{
				ObstacleGenerator.SpawnObstacle(ground, obstaclePrefab, new Vector3(x, y, 0), rightCounter);
			}
		}
    }

    void HandleRowDeleted(GameObject g, float x)
    {
        if (m_groundObjects.Contains(g)) m_groundObjects.Remove(g);
        StartCoroutine(DelaySpawnRow(x, 15f));
    }

    IEnumerator DelaySpawnRow(float x, float y)
    {
        yield return null;
        SpawnGroundRow(x, y);
    }

    void EndGame()
    {
        GameEnded = true;
        TempleJump.current.SetWinState();
    }
}
