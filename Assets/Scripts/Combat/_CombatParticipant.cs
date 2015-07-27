using UnityEngine;
using System.Collections;

public abstract class _CombatParticipant : MonoBehaviour {

    public Transform hpBar;
    public Transform mpBar;

    public int maxHp;
    public int maxMp;
    public int minAttack;
    public int maxAttack;

    public GameObject particleBasicAttack;
    public GameObject particleSpecialAttack;
    public GameObject particleHurt;
    public GameObject particleDeath;

    protected int currentHp;
    protected int currentMp;

    protected Animator animator;
    protected CombatController combatController;

    void Start() {
        currentHp = maxHp;
        currentMp = maxMp;

        animator = GetComponent<Animator>();
        combatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
    }

    public void basicAttackOther(_CombatParticipant other) {
        basicAttack(other);

        if (particleBasicAttack != null) {
			if( particleBasicAttack.GetComponent<ParticleSystem>() != null) {
				particleBasicAttack.GetComponent<ParticleSystem>().Play();
			}
			if( particleBasicAttack.GetComponent<AudioSource>()) {
				particleBasicAttack.GetComponent<AudioSource>().Play();
			}
        }
    }

    public void specialMoveOther(_CombatParticipant other) {
        specialMove(other);

        if (particleSpecialAttack != null) {
			if( particleSpecialAttack.GetComponent<ParticleSystem>() != null) {
				particleSpecialAttack.GetComponent<ParticleSystem>().Play();
			}
			if( particleSpecialAttack.GetComponent<AudioSource>() != null) {
				particleSpecialAttack.GetComponent<AudioSource>().Play();
			}
		}
    }

    public void damage(int amount) {
        currentHp -= amount;
        if (currentHp <= 0) {
            currentHp = 0;
            die();

            if (particleDeath != null) {
				if( particleDeath.GetComponent<ParticleSystem>() != null) {
					particleDeath.GetComponent<ParticleSystem>().Play();
				}
				if( particleDeath.GetComponent<AudioSource>() != null) {
					particleDeath.GetComponent<AudioSource>().Play();
				}
			}
        }
        else {
            if (this is _HeroCombat) {
                animator.SetBool("damaged", true);
            }
            else if (this is _EnemyCombat) {
                animator.SetBool("Damage", true);
            }

            if (particleHurt != null) {
				if( particleHurt.GetComponent<ParticleSystem>() != null) {
					particleHurt.GetComponent<ParticleSystem>().Play();
				}
				if( particleHurt.GetComponent<AudioSource>() != null) {
					particleHurt.GetComponent<AudioSource>().Play();
				}
			}
        }
        hpBar.localScale = new Vector3(currentHp * 1f / maxHp, 1, 1);
    }

    public void heal(int amount) {
        Debug.Log(gameObject.name + " was healed for " + amount + " health.");
        currentHp = Mathf.Min(currentHp + amount, maxHp);

        hpBar.localScale = new Vector3(currentHp * 1f / maxHp, 1, 1);

        if (this is _HeroCombat) {
            if (isDead()) {
                // Do revive animation here
            }
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

    public bool canKillWithBasicAttack(_HeroCombat other) {
        return other.currentHp <= maxAttack;
    }

    public virtual bool hasEnoughMp() {
        return false;
    }

    protected abstract void die();

    protected virtual void basicAttack(_CombatParticipant other) {
        int damageAmount = Random.Range(minAttack, maxAttack);

        other.damage(damageAmount);
        Debug.Log(gameObject.name + " has done " + damageAmount + " damage to " + other.gameObject.name); 

        if (this is _HeroCombat) {
            animator.SetTrigger("basic");
        }
        else if (this is _EnemyCombat) {
            animator.SetTrigger("Attack");
        }


        //combatController.waitForAnimationFinished(animator);
        combatController.waitForParticipant(this, 4);
    }

    protected virtual void specialMove(_CombatParticipant other) {
        basicAttack(other);
    }
}
