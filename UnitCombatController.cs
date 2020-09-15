using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatController : MonoBehaviour
{
    private float attackTimer;
    public UnitAttackStats unitAttackStats;
    public float health;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        health = unitAttackStats.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }
    public void Attack()
    {
        if (attackTimer >= unitAttackStats.attackSpeed)
        {

            GameController.UnitTakeDamage(this, GetComponent<EnemyController>().GetCurrentTargetCombat());
            attackTimer = 0;
        }
    }
    public void TakeDamage(UnitCombatController attacker, float damage)
    {
        StartCoroutine(Flasher(GetComponent<Renderer>().material.color));
        health -= damage;
        anim.SetTrigger("Hit");
    }
    IEnumerator Flasher(Color defaultColor)
    {
        Renderer render = GetComponent<Renderer>();
        for (int i = 0; i < 2; i++)
        {
            render.material.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            render.material.color = defaultColor;
        }
    }
}
