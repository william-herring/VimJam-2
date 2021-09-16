using System;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private AIPath path;
    [SerializeField] private float searchRadius;
    [SerializeField] private Collider2D playerCollider;
    public int health = 2; 

    private void Update()
    {
        if (destinationSetter.target.position.x > transform.position.x)
        {
            spriteTransform.localScale = new Vector3(1, 1, 1);
        }
        
        if (destinationSetter.target.position.x < transform.position.x)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRange())
        {
            path.enabled = true;
        }
        else
        {
            path.enabled = false;
        }
    }

    public void DoDamage(int amount)
    {
        health -= amount;
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool IsPlayerInRange()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, searchRadius);

        if (hit.Contains(playerCollider))
        {
            return true;
        }

        return false;
    }
}
