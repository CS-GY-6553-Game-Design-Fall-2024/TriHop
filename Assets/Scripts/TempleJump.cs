using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleJump : MonoBehaviour
{
    public enum GameState { Menu, Play }
    public static TempleJump current;
    
    [Header("=== Current Game State ===")]
    [SerializeField] private GameState m_gameState = GameState.Menu;

    [Header("=== UI Elements ===")]
    [SerializeField] private Canvas m_menuCanvas;
    [SerializeField] private Canvas m_playingCanvas;
    [SerializeField] private Canvas m_debugCanvas;

    [Header("=== Game Mechanic Controllers ===")]
    [SerializeField] private KeyCode m_interactionKey = KeyCode.Space;
    public KeyCode interactionKey => m_interactionKey;  // Make it accessible to onlookers
    [SerializeField] private GroundSpawner m_groundSpawner;
    [SerializeField] private PlayerInput m_player;

    [Header("=== Debug Settings ===")]
    [SerializeField] private bool m_debugMode = false;
    [SerializeField] private KeyCode m_activateDebugKey = KeyCode.Return;
    [SerializeField] private KeyCode m_toggleMenuStateKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode m_togglePlayStateKey = KeyCode.Alpha2;

    private void Awake() {
        current = this;
    }

    private void Start() {
        SetDebugMode(false);
        SetMenuState();
    }

    private void Update() {
        // If we're not in the menu, then the space bar interaction that "starts" the play mode shouldn't work.
        if (m_gameState == GameState.Menu && Input.GetKeyDown(m_interactionKey)) SetPlayState();
        // Purely for debug purposes
        UpdateDebug();
    }

    private void UpdateDebug() {
        if (Input.GetKeyDown(m_activateDebugKey)) ToggleDebugMode();
        if (!m_debugMode) return;
        if (Input.GetKeyDown(m_toggleMenuStateKey)) SetMenuState();
        if (Input.GetKeyDown(m_togglePlayStateKey)) SetPlayState();
    }

    public void SetMenuState() {
        // First, let's set the game state itself to the menu state
        m_gameState = GameState.Menu;

        // Secondly, deactive any controllers or gameplay elements involved in the Play mode
        m_groundSpawner.gameObject.SetActive(false);
        m_player.gameObject.SetActive(false);

        // Lastly, hide the playing canvas and show the menu canvas
        m_playingCanvas.gameObject.SetActive(false);
        m_menuCanvas.gameObject.SetActive(true);
        
    }
    public void SetPlayState() {
        // Firstly, let's set the game state itself to the playing state
        m_gameState = GameState.Play;

        // Secondly, activate any controllers or gameplay elements involved in the Play mode
        m_groundSpawner.gameObject.SetActive(true);
        m_player.gameObject.SetActive(true);

        // Lastly, hide the menu canvas and show the playing canvas
        m_menuCanvas.gameObject.SetActive(false);
        m_playingCanvas.gameObject.SetActive(true);
    }
    public void SetDebugMode(bool setTo) {
        m_debugMode = setTo;
        m_debugCanvas.gameObject.SetActive(m_debugMode);
    }

    private void ToggleDebugMode() {
        m_debugMode = !m_debugMode;
        m_debugCanvas.gameObject.SetActive(m_debugMode);
    }
}
