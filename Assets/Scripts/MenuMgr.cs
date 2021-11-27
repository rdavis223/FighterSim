using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMgr : MonoBehaviour
{
    public bool pauseAllowed = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (pauseAllowed && Input.GetKeyDown(KeyCode.Escape)){
            togglePause();
        }
    }


    public void togglePause()
    {
        GlobalStateMgr.togglePause();
        pauseMenu.SetActive(GlobalStateMgr.isPaused());
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
