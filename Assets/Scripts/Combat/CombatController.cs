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

    private bool waitForParticipantFinished;
    private _CombatParticipant waitingParticipant;
    private float finishedWaitingTime;

    void Start() {
        turn = 0;

        activeHeroes = new _HeroCombat[3] { ariaCombat, brioCombat, cadenceCombat };
        //animationFinished = true;
        waitForParticipantFinished = true;
    }

    void OnEnable() {
        turn = 0;
        //animationFinished = true;
        waitForParticipantFinished = true;
    }

    void Update() {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.T)) {
#else
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
#endif
            //advanceTurn();
        }
        /*
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
        */

        if (!waitForParticipantFinished) {
            if (Time.time > finishedWaitingTime) {
                waitForParticipantFinished = true;

                if (waitingParticipant is _EnemyCombat) {
                    selectMenu.SetActive(true);
                }
                else if (waitingParticipant is _HeroCombat) {
                    advanceTurn();
                }
            }
        }

        int selectedEnemyId = selectEnemyByLooking();

        for (int i = 0; i < activeEnemies.Length; i++) {
            if (i == selectedEnemyId) {
                activeEnemies[i].selectEnemy();
            }
            else {
                activeEnemies[i].deselectEnemy();
            }
        }
    }

    public void generateEnemies() {

        int additionalEnemies = Random.Range(0, 3);

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

    public void generateEnemiesUsingCombatTrigger(CombatTrigger ct) {
        int additionalEnemies = Random.Range(1, 3);

        GameObject newEnemy = GameObject.Instantiate<GameObject>(prefabSpider);
        newEnemy.transform.position = ct.spiderSpawn1.position;
        newEnemy.transform.rotation = ct.spiderSpawn1.rotation;

        if (additionalEnemies == 0) {
            activeEnemies = new _EnemyCombat[] { newEnemy.GetComponent<_EnemyCombat>() };
        }
        else {
            GameObject newEnemy2 = GameObject.Instantiate<GameObject>(prefabSpider);
            newEnemy2.transform.position = ct.spiderSpawn2.position;
            newEnemy2.transform.rotation = ct.spiderSpawn2.rotation;

            if (additionalEnemies == 1) {
                activeEnemies = new _EnemyCombat[] {
                    newEnemy.GetComponent<_EnemyCombat>(),
                    newEnemy2.GetComponent<_EnemyCombat>()
                };
            }
            else {
                GameObject newEnemy3 = GameObject.Instantiate<GameObject>(prefabSpider);
                newEnemy3.transform.position = ct.spiderSpawn3.position;
                newEnemy3.transform.rotation = ct.spiderSpawn3.rotation;

                activeEnemies = new _EnemyCombat[] {
                    newEnemy.GetComponent<_EnemyCombat>(),
                    newEnemy2.GetComponent<_EnemyCombat>(),
                    newEnemy3.GetComponent<_EnemyCombat>()
                };
            }
        }
    }

    public void generateBossBattle(BossBattleTrigger bossTrigger) {
        activeEnemies = new _EnemyCombat[] {
            bossTrigger.GetComponent<_EnemyCombat>()
        };
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

        return minIndex;
    }

    public void confirmSelection() {
        advanceTurn();
    }

    public void waitForAnimationFinished(Animator animator) {
        animationFinished = false;
        currentAnimator = animator;
    }

    public void waitForParticipant(_CombatParticipant participant, float seconds) {
        waitForParticipantFinished = false;
        waitingParticipant = participant;
        finishedWaitingTime = Time.time + seconds;
    }

    private void advanceTurn() {
        if (turn % 2 == 0) {
            _HeroCombat selectedCharacterCombat = activeHeroes[(int)selectedCharacter];

            if (selectedAction == HeroAction.Basic) {
                selectedCharacterCombat.basicAttackOther(activeEnemies[selectedTargetId]);
                if (!(selectedCharacterCombat is HeroCombat_Aria)) {
                    selectedCharacterCombat.gameObject.transform.LookAt(activeEnemies[selectedTargetId].gameObject.transform);
                    selectedCharacterCombat.gameObject.transform.eulerAngles.Scale(Vector3.up);
                }
            }
            else {
                if (selectedCharacterCombat is HeroCombat_Cadence) {
                    selectedCharacterCombat.specialMoveOther(activeHeroes[selectedTargetId]);

                    selectedCharacterCombat.gameObject.transform.LookAt(activeHeroes[selectedTargetId].gameObject.transform);
                    selectedCharacterCombat.gameObject.transform.eulerAngles.Scale(Vector3.up);
                }
                else {
                    selectedCharacterCombat.specialMoveOther(activeEnemies[selectedTargetId]);

                    if (!(selectedCharacterCombat is HeroCombat_Aria)) {
                        selectedCharacterCombat.gameObject.transform.LookAt(activeEnemies[selectedTargetId].gameObject.transform);
                        selectedCharacterCombat.gameObject.transform.eulerAngles.Scale(Vector3.up);
                    }
                }
            }
        }
        else {
            int activeEnemyId = getRandomLiveParticipant(activeEnemies);
            _EnemyCombat activeEnemy = activeEnemies[activeEnemyId];

            bool specialAttack = Random.Range(0f, 1f) > 0.5f;
            _HeroCombat targetHero;

            if (activeEnemy.canKillWithBasicAttack(ariaCombat)) {
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
            }
            else {
                targetHero = activeHeroes[getRandomLiveParticipant(activeHeroes)];
            }

            if (specialAttack) {
                activeEnemy.specialMoveOther(targetHero);
            }
            else {
                activeEnemy.basicAttackOther(targetHero);
            }

            activeEnemy.gameObject.transform.LookAt(targetHero.transform);
            activeEnemy.transform.eulerAngles.Scale(Vector3.up);
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
                GameObject.Destroy(activeEnemies[i].gameObject);
            }

            if (brioCombat.isDead()) {
                brioCombat.heal(10);
            }

            if (cadenceCombat.isDead()) {
                cadenceCombat.heal(10);
            }

            modeSwitch.switchToExplorationMode();
        }
        
        turn++;
    }
    private int getRandomLiveParticipant(_CombatParticipant[] participants) {
        int numParticipants = 0;
        for (int i = 0; i < participants.Length; i++) {
            if (!participants[i].isDead()) {
                numParticipants++;
            }
        }

        if (numParticipants == 0) {
            return -1;
        }

        int[] liveParticipants = new int[numParticipants];

        for (int i = 0, j = 0; i < activeEnemies.Length; i++) {
            if (!activeEnemies[i].isDead()) {
                liveParticipants[j++] = i;
            }
        }

        int randomIndex = Random.Range(0, numParticipants);

        return liveParticipants[randomIndex];
    }
}
