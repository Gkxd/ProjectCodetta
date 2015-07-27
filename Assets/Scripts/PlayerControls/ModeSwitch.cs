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
    public bool randomEncounters = true;

    private float timeOfLastBattle = 0;
	private float waitTime = 0;
	public float battleWaitMin;
	public float battleWaitMax;

    void Awake() {
        instance = this;
    }


    void Start() {
        ariaMovement = aria.GetComponent<PlayerMovement>();

        brioMovement = brio.GetComponent<FollowPlayer>();
        cadenceMovement = cadence.GetComponent<FollowPlayer>();

        combatController.enabled = false;

		waitTime = Random.Range(battleWaitMin,battleWaitMax);
	}

    void Update() {
        if (cutsceneMode) {
		}

        else if (randomEncounters) {
            if (explorationMode) {
                float timeSinceLastBattle = Time.time - timeOfLastBattle;

                if (timeSinceLastBattle > waitTime) {
                    if (Random.Range(0f, 1f) < (timeSinceLastBattle - 10) * 0.1f) {
                        switchToCombatMode();
						waitTime = Random.Range(battleWaitMin,battleWaitMax);
                        Debug.Log("Random Encounter at time " + Time.time + ". Time elapsed since last battle: " + timeSinceLastBattle);
                    }
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

		gameObject.GetComponent<AudioSource>().Play ();
		aria.GetComponent<Animator>().SetBool("moving",false);
		aria.GetComponent<AudioSource>().volume = 0f;
		brio.GetComponent<Animator>().SetBool("moving",false);
	    cadence.GetComponent<Animator>().SetBool("moving",false);
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

    public static void triggerCombatMode(CombatTrigger trigger) {
        instance.explorationMode = false;
        instance.combatController.enabled = true;

        instance.combatController.generateEnemiesUsingCombatTrigger(trigger);
        instance.selectMenu.SetActive(true);

        instance.ariaMovement.enterCombat();
        instance.brioMovement.enterCombat();
        instance.cadenceMovement.enterCombat();
    }

    public static void triggerBossBattle(BossBattleTrigger trigger) {
        instance.explorationMode = false;
        instance.combatController.enabled = true;

        instance.combatController.generateBossBattle(trigger);
        instance.selectMenu.SetActive(true);

        instance.ariaMovement.enterCombat();
        instance.brioMovement.enterCombat();
        instance.cadenceMovement.enterCombat();
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
