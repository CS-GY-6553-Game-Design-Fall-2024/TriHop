using UnityEngine;
using System.Collections.Generic;

public class ObstacleGenerator : MonoBehaviour
{
	private static int lane1Counter = 0;
	private static int lane2Counter = 0;
	private static int lastObstacle = 0;
	public static void SpawnObstacle(GameObject ground, GameObject obstaclePrefab, Vector3 position, int counter)
	{
		bool isLane1 = position.x < 0; 
		if (Random.Range(0, 2) == 0)
		{
			if (isLane1 && lane1Counter > 2 && lane2Counter !=0 && lastObstacle != counter)
			{
				GameObject spawnedObstacle = Instantiate(obstaclePrefab, new Vector3(position.x, position.y, -0.5f), Quaternion.identity);
				spawnedObstacle.transform.SetParent(ground.transform);
				lane1Counter = 0;
				lastObstacle = counter;
			} else if (isLane1){
				lane1Counter++;
			}
			else if (!isLane1 && lane2Counter > 2 && lane1Counter !=0 && lastObstacle != counter)
			{
				GameObject spawnedObstacle = Instantiate(obstaclePrefab, new Vector3(position.x, position.y, -0.5f), Quaternion.identity);
				spawnedObstacle.transform.SetParent(ground.transform);
				lane2Counter = 0;
				lastObstacle = counter;
			} else if (!isLane1){
				lane2Counter++;
			}
		}
	}

	public static void ResetObstacles()
	{
		lane1Counter = 0;
		lane2Counter = 0;
	}
}
