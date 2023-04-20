using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateManager : MonoBehaviour, SoundHearer
{

    public GuardBaseState currentState;
    public GuardDefaultState defaultState = new GuardDefaultState();
    public GuardSeePlayerState seePlayerState = new GuardSeePlayerState();
    public GuardHearNoiseState hearNoiseState = new GuardHearNoiseState();
    public GuardChasePlayerState chasePlayerState = new GuardChasePlayerState();

    public GuardSenses guardSenses;

    public NavMeshAgent agent;
    public Animator guardAnimator;
    public float rotationSpeed = 100.0f;

    public GameObject[] waypoints;
    public int currentWaypoint;

    //Vision stuff
    public float guardFov = 90.0f;
    public float guardVisionRange = 8.0f;
    public float maxAlertness = 100;

    private bool canGuardSeePlayer;
    private bool isGuardAtMaxAlertness;

    //Hearing stuff
    public Sound mostRecentSoundHeard;
    public bool moveFromDefaultToSoundState = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        guardAnimator = GetComponent<Animator>();
        currentWaypoint = 0;
        guardSenses = new GuardSenses(this);
        //maxAlertness = guardSenses.maxAlertness;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = defaultState;

        defaultState.enterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //updateAlertness
        //checkForSound
        guardSenses.updateGuardSenses();
        //currentAlertness = guardSenses.GetGuardAlertness();
        canGuardSeePlayer = guardSenses.canGuardSeePlayer();
        //print(canGuardSeePlayer);
        isGuardAtMaxAlertness = guardSenses.isGuardAtMaxAlertness();
        currentState.updateState(this);

    }

    private void FixedUpdate()
    {
        guardSenses.fixedUpdateGuardAlertness();
    }

    public void SwitchState(GuardBaseState state)
    {
        currentState = state;
        state.enterState(this);
    }

    public bool getIfGuardCanSeePlayer() 
    {
        return canGuardSeePlayer;
    }

    public bool getIfGuardShouldChasePlayer()
    {
        return isGuardAtMaxAlertness;
    }

    public bool getIfShouldReactToSound()
    {
        return moveFromDefaultToSoundState;
    }

    public void RespondToSound(Sound sound)
    {
        Debug.Log($"Guard heard sound at {sound.pos} with range {sound.range}");
        mostRecentSoundHeard = sound;
        moveFromDefaultToSoundState = true;
        //move the state switch into a state
        //SwitchState(hearNoiseState);
    }




    
}
