using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static float _timer;
    [SerializeField] private float startingTime;
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject hintCanvas;
    [SerializeField] private GameObject collapseScreen;
    private GameObject player;
    private static float levelEnterTime;
    private static bool restart;
    private bool hintsEnabled = false;
    private float elapsed;

    private void Start()
    {
        player = GameObject.Find("Player");
        
        if (restart)
        {
            restart = false;
            _timer = levelEnterTime;
        }
        
        if (_timer == 0f)
        {
            _timer = startingTime;
            levelEnterTime = _timer;
        }
        
        levelEnterTime = _timer;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        _timer -= Time.deltaTime;
        timeText.text = FormatAsTimeInMinutes(_timer);

        if (elapsed > 1f)
        {
            clock.GetComponent<RectTransform>().Translate(Vector2.left * 1.1f);
            elapsed = elapsed % 1f;
        }

        if (_timer <= 0f)
        {
            player.GetComponent<PlayerController>().Pause();
            collapseScreen.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHints();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        restart = true;
        GameObject.Find("EndTrigger").GetComponent<EndTrigger>().LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToTitleScreen()
    {
        Destroy(GameObject.Find("Music"));
        SceneManager.LoadScene(0);
    }

    public void ToggleHints()
    {
        if (!hintsEnabled)
        {
            hintCanvas.SetActive(true);
            hintsEnabled = true;
        } else if (hintsEnabled)
        {
            hintCanvas.SetActive(false);
            hintsEnabled = false;
        }
    }

    private string FormatAsTimeInMinutes(float time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");

        return $"{minutes}:{seconds}";
    }

    public void AddTime(float time)
    {
        _timer += time;
    }
}
