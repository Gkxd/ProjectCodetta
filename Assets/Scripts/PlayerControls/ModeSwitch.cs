using UnityEngine;
using System.Collections;

public class ModeSwitch : MonoBehaviour {

    public GameObject aria;
    public GameObject brio;
    public GameObject cadence;
    public CombatController combatController;

    private PlayerMovement ariaMovement;
    private FollowPlayer brioMovement;
    private FollowPlayer cadenceMovement;
    private Rigidbody playerRigidbody;

    public bool explorationMode = true;

    private float timeOfLastBattle = 0;


    void Start() {
        ariaMovement = aria.GetComponent<PlayerMovement>();
        playerRigidbody = aria.GetComponent<Rigidbody>();

        brioMovement = brio.GetComponent<FollowPlayer>();
        cadenceMovement = cadence.GetComponent<FollowPlayer>();

        combatController.enabled = false;
    }

    void Update() {
        if (explorationMode) {
            float timeSinceLastBattle = Time.time - timeOfLastBattle;

            if (timeSinceLastBattle > 10) {
                if (Random.Range(0f, 1f) < (timeSinceLastBattle - 10) * 0.1f) {
                    generateEnemies();
                    switchToCombatMode();

                    Debug.Log("Random Encounter at time " + Time.time + ". Time elapsed since last battle: " + timeSinceLastBattle);
                }
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
            if (explorationMode) {
                switchToCombatMode();
                Debug.Log("Entering combat mode (debug)...");
            }
            else {
                switchToExplorationMode();
                Debug.Log("Exiting combat mode (debug)...");
            }
        }
	}

    public void generateEnemies() {

    }

    public void switchToCombatMode() {
        explorationMode = false;

        ariaMovement.enterCombat();
        brioMovement.enterCombat();
        cadenceMovement.enterCombat();
    }

    public void switchToExplorationMode() {
        explorationMode = true;

        ariaMovement.exitCombat();
        brioMovement.exitCombat();
        cadenceMovement.exitCombat();

        timeOfLastBattle = Time.time;
    }
}
