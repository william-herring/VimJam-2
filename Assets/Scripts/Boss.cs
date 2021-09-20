using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float attackRange;
    [SerializeField] private float speed;
    [SerializeField] private float idleTime;
    [SerializeField] private float cooldownTime;
    [SerializeField] private GameObject projectile;

    private float timer;
    private float cooldown;
    [SerializeField] private bool isIdle;

    public float health = 20f;

    private void Update()
    {
        healthBar.value = health;
        cooldown -= Time.deltaTime;

        if (health <= 0)
        {
            anim.Play("BossDie");
            healthBar.gameObject.SetActive(false);
            this.enabled = false;
        }

        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        if (target.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (isIdle)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                isIdle = false;
            }
            return;
        }
        
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, attackRange);

        int shootChance = Random.Range(0, 8);

        if (!rangeCheck.Contains(playerCollider))
        {
            return;
        }
        
        if (shootChance != 1 && cooldown <= 0)
        {
            Shoot(2);
            cooldown = cooldownTime;
        }
        else
        {
            isIdle = true;
            timer = idleTime;
        }
    }

    public void Shoot(int num)
    {
        anim.Play("BossShoot");

        for (var i = 0; i < num; i++)
        {
            GameObject shot = Instantiate(projectile, transform.position + Vector3.down * 2.3f, Quaternion.identity);
            Vector2 direction = default;

            if (target.position.x > transform.position.x)
            {
                direction = Vector2.right;
                shot.transform.localScale = new Vector3(1, 1, 1);
            }

            if (target.position.x < transform.position.x)
            {
                direction = Vector2.left;
                shot.transform.localScale = new Vector3(-1, 1, 1);
            }

            shot.GetComponent<Rigidbody2D>().AddForce(direction * 1000f);
        }
    }

    public void Smash()
    {
        anim.Play("BossAttack");
    }
}
