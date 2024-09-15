using UnityEngine;

public class ConfettiOnCanvasActive : MonoBehaviour
{
    public ParticleSystem confettiSystem;
    [SerializeField] private Transform m_player;
    [SerializeField] private RectTransform m_canvasRect;
    [SerializeField] private RectTransform m_hatRect;

    void OnEnable()
    {
        if (confettiSystem != null)
        {
            confettiSystem.gameObject.SetActive(true);
            confettiSystem.Play();
        }

        Vector2 m_playerViewportPos = Camera.main.WorldToViewportPoint(m_player.position);
        Vector2 m_playerScreenPos = new Vector2(
            (m_playerViewportPos.x * m_canvasRect.sizeDelta.x) - (m_canvasRect.sizeDelta.x*0.5f),
            (m_playerViewportPos.y * m_canvasRect.sizeDelta.y) - (m_canvasRect.sizeDelta.y*0.5f)
        );
        m_hatRect.anchoredPosition = m_playerScreenPos + new Vector2(25f, 25f);
    }
    void OnDisable() {
        if (confettiSystem != null) {
            confettiSystem.Stop();
            confettiSystem.gameObject.SetActive(false);
        }
    }
}
