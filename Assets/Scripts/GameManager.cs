using System;
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
    private bool hintsEnabled = false;
    private float elapsed;

    private void Start()
    {
        if (_timer == 0f)
        {
            _timer = startingTime;
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        _timer -= Time.deltaTime;
        timeText.text = FormatAsTimeInMinutes(_timer);

        if (elapsed > 1f)
        {
            clock.GetComponent<RectTransform>().Translate(Vector2.left * 0.37f);
            elapsed = elapsed % 1f;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHints();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
