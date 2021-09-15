using System;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float arrowForce;
    [SerializeField] private float range;
    [SerializeField] private float arrowCooldown = 1f;
    private float cooldown;

    private void Update()
    {
        cooldown -= Time.deltaTime;
    }
    
    private void FixedUpdate()
    {
        if (IsPlayerInRange() && cooldown <= 0f)
        {
            Shoot();
            cooldown = arrowCooldown;
        }
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        if (direction == new Vector2(1, 0))
        {
            arrow.transform.localScale = new Vector3(1, 1, 1);
        }
        
        if (direction == new Vector2(-1, 0))
        {
            arrow.transform.localScale = new Vector3(-1, 1, 1);
        }

        arrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
    }

    private bool IsPlayerInRange()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2.3f, direction, range, mask);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }
}
