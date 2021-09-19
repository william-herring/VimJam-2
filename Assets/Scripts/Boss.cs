using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform target;
    [SerializeField] private List<string> cycleKeys;
    [SerializeField] private List<float> cycleValues;
    [SerializeField] private Rigidbody2D rb;
    
    private Dictionary<string, float> cycle = new Dictionary<string, float>();

    private int next = 0;

    private void Start()
    {
        for (var i = 0; i <= cycleKeys.Count; i++)
        {
            cycle.Add(cycleKeys[i], cycleValues[i]);
        }
    }

    IEnumerator Idle(float time)
    { 
        anim.Play("Idle");

        yield return new WaitForSeconds(time);
    }
}
