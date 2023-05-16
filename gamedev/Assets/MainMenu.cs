using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public void LevelSelect(int level) {
        //GameObject.Find("Menu").SetActive(false);
        SceneManager.LoadScene(level);
    }

    public void changeActiveButton(GameObject button) {
        eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(button);
        Debug.Log("Selected button changed to : " + button);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
