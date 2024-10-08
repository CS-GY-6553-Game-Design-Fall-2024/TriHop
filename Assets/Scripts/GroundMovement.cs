using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GroundMovement : MonoBehaviour
{
    public static event RowDeletedDelegate OnRowDeleted;
    public delegate void RowDeletedDelegate(GameObject g, float x);

    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite[] m_tiles;
    [SerializeField] private static float speed = 5f;


    void Start() {
        m_spriteRenderer.sprite = m_tiles[Random.Range(0, m_tiles.Length)];
    }

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
