using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleJump : MonoBehaviour
{
    public enum GameState { Menu=0, Lose=1, Win=2, Play=3 }
    public static TempleJump current;
    
    [Header("=== Current Game State ===")]
    [SerializeField] private GameState m_gameState = GameState.Menu;
    public GameState gameState => m_gameState;

    [Header("=== UI Elements ===")]
    [SerializeField] private Canvas m_menuCanvas;
    [SerializeField] private Canvas m_playingCanvas;
    [SerializeField] private Canvas m_loseCanvas;
    [SerializeField] private Canvas m_winCanvas;
    [SerializeField] private Canvas m_debugCanvas;

    [Header("=== Game Mechanic Controllers ===")]
    [SerializeField] private KeyCode m_interactionKey = KeyCode.Space;
    public KeyCode interactionKey => m_interactionKey;  // Make it accessible to onlookers
    [SerializeField] private GroundSpawner m_groundSpawner;
    [SerializeField] private PlayerInput m_player;
    [SerializeField] private bool m_canSetPlayMode = true;

    [Header("=== Debug Settings ===")]
    [SerializeField] private bool m_debugMode = false;
    [SerializeField] private KeyCode m_activateDebugKey = KeyCode.Return;
    [SerializeField] private KeyCode m_toggleMenuStateKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode m_toggleLoseStateKey = KeyCode.Alpha2;
    [SerializeField] private KeyCode m_toggleWinStateKey = KeyCode.Alpha3;
    [SerializeField] private KeyCode m_togglePlayStateKey = KeyCode.Alpha4;

    private void Awake() {
        current = this;
    }

    private void Start() {
        SetDebugMode(false);
        SetMenuState();
    }

    private void Update() {
        // We need to enable play state if the user has pressed the interaction key while in the menu scene
        // If we're not in the menu, then the interaction key that "starts" the play mode shouldn't work.
        // One caveat is that if the interaction key is already held down (i.e. they lost during gameplay), then...
        //      ... without a check for that, this will automatically default to being called.
        //      So we need to check if the player is transitioning from play to menu state, and use a boolean check here.
        if ((int)m_gameState <= 2 && m_canSetPlayMode && Input.GetKey(m_interactionKey)) SetPlayState();
        m_canSetPlayMode = !Input.GetKey(m_interactionKey);
        // Purely for debug purposes
        UpdateDebug();
    }

    private void UpdateDebug() {
        if (Input.GetKeyDown(m_activateDebugKey)) ToggleDebugMode();
        if (!m_debugMode) return;
        if (Input.GetKeyDown(m_toggleMenuStateKey)) SetMenuState();
        if (Input.GetKeyDown(m_toggleLoseStateKey)) SetLoseState();
        if (Input.GetKeyDown(m_toggleWinStateKey)) SetWinState();
        if (Input.GetKeyDown(m_togglePlayStateKey)) SetPlayState();
    }

    public void SetMenuState() {
        // First, let's set the game state itself to the menu state
        m_gameState = GameState.Menu;

        // Secondly, deactive any controllers or gameplay elements involved in the Play mode
        m_groundSpawner.gameObject.SetActive(false);
        m_player.gameObject.SetActive(false);

        // Thirdly, use the boolean check to prevent the interaction key from auto-triggering the play mode if held down.
        m_canSetPlayMode = !Input.GetKey(m_interactionKey);

        // Lastly, hide the playing canvas and show the menu canvas
        m_playingCanvas.gameObject.SetActive(false);
        m_menuCanvas.gameObject.SetActive(true);
    }
    public void SetLoseState() {
        // First, let's set the game state itself to the lose state
        m_gameState = GameState.Lose;

        // Secondly, deactive the ground spawner. We DON'T disable the player though.
        m_groundSpawner.gameObject.SetActive(false);
        m_player.transform.rotation = Quaternion.identity;

        // Thirdly, use the boolean check to prevent the interaction key from auto-triggering the play mode if held down.
        m_canSetPlayMode = !Input.GetKey(m_interactionKey);

        // Lastly, hide the playing canvas and show the menu + lose canvases
        m_playingCanvas.gameObject.SetActive(false);
        m_menuCanvas.gameObject.SetActive(true);
        m_loseCanvas.gameObject.SetActive(true);
    }
    public void SetWinState() {
        // First, let's set the game state itself to the lose state
        m_gameState = GameState.Win;

        // Secondly, deactive the ground spawner. We DON'T disable the player though.
        m_groundSpawner.gameObject.SetActive(false);
        m_player.transform.rotation = Quaternion.identity;

        // Thirdly, use the boolean check to prevent the interaction key from auto-triggering the play mode if held down.
        m_canSetPlayMode = !Input.GetKey(m_interactionKey);

        // Lastly, hide the playing canvas and show the menu + win canvases
        m_playingCanvas.gameObject.SetActive(false);
        m_menuCanvas.gameObject.SetActive(true);
        m_winCanvas.gameObject.SetActive(true);
    }
    public void SetPlayState() {
        // Firstly, let's set the game state itself to the playing state
        m_gameState = GameState.Play;

        // Secondly, activate any controllers or gameplay elements involved in the Play mode
        m_groundSpawner.gameObject.SetActive(true);
        m_player.gameObject.SetActive(true);
        m_player.ResetPlayer();

        // Lastly, hide the menu canvas and show the playing canvas
        m_menuCanvas.gameObject.SetActive(false);
        m_loseCanvas.gameObject.SetActive(false);
        m_winCanvas.gameObject.SetActive(false);
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
