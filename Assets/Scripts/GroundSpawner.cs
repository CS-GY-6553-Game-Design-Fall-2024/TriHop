using UnityEngine;
using System.Collections;

public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner Instance { get; private set; }
    public GameObject groundPrefab;
    public float gameDuration = 180f;
    public float initialSpeed = 2f;
    public float maxSpeed = 10f;
    public float speedIncreaseRate = 0.06f; // 0.06 reaches speed 10 in about 2 mins

    private float elapsedTime = 0f;
    public bool GameEnded = false;

    void Start()
    {
        // Create the ground on start screen
        for (int i = -10; i < 20; i++)
        {
            SpawnGroundRow(-2f, i);
            SpawnGroundRow(2f, i);
        }

        GroundMovement.SetSpeed(initialSpeed);
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
        Instantiate(groundPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    void OnEnable()
    {
        GroundMovement.OnRowDeleted += HandleRowDeleted;
    }

    void OnDisable()
    {
        GroundMovement.OnRowDeleted -= HandleRowDeleted;
    }

    void HandleRowDeleted(float x)
    {
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
        this.enabled = false;
    }
}
