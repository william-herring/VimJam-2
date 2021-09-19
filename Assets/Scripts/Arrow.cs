using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
