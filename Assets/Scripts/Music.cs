using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameObject[] other;
    private bool notFirst;
    private void Awake()
    {
        other = GameObject.FindGameObjectsWithTag("Music");
 
        foreach (GameObject oneOther in other)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                notFirst = true;
            }
        }
 
        if (notFirst)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }
}
