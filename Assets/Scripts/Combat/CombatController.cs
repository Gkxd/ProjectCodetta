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

    void Start() {
        turn = 0;

        activeHeroes = new _HeroCombat[3] { ariaCombat, brioCombat, cadenceCombat };
    }

    void OnEnable() {
        turn = 0;
    }

    void Update() {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.T)) {
#else
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
#endif
            advanceTurn();
        }
    }

    public void generateEnemies() {
        GameObject newEnemy = GameObject.Instantiate<GameObject>(prefabDrone);

        newEnemy.transform.position = droneSpawn1.position;
        newEnemy.transform.rotation = droneSpawn1.rotation;

        activeEnemies = new _EnemyCombat[] { newEnemy.GetComponent<_EnemyCombat>() };
    }

    // Returns false if selection is bad
    public bool makeSelectionFromHUD(int row, int col) {
        if (col == 0) {
            selectedCharacter = (HeroCharacter)row;
            selectedAction = HeroAction.Basic;

            selectedTargetId = 0;
        }
        else if (col == 1 && row != 2) {
            selectedCharacter = (HeroCharacter)row;
            selectedAction = HeroAction.Special;

            selectedTargetId = 0;

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

    public void confirmSelection() {
        Debug.Log("Turn confirmed. Advancing...");
        advanceTurn();
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
            int activeEnemyId = (turn / 2) % activeEnemies.Length;
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
                GameObject.Destroy(activeEnemies[i].gameObject);
            }
            
            modeSwitch.switchToExplorationMode();
        }
        else if (turn % 2 == 1) {
            selectMenu.SetActive(true);
        }
        
        turn++;
    }
}
