using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        if (!other.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
