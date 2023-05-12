using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    bool gameHasEnded = false;
    [SerializeField] private float delayBeforeRestart = 0.5f;

    private float alertness;
    private Volume theVolume;
    private ChromaticAberration theChromaticAberration;
    private FilmGrain theFilmGrain;
    [SerializeField] private float guardAlertness = 100;
    private List<GuardStateManager> guardStateManagers = new List<GuardStateManager>();

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

    public void CompleteLevel()
    {
        Debug.Log("Level complete!");
    }
    void Restart()
    { //reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        GameObject[] guardObjs = GameObject.FindGameObjectsWithTag("Guard");
        foreach (GameObject guardObj in guardObjs)
        {
            if (guardObj.TryGetComponent(out GuardStateManager gsm))
            {
                guardStateManagers.Add(gsm);
            }
        }

        theVolume = GameObject.FindObjectOfType<GameManager>().GetComponent<Volume>();
        theVolume.profile.TryGet(out theFilmGrain);
        theVolume.profile.TryGet(out theChromaticAberration);
    }

    void Update()
    {
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

        Debug.Log("Alertness:" + alertness);
        Debug.Log("intsinsity:" + (alertness / guardAlertness));


        theChromaticAberration.intensity.value = alertness / guardAlertness;
        theFilmGrain.intensity.value = alertness / guardAlertness;
    }




}