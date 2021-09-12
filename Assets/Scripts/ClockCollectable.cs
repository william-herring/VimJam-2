using System;
using UnityEngine;

public class ClockCollectable : MonoBehaviour
{
    [SerializeField] private float value = 15;
    [SerializeField] private GameManager gm;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm.AddTime(value);
            Destroy(gameObject);
        }
    }
}
