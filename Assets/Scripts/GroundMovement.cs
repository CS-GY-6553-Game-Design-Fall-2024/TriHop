using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public static event RowDeletedDelegate OnRowDeleted;
    public delegate void RowDeletedDelegate(GameObject g, float x);

    private static float speed = 5f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
        // Debug.Log(speed);
    }

    void OnDestroy()
    {
        OnRowDeleted?.Invoke(this.gameObject, transform.position.x);
    }

    public static void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
