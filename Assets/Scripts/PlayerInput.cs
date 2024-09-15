using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput current;
    public enum PlayerSide { Left=-1, Right=1 }
    
    [Header("=== Player Settings ===")]
    [SerializeField] private Collider m_collider;
    [SerializeField] private Transform m_leftTarget;
    [SerializeField] private Transform m_rightTarget;
    [SerializeField] private float m_jumpSpeed = 2f;
    [SerializeField] private float m_maxSize = 1f;
    [SerializeField] private float m_minSize = 0.75f;
    [SerializeField] private AnimationCurve m_sizeCurve;
    [SerializeField] private float m_loseSpinSpeed = 10f;
    [SerializeField] private float m_winSpinSpeed = 5f;

    // Private variables
    private bool m_jumping = false;
    private float m_totalJumpTime, m_jumpStartTime;
    private Vector3 m_startPos, m_endPos;

    private void Awake() {
        current = this;
    }

    public void ResetPlayer() {
        // If this is called, then we will have to:
        //  1. Reset the orientation of the player
        transform.rotation = Quaternion.identity;
        //  2. Reset the position of the player
        transform.position = new Vector3(2f, 0f, -0.9f);
        //  3. Reset the collider of the player
        m_collider.enabled = true;
    }

    // Update is called once per frame
    private void Update() {

        // Depending on what the current game mode state is, we have to adjust he behavior of this player
        switch(TempleJump.current.gameState) {
            // The game is currently playing
            case TempleJump.GameState.Play:
                // Check the player's current side, which is determined which side the x-position happens to be
                PlayerSide currentSide = CheckPlayerSide();
                // Check the player's input, which should tell us what the player's intended lane is
                PlayerSide intendedLane = CheckPlayerInput();

                // Check if there's a discrepancy between the intended lane and the player's current lane (`m_playerSide`)
                // If there is a discrepancy, and we're not jumping, then we need to initiate the jump mechanism.
                // Then, if we are SUPPOSED to be jumping, we update our position
                if (intendedLane != currentSide && !m_jumping) InitializeJump(intendedLane);
                if (m_jumping) JumpMovement();

                // Break out of switch statement
                break;

            // The lose state
            case TempleJump.GameState.Lose:
                // If the player has lost... we'll do a stupid spin that conveys frustration. We'll rotate the player around the z-axis
                transform.Rotate(Vector3.forward, m_loseSpinSpeed);

                // Break out of the switch statement
                break;

            // The win state
            case TempleJump.GameState.Win:
                // If the player has lost... we'll do a stupid spin that conveys frustration. We'll rotate the player around the z-axis
                transform.Rotate(Vector3.up, m_winSpinSpeed);

                // Break out of the switch statement
                break;

            // The menu state
            default:
                break;
        }

        // If we're currently in the play state (informed by `TempleJump`, then we perform all the jumps and the like)
        if (TempleJump.current.gameState == TempleJump.GameState.Play) {
            // Check the player's current side, which is determined which side the x-position happens to be
            PlayerSide currentSide = CheckPlayerSide();
            // Check the player's input, which should tell us what the player's intended lane is
            PlayerSide intendedLane = CheckPlayerInput();

            // Check if there's a discrepancy between the intended lane and the player's current lane (`m_playerSide`)
            // If there is a discrepancy, and we're not jumping, then we need to initiate the jump mechanism.
            if (intendedLane != currentSide && !m_jumping) InitializeJump(intendedLane);
            // Then, if we are SUPPOSED to be jumping, we update our position
            if (m_jumping) JumpMovement();
        }
        // If we're currently in the menu state, then we're in a situation where we have to 
        else {
        
        }
    }

    private PlayerSide CheckPlayerSide() {
        return (transform.position.x <= 0f) ? PlayerSide.Left : PlayerSide.Right;
    }

    private PlayerSide CheckPlayerInput() {
        // Check the condition of whatever the jump button we set.
        bool isHeldDown = Input.GetKey(TempleJump.current.interactionKey);

        // Based on the button condition, which side should the player be on?
        PlayerSide intendedLane = (isHeldDown) ? PlayerSide.Left : PlayerSide.Right;

        // Return the intended side.
        return intendedLane;
    }

    private void InitializeJump(PlayerSide intendedLane) {
        // When we "initialize" a jum-p, we need to know the following:
        //  1. Based on the jump speed (`m_jumpSpeed`), how much time is actually required?
        float distance = Vector3.Distance(m_leftTarget.position, m_rightTarget.position);
        m_totalJumpTime = distance / m_jumpSpeed;

        //  2. What are the start and end positions of your jump?
        if (intendedLane == PlayerSide.Right) {
            // We want to move from left to right
            m_startPos = m_leftTarget.position;
            m_endPos = m_rightTarget.position;
        } else {
            // We want to move from right to left
            m_startPos = m_rightTarget.position;
            m_endPos = m_leftTarget.position;
        }

        // 3. At what point in time did we start the jump? We use this to calculate the fraction of the jump journey the player is at, given a frame
        m_jumpStartTime = Time.time;

        // We've calculated all that we need: the total amount of time required to jump, when we started the jump, and the start/end positiosn of our jump
        // Now we just need to tell the system that we're jumping.
        m_jumping = true;
    }

    private void JumpMovement() {
        // We prevent jumping if we're not jumping
        if (!m_jumping) return;

        // How much of the fraction of the journey are we at? We clamp between 0 and 1
        float journeyFrac = Mathf.Clamp((Time.time - m_jumpStartTime) / m_totalJumpTime, 0f, 1f);
        
        // Based on this fraction, we lerp from start to end
        transform.position = Vector3.Lerp(m_startPos, m_endPos, journeyFrac);

        // Change the relative lcoal scale of the object based on the journey fraction and animation curve
        float m_localScale = m_minSize + m_sizeCurve.Evaluate(journeyFrac) * (m_maxSize - m_minSize);
        transform.localScale = new Vector3(m_localScale, m_localScale, m_localScale);
        
        // If we are sufficiently close to the target position (or if journeyFrac == 1f), then we stop the jump
        if (journeyFrac == 1f) {
            m_jumping = false;
        }
    }
	
    // We call this function if 
	private void GameOver() {
		//Time.timeScale = 1;
        m_collider.enabled = false;
		ObstacleGenerator.ResetObstacles();
		TempleJump.current.SetLoseState();
	}

	void OnTriggerEnter(Collider other) {
		// if the other object has a tag "obstacle", then we know that we need to force a game over state
        if(other.gameObject.tag == "obstacle") {
			GameOver();
            //Invoke("GameOver",0.03f);
			//Time.timeScale = 0.07f;
		}
	}
	
}
