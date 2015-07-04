using UnityEngine;
using System.Collections;

public abstract class _CombatParticipant : MonoBehaviour {

    public int maxHp;
    public int maxMp;
    public int minAttack;
    public int maxAttack;

    [Range(0.0f, 1.0f)]
    public float accuracy;

    protected int currentHp;
    protected int currentMp;

    void Start() {
        currentHp = maxHp;
        currentMp = maxMp;
    }

    public void basicAttackOther(_CombatParticipant other) {
        basicAttack(other);
    }

    public void specialMoveOther(_CombatParticipant other) {
        specialMove(other);
    }

    public void damage(int amount) {
        Debug.Log(gameObject.name + " has taken " + amount + " damage.");
        currentHp -= amount;
        if (currentHp <= 0) {
            currentHp = 0;
            die();
        }
    }

    public void heal(int amount) {
        Debug.Log(gameObject.name + " was healed for " + amount + " health.");
        currentHp = Mathf.Min(currentHp, maxHp);
    }

    public bool isDead() {
        return currentHp == 0;
    }

    protected abstract void die();

    protected virtual void basicAttack(_CombatParticipant other) {
        int damageAmount = Random.Range(minAttack, maxAttack);

        if (Random.Range(0f, 1f) < accuracy) {
            other.damage(damageAmount);
        }
        else {
            Debug.Log(gameObject.name + " has missed.");
        }
    }

    protected virtual void specialMove(_CombatParticipant other) {
        basicAttack(other);
    }
}
