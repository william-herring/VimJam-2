using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject startGameScene;

    public void StartGame()
    {
        StartCoroutine(LoadLevel(1));
    }
    
    IEnumerator LoadLevel(int buildIndex) 
    { 
        startGameScene.SetActive(true);
        
        yield return new WaitForSeconds(18f);

        SceneManager.LoadScene(buildIndex);
    }
}
