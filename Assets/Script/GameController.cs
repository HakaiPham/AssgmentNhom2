using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _PanelMenu;
    public TimeLineStory _TimeLine;
    bool checkTimeLine;
    public GameObject _ButtonMenu;
    private void Start()
    {
        _PanelMenu.SetActive(false);
    }
    private void Update()
    {
         checkTimeLine = _TimeLine.CheckTimeLineStart();
        if (checkTimeLine)
        {
            _ButtonMenu.SetActive(false);
        }
        else
        {
            _ButtonMenu.SetActive(true);
        }
    }
    public void MenuButton()
    {
        if (checkTimeLine) return;
        else
        {
            _PanelMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
   public void HomeButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void HomeMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ResumeButton()
    {
        Time.timeScale = 1f;
        _PanelMenu.SetActive(false);
    }
}
