using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    public bool pausePanelActive = false;

    public Text speedText;
    public Text attackSpeedText;
    public Text damageText;


    private void Start()
    {
        pausePanel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!pausePanelActive)
            {
                StartCoroutine(Wait(true));
                Time.timeScale = 0;
                Debug.Log("Paused");
                pausePanel.SetActive(true);
            }
            if (pausePanelActive)
            {
                StartCoroutine(Wait(false));
                Time.timeScale = 1;
                Debug.Log("Play");
                pausePanel.SetActive(false);
            }
        }

        speedText.text = "" + Mathf.Round(GameController.Speed * 100) / 100f;
        damageText.text = "" + Mathf.Round(GameController.Damage * 100) / 100f;

        if (GameController.FireDelay > 0.5f)
        {
            attackSpeedText.text = "Slow";
        }
        else if (GameController.FireDelay < 0.5f)
        {
            attackSpeedText.text = "Fast";
        }
        else
        {
            attackSpeedText.text = "Medium";
        }

    }

    public void Resume()
    {
        pausePanelActive = false;
        Time.timeScale = 1;
        Debug.Log("Play");
        pausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    IEnumerator Wait(bool value)
    {
        yield return new WaitForSecondsRealtime(.2f);
        pausePanelActive = value;
    }
}
