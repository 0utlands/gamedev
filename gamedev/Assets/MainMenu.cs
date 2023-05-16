using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MainMenu;

public class MainMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public int levelsUnlocked;

    void Start()
    {
        Load();
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

    public class SaveObj
    {
        public int levelUnlocked;
    }

    public void Save()
    {
        SaveObj saveObj = new SaveObj
        {
            levelUnlocked = levelsUnlocked
        };
        string saveJson = JsonUtility.ToJson(saveObj);
        File.WriteAllText(Application.dataPath + "/save.txt", saveJson);
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObj saveObj = JsonUtility.FromJson<SaveObj>(saveString);
            Debug.Log("File exists");
            levelsUnlocked = saveObj.levelUnlocked;
            //Debug.Log("Levels unlocked: " + levelsUnlocked);
        }
        else
        {
            levelsUnlocked = 1;
        }
    }
    public void ResetSave()
    {
        levelsUnlocked = 1;
        Save();
        levelSelectButtons();
    }


}
