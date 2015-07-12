using UnityEngine;
using System.Collections;

public enum HeroCharacter {
    Aria = 0, Brio = 1, Cadence = 2
}

public enum HeroAction {
    Basic = 0, Special = 1
}

public class CombatController : MonoBehaviour {

    public _EnemyCombat[] activeEnemies;
    public _HeroCombat[] activeHeroes;

    public GameObject prefabDrone;
    public GameObject prefabSpider;
    public GameObject prefabBoss;

    public GameObject selectMenu;

    public Transform mainCamera;

    public Transform droneSpawn1;
    public Transform droneSpawn2;
    public Transform droneSpawn3;

    public Transform spiderSpawn1;
    public Transform spiderSpawn2;
    public Transform spiderSpawn3;

    public Transform bossSpawn;

    public HeroCombat_Aria ariaCombat;
    public HeroCombat_Brio brioCombat;
    public HeroCombat_Cadence cadenceCombat;

    public ModeSwitch modeSwitch;

    public HeroCharacter selectedCharacter;
    public HeroAction selectedAction;
    public int selectedTargetId;

    private int turn;
    private bool animationFinished;
    private Animator currentAnimator;

    void Start() {
        turn = 0;

        activeHeroes = new _HeroCombat[3] { ariaCombat, brioCombat, cadenceCombat };
        animationFinished = true;
    }

    void OnEnable() {
        turn = 0;
        animationFinished = true;
    }

    void Update() {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.T)) {
#else
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
#endif
            advanceTurn();
        }

        if (!animationFinished) {
            if (currentAnimator == null) {
                Debug.LogError("Something is missing an animator.");
            }

            if (currentAnimator.gameObject.GetComponent<_CombatParticipant>() is _EnemyCombat) {
                if (currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                    animationFinished = true;
                    selectMenu.SetActive(true);
                }
            }
            else if (currentAnimator.gameObject.GetComponent<_CombatParticipant>() is _HeroCombat) {
                if (currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Combat Idle")) {
                    animationFinished = true;
                    advanceTurn();
                }
            }
        }
    }

    public void generateEnemies() {

        int additionalEnemies = Random.Range(1, 3);

        GameObject newEnemy = GameObject.Instantiate<GameObject>(prefabDrone);
        newEnemy.transform.position = droneSpawn1.position;
        newEnemy.transform.rotation = droneSpawn1.rotation;

        if (additionalEnemies == 0) {
            activeEnemies = new _EnemyCombat[] { newEnemy.GetComponent<_EnemyCombat>() };
        }
        else {
            GameObject newEnemy2 = GameObject.Instantiate<GameObject>(prefabDrone);
            newEnemy2.transform.position = droneSpawn2.position;
            newEnemy2.transform.rotation = droneSpawn2.rotation;

            if (additionalEnemies == 1) {
                activeEnemies = new _EnemyCombat[] {
                    newEnemy.GetComponent<_EnemyCombat>(),
                    newEnemy2.GetComponent<_EnemyCombat>()
                };
            }
            else {
                GameObject newEnemy3 = GameObject.Instantiate<GameObject>(prefabDrone);
                newEnemy3.transform.position = droneSpawn3.position;
                newEnemy3.transform.rotation = droneSpawn3.rotation;

                activeEnemies = new _EnemyCombat[] {
                    newEnemy.GetComponent<_EnemyCombat>(),
                    newEnemy2.GetComponent<_EnemyCombat>(),
                    newEnemy3.GetComponent<_EnemyCombat>()
                };
            }
        }
    }

    // Returns false if selection is bad
    public bool makeSelectionFromHUD(int row, int col) {
        if (col == 0) {
            selectedCharacter = (HeroCharacter)row;
            selectedAction = HeroAction.Basic;

            selectedTargetId = selectEnemyByLooking();
        }
        else if (col == 1 && row != 2) {
            selectedCharacter = (HeroCharacter)row;
            selectedAction = HeroAction.Special;

            selectedTargetId = selectEnemyByLooking();

            if (!activeHeroes[(int)selectedCharacter].hasEnoughMp()) {
                return false;
            }
        }
        else if (col == 2) {
            selectedCharacter = HeroCharacter.Cadence;
            selectedAction = HeroAction.Special;
            selectedTargetId = row;

            if (!cadenceCombat.hasEnoughMp()) {
                return false;
            }
        }
        else {
            return false;
        }

        if (activeHeroes[(int)selectedCharacter].isDead()) {
            return false;
        }

        return true;
    }

    public int selectEnemyByLooking() {

        float minAngle = 9000;
        int minIndex = -1;

        for (int i = 0; i < activeEnemies.Length; i++) {
            if (activeEnemies[i].isDead()) continue;

            float angle = Vector3.Angle(activeEnemies[i].transform.position - mainCamera.position, mainCamera.forward);

            if (angle < minAngle) {
                minAngle = angle;
                minIndex = i;
            }

        }

        Debug.Log("Selected enemy: " + minIndex);
        return minIndex;
    }

    public void confirmSelection() {
        Debug.Log("Turn confirmed. Advancing...");
        advanceTurn();
    }

    public void waitForAnimationFinished(Animator animator) {
        animationFinished = false;
        currentAnimator = animator;
    }

    private void advanceTurn() {
        if (turn % 2 == 0) {
            _HeroCombat selectedCharacterCombat = activeHeroes[(int)selectedCharacter];

            if (selectedAction == HeroAction.Basic) {
                selectedCharacterCombat.basicAttackOther(activeEnemies[selectedTargetId]);
            }
            else {
                if (selectedCharacterCombat is HeroCombat_Cadence) {
                    selectedCharacterCombat.specialMoveOther(activeHeroes[selectedTargetId]);
                }
                else {
                    selectedCharacterCombat.specialMoveOther(activeEnemies[selectedTargetId]);
                }
            }
        }
        else {
            int activeEnemyId = getRandomLiveEnemy();
            _EnemyCombat activeEnemy = activeEnemies[activeEnemyId];

            bool specialAttack = Random.Range(0f, 1f) > 0.5f;
            _HeroCombat targetHero;

            if (!brioCombat.isDead() && !cadenceCombat.isDead()) {
                if (Random.Range(0f, 1f) > 0.5f) {
                    targetHero = brioCombat;
                }
                else {
                    targetHero = cadenceCombat;
                }
            }
            else if (!brioCombat.isDead()) {
                targetHero = brioCombat;
            }
            else if (!cadenceCombat.isDead()) {
                targetHero = cadenceCombat;
            }
            else {
                targetHero = ariaCombat;
            }

            if (specialAttack) {
                activeEnemy.specialMoveOther(targetHero);
            }
            else {
                activeEnemy.basicAttackOther(targetHero);
            }
        }

        bool combatFinished = true;
        for (int i = 0; i < activeEnemies.Length; i++) {
            if (!activeEnemies[i].isDead()) {
                combatFinished = false;
                break;
            }
        }
        if (combatFinished) {
            turn = 0;

            for (int i = 0; i < activeEnemies.Length; i++) {
                //GameObject.Destroy(activeEnemies[i].gameObject);
            }
            
            modeSwitch.switchToExplorationMode();
        }
        
        turn++;
    }
    private int getRandomLiveEnemy() {
        int index = Random.Range(0, activeEnemies.Length);

        while (activeEnemies[index].isDead()) {
            index = Random.Range(0, activeEnemies.Length);
        }

        return index;
    }
}
