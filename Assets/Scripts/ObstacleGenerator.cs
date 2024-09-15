using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
	private static int lane1Counter = 0;
	private static int lane2Counter = 0;
	public static void SpawnObstacle(GameObject ground, GameObject obstaclePrefab, Vector3 position)
	{
		bool isLane1 = position.x < 0; 
		if (Random.Range(0, 2) == 0)
		{
			if (isLane1 && lane1Counter > 2 && lane2Counter !=0)
			{
				GameObject spawnedObstacle = Instantiate(obstaclePrefab, new Vector3(position.x, position.y, -0.5f), Quaternion.identity);
				spawnedObstacle.transform.SetParent(ground.transform);
				lane1Counter = 0;
			} else if (isLane1){
				lane1Counter++;
			}
			else if (!isLane1 && lane2Counter > 2 && lane1Counter !=0)
			{
				GameObject spawnedObstacle = Instantiate(obstaclePrefab, new Vector3(position.x, position.y, -0.5f), Quaternion.identity);
				spawnedObstacle.transform.SetParent(ground.transform);
				lane2Counter = 0;
			} else if (!isLane1){
				lane2Counter++;
			}
		}
	}

	public static void ResetObstacles()
	{
		Debug.Log("Resetting Obstacles");
	}
}
