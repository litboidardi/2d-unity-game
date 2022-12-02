using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour 
{
	public Animator animator;

	public Transform attackPoint;
	public LayerMask enemyLayers;

	public float attackRange = 0.5f;
	public int attackDamage = 40;

	public float attackRate = 2f;
	float nextAttackTime = 0f;

	void Update()
	{
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Attack();
				nextAttackTime = Time.time + 1f / attackRate;

			}
		}
	}

	void Attack()
	{
		// prehrá animáciu útoku
		animator.SetTrigger("Attack");
		// detekcia nepriateľov v dosahu útoku
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		// damage alebo hitpoint
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
		}

	}

	void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

}
