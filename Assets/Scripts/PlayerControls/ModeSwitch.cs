using UnityEngine;
using System.Collections;

public class ModeSwitch : MonoBehaviour {

    private static ModeSwitch instance;

    public GameObject aria;
    public GameObject brio;
    public GameObject cadence;
    public GameObject selectMenu;
    public CombatController combatController;

    public GameObject textBox;
    public GameObject ariaHud;
    public GameObject brioHud;
    public GameObject cadenceHud;

    private PlayerMovement ariaMovement;
    private FollowPlayer brioMovement;
    private FollowPlayer cadenceMovement;

    public bool cutsceneMode = false;
    public bool explorationMode = true;

    private float timeOfLastBattle = 0;

    void Awake() {
        instance = this;
    }


    void Start() {
        ariaMovement = aria.GetComponent<PlayerMovement>();

        brioMovement = brio.GetComponent<FollowPlayer>();
        cadenceMovement = cadence.GetComponent<FollowPlayer>();

        combatController.enabled = false;
    }

    void Update() {
        if (cutsceneMode) {
            
        }
        else if (explorationMode) {
            float timeSinceLastBattle = Time.time - timeOfLastBattle;

            if (timeSinceLastBattle > 10) {
                if (Random.Range(0f, 1f) < (timeSinceLastBattle - 10) * 0.1f) {
                    switchToCombatMode();

                    Debug.Log("Random Encounter at time " + Time.time + ". Time elapsed since last battle: " + timeSinceLastBattle);
                }
            }
        }
	}

    public void switchOnCutsceneMode() {
        cutsceneMode = true;
        textBox.SetActive(true);

        ariaHud.SetActive(false);
        brioHud.SetActive(false);
        cadenceHud.SetActive(false);
    }

    public void switchOffCutsceneMode() {
        cutsceneMode = false;
        textBox.SetActive(false);

        ariaHud.SetActive(true);
        brioHud.SetActive(true);
        cadenceHud.SetActive(true);

        timeOfLastBattle = Time.time;
    }

    public void switchToCombatMode() {
        explorationMode = false;
        combatController.enabled = true;

        combatController.generateEnemies();
        selectMenu.SetActive(true);

        ariaMovement.enterCombat();
        brioMovement.enterCombat();
        cadenceMovement.enterCombat();
    }

    public void switchToExplorationMode() {
        explorationMode = true;
        combatController.enabled = false;

        selectMenu.SetActive(false);

        ariaMovement.exitCombat();
        brioMovement.exitCombat();
        cadenceMovement.exitCombat();

        timeOfLastBattle = Time.time;
    }

    public static bool isCutscene() {
        return instance.cutsceneMode;
    }

    public static void switchCutsceneOn() {
        instance.switchOnCutsceneMode();
    }

    public static void switchCutsceneOff() {
        instance.switchOffCutsceneMode();
    }
}
