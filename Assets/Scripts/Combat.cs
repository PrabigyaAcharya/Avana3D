using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStat))]
public class Combat : MonoBehaviour
{

    public AudioSource attackSound;
    
    CharacterStat myStat;

    public float attackSpeed = 1f;

    private float attackCooldown = 0f;

    public float attackTime = .6f;

    public bool InCombat { get; private set; }

    const float combatCooldown = 5;
    float lastAttackTime;


    public event System.Action OnAttack; //void and no arguments

    private void Start()
    {
        myStat= GetComponent<CharacterStat>();
    }

    private void Update()
    {
        attackCooldown -=Time.deltaTime;
        if (Time.time - lastAttackTime > combatCooldown)
        {
            InCombat = false;
        }
    }

    public void Attack(CharacterStat targetStat) 
    {
        if (attackCooldown < 0f)
        {
            StartCoroutine(DoDamage(targetStat, attackTime));

            if (OnAttack != null)
            {
                OnAttack();
                AttackSound();
            }

            attackCooldown = 1f / attackSpeed;
        }
        InCombat= true;
        lastAttackTime= Time.time;
    }

    IEnumerator DoDamage (CharacterStat stat, float delay)
    {
        yield return new WaitForSeconds(delay);
        stat.TakeDamage(myStat.damage.getValue());

        if (stat.currentHelath<=0)
        {
            InCombat = false;
            attackSound.enabled= false;
        }

    }

    public void AttackSound()
    {
        attackSound.enabled= true;
    }
}
