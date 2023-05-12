using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void LevelSelect(int level) {
        //GameObject.Find("Menu").SetActive(false);
        SceneManager.LoadScene(level);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
