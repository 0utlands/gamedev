using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public int levelsUnlocked;

    void Start()
    {
        levelSelectButtons();
    }
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

    public void levelSelectButtons() {
        for(int i = 0; i < 5; i++)
        {
            Debug.Log("i = " + i + "unlockedlevels = " + levelsUnlocked);
            if (i < levelsUnlocked)
            {
                GameObject.Find("Menu").gameObject.transform.GetChild(2).gameObject.transform.GetChild(i + 1).GetComponent<Button>().interactable = true;
            }
            else {
                //Debug.Log("TESTING2");
                GameObject.Find("Menu").gameObject.transform.GetChild(2).gameObject.transform.GetChild(i + 1).GetComponent<Button>().interactable = false;
            }

        }
        
    }
}
