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
    private Vignette theVignette;
    [SerializeField] private float guardAlertness = 100;
    private List<GuardStateManager> guardStateManagers = new List<GuardStateManager>();

    public AudioSource stinger1;
    public AudioSource stinger2;
    public AudioSource suspenseLoop;

    private bool shouldStinger2Play = true;

    private float previousAlertness = 0;

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

    private void Start()
    {
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


        if(alertness <= 0)
        {
            shouldStinger2Play = true;
        }

        if(alertness > 0 && previousAlertness <=0) 
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




}