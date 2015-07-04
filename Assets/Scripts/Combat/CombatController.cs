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

    public HeroCombat_Aria ariaCombat;
    public HeroCombat_Brio brioCombat;
    public HeroCombat_Cadence cadenceCombat;

    public HeroCharacter selectedCharacter;
    public HeroAction selectedAction;
    public int selectedTargetId;

    private int turn;

    void Start() {
        turn = 0;

        activeHeroes = new _HeroCombat[3] { ariaCombat, brioCombat, cadenceCombat };
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            advanceTurn();
        }
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

        turn++;
    }
}
