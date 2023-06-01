using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    bool gameHasEnded = false;
    [SerializeField] private float delayBeforeRestart = 0.5f;

    //this block of variables is used to apply visual effects depending on what the max guard alertness is.
    private float alertness;
    private float previousAlertness = 0;
    private Volume theVolume;
    private ChromaticAberration theChromaticAberration;
    private FilmGrain theFilmGrain;
    private Vignette theVignette;
    [SerializeField] private float guardAlertness = 100;
    private List<GuardStateManager> guardStateManagers = new List<GuardStateManager>();
    //this block of variables are the audio effects that play depending on the max guard alertness.
    public AudioSource stinger1;
    public AudioSource stinger2;
    public AudioSource suspenseLoop;
    private bool shouldStinger2Play = true;

    public int levelNumber; //used to track which levels the user has unlcoked
    private GameObject menu;

    //used to diaplay tooltips and button promptas as UI.
    public GameObject TooltipText;
    public GameObject DoorText;

    
    bool levelCompleted = false;

    public void EndGame()
    {
        
        if (gameHasEnded == false)
        {
            Debug.Log("Game over");
            gameHasEnded = true;
            //call the restart function after a small delay.
            Invoke("Restart", delayBeforeRestart);
        }
    }

    public void LeaveLevel() {
        Debug.Log("Exited level");
        levelCompleted = true;

        //set up the menu
        menu.SetActive(true);
        GameObject mainMenu = menu.transform.GetChild(1).gameObject;
        GameObject levelSelect = menu.transform.GetChild(2).gameObject;
        GameObject levelComplete = menu.transform.GetChild(3).gameObject;
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
        levelComplete.SetActive(false);
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(levelSelectButton);
        //GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().enabled = false;


        //Debug.Log(GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject);

        GameObject.Find("Music").GetComponent<AudioSource>().Stop();

        SceneManager.LoadScene(6);
    }

    //called when the player finishes a level successully. It makes the menu active, and shows the player a level complete screen.
    //It increases the number of levels unlocked if the player has not already completed the level.
    public void CompleteLevel()
    {
        Debug.Log("Completed level");
        levelCompleted = true;
        menu.SetActive(true);
        GameObject mainMenu = menu.transform.GetChild(1).gameObject;
        GameObject levelSelect = menu.transform.GetChild(2).gameObject;
        GameObject levelComplete = menu.transform.GetChild(3).gameObject;
        GameObject gameComplete = menu.transform.GetChild(5).gameObject;
        mainMenu.SetActive(false);
        levelSelect.SetActive(false);
        levelComplete.SetActive(true);
        
        
        GameObject levelCompleteText = levelComplete.transform.GetChild(1).gameObject;
        GameObject NextLevelUnlockedText = levelComplete.transform.GetChild(2).gameObject;
        GameObject levelSelectButton = levelComplete.transform.GetChild(0).gameObject;
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(levelSelectButton);
        //GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().enabled = false;

        //Destroy(GameObject.Find("EventSystem"));

        GameObject.Find("Music").GetComponent<AudioSource>().Stop();

        levelCompleteText.GetComponent<TextMeshProUGUI>().text = "Level " + levelNumber + " Complete!";
        if (levelNumber < 5)
        {
            NextLevelUnlockedText.GetComponent<TextMeshProUGUI>().text = "Level " + (levelNumber + 1) + " Unlocked!";
        }
        else
        {
            //NextLevelUnlockedText.GetComponent<TextMeshProUGUI>().text = "All Levels Unlocked!";
            levelComplete.SetActive(false);
            gameComplete.SetActive(true);


        }
        //levelSelect.transform.GetChild(1+levelNumber).GetComponent<Button>().interactable = true;

        if (mainMenu.GetComponent<MainMenu>().levelsUnlocked == levelNumber && levelNumber != 5)
        {
            mainMenu.GetComponent<MainMenu>().levelsUnlocked += 1;
            mainMenu.GetComponent<MainMenu>().Save();
            mainMenu.GetComponent<MainMenu>().levelSelectButtons();
        }

        SceneManager.LoadScene(6);

        //var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        //var eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        //eventSystem.SetSelectedGameObject(levelSelectButton.GetComponent<Button>().gameObject);

        //eventSystem.GetComponent<InputSystemUIInputModule>().deselectOnBackgroundClick = false;

        //eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(levelSelectButton.GetComponent<Button>().gameObject);

        //Debug.Log(GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject);



        //GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(levelSelectButton);
        //mainMenu.GetComponent<MainMenu>().changeActiveButton(levelSelectButton);
        //levelSelectButton.GetComponent<Button>().Select();
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(0);
    }
    void Restart()
    { //reload the active scene
        //menu.SetActive(true);
        if(!levelCompleted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Awake()
    {
        TooltipText = GameObject.Find("TooltipText");
        TooltipText.SetActive(false);
        DoorText = GameObject.Find("DoorText");
        DoorText.SetActive(false);

        if (GameObject.Find("MenuAccess") != null)
        {
            Debug.Log("Menu does exist");
            menu = GameObject.Find("MenuAccess").GetComponent<Menu>().menu;
            //menu = GameObject.Find("Menu");
            menu.SetActive(false);
        }
    }

    private void Start()
    {
        
        //upon starting a level, find all the guard's GSM's, and add them to a list. this list is used in update to update the visual and audio effects.
        GameObject[] guardObjs = GameObject.FindGameObjectsWithTag("Guard");
        foreach (GameObject guardObj in guardObjs)
        {
            if (guardObj.TryGetComponent(out GuardStateManager gsm))
            {
                guardStateManagers.Add(gsm);
            }
        }

        theVolume = GameObject.FindObjectOfType<Volume>();
        //theVolume = GetComponent<Volume>();
        theVolume.profile.TryGet(out theFilmGrain);
        theVolume.profile.TryGet(out theChromaticAberration);
        theVolume.profile.TryGet(out theVignette);

        suspenseLoop.Play();
    }

    void Update()
    {

        /*if(menu.activeSelf == true)
        {
            menu.SetActive(false);
        }*/

        if (!levelCompleted)
        {
            //get the max guard alertness, and apply visual effects depending on its intensity.
            float alertnessThisFrame = 0;
            foreach (GuardStateManager guard in guardStateManagers)
            {
                float thisGuardsAlertness = guard.guardSenses.GetGuardAlertness();
                if (thisGuardsAlertness > alertnessThisFrame)
                {
                    alertnessThisFrame = thisGuardsAlertness;
                }
            }
            alertness = alertnessThisFrame;

            //apply audio effects for if the player is seen, or the first time the guard reaches its max alertness (these audio effects only play again once the player is completely safe)
            if (alertness <= 0)
            {
                shouldStinger2Play = true;
            }

            if (alertness > 0 && previousAlertness <= 0)
            {
                stinger1.Play();
            }

            if (alertness >= 100 && previousAlertness < 100 && shouldStinger2Play)
            {
                stinger2.Play();
                shouldStinger2Play = false;
            }

            previousAlertness = alertness;


            //Debug.Log("Alertness:" + alertness);
            //Debug.Log("intsinsity:" + (alertness / guardAlertness));


            theChromaticAberration.intensity.value = alertness / guardAlertness;
            theFilmGrain.intensity.value = alertness / guardAlertness;
            theVignette.intensity.value = alertness / guardAlertness / 2;
            suspenseLoop.volume = alertness / guardAlertness;
        }
        else
        {
            suspenseLoop.volume = 0;
        }

        
    }




}