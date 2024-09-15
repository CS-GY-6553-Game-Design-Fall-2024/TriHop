using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    [SerializeField] private RectTransform m_canvasRect;
    [SerializeField] private RectTransform m_frustrated;

    private void OnEnable() {
        // When this is active, we need to move the "frustrated" UI element to the position of the player
        Vector2 m_playerViewportPos = Camera.main.WorldToViewportPoint(m_player.position);
        Vector2 m_playerScreenPos = new Vector2(
            (m_playerViewportPos.x * m_canvasRect.sizeDelta.x) - (m_canvasRect.sizeDelta.x*0.5f),
            (m_playerViewportPos.y * m_canvasRect.sizeDelta.y) - (m_canvasRect.sizeDelta.y*0.5f)
        );
        m_frustrated.anchoredPosition = m_playerScreenPos + new Vector2(25f, 25f);
    }
}
