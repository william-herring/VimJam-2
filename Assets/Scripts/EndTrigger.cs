using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start()
    {
        anim.Play("FadeIn");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    public void LoadScene(int i)
    {
        StartCoroutine(LoadLevel(i));
    }
    
    IEnumerator LoadLevel(int buildIndex)
    {
        anim.SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("fadeout");
        
        SceneManager.LoadScene(buildIndex);
    }
}
