using UnityEngine;
using System.Collections;

public abstract class _CombatParticipant : MonoBehaviour {

    public Transform hpBar;
    public Transform mpBar;

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

        if (hpBar != null) {
            hpBar.localScale = new Vector3(currentHp * 1f / maxHp, 1, 1);
        }
    }

    public void heal(int amount) {
        Debug.Log(gameObject.name + " was healed for " + amount + " health.");
        currentHp = Mathf.Min(currentHp + amount, maxHp);

        if (hpBar != null) {
            hpBar.localScale = new Vector3(currentHp * 1f / maxHp, 1, 1);
        }
    }

    public void useMP(int amount) {
        Debug.Log(gameObject.name + " has used " + amount + " MP.");
        currentMp = Mathf.Max(currentMp - amount, 0);

        if (mpBar != null) {
            mpBar.localScale = new Vector3(currentMp * 1f / maxMp, 1, 1);
        }
    }

    public void healMP(int amount) {
        Debug.Log(gameObject.name + " has recovered " + amount + " MP.");
        currentMp = Mathf.Max(currentMp + amount, maxMp);

        if (mpBar != null) {
            mpBar.localScale = new Vector3(currentMp * 1f / maxMp, 1, 1);
        }
    }

    public bool isDead() {
        return currentHp == 0;
    }

    public virtual bool hasEnoughMp() {
        return false;
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
