using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateManager : MonoBehaviour, SoundHearer
{
    //chaning guard states
    public GuardBaseState currentState;
    public GuardDefaultState defaultState = new GuardDefaultState();
    public GuardSeePlayerState seePlayerState = new GuardSeePlayerState();
    public GuardHearNoiseState hearNoiseState = new GuardHearNoiseState();
    public GuardChasePlayerState chasePlayerState = new GuardChasePlayerState();
    public GuardMaintainMapState maintainMapState = new GuardMaintainMapState();
    public GuardFeelsPlayerState feelsPlayerState = new GuardFeelsPlayerState();
    public GuardHuntPlayerState huntPlayerState = new GuardHuntPlayerState();


    //guard senses and going to waypoint
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

    //returning map to default state stuff
    public bool isMapOutOfDefault = false;
    public GameObject[] objsToReturnToNormal;
    public GameObject objToReturnToNormal;

    //stuff for if the player touches guard
    private bool isGuardTouchingPlayer = false;
    public bool isGuardBeingTouchedFromBehind = false;
    public GameObject playerNotNullWhenTouched = null;

    public AudioSource huhSound;

    public bool isGuardAtZeroAlertness;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        guardAnimator = GetComponent<Animator>();
        guardAnimator.SetBool("IsFindingPath", false);
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
        isGuardAtZeroAlertness = guardSenses.isGuardAtZeroAlertness();
        currentState.updateState(this);
        updateAnimations();

    }

    private void FixedUpdate()
    {
        guardSenses.fixedUpdateGuardAlertness(this);
    }
    private void updateAnimations()
    {

        if (agent.remainingDistance < 0.5)
        {
            guardAnimator.SetBool("IsFindingPath", false);
        }
        else
        {
            guardAnimator.SetBool("IsFindingPath", true);
        }

        /*isGuardMovingForAnimations = guardAnimator.GetBool("IsFindingPath");

        if (agent.remainingDistance < 0.5 && isGuardMovingForAnimations == true)
        {
            guardAnimator.SetBool("IsFindingPath", false );
        } else if ( isGuardMovingForAnimations == false)
        {
            Debug.Log("here: " + isGuardMovingForAnimations);
            guardAnimator.SetBool("IsFindingPath", true);
        }*/

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

    public bool getIfGuardIsTouchingPlayer()
    {
        return isGuardTouchingPlayer;
    }

    public bool getIfGuardIsBeingTouchedFromBehind()
    {
        return isGuardBeingTouchedFromBehind;
    }

    public bool getIfGuardShouldStopHuntingPlayer()
    {
        return isGuardAtZeroAlertness;
    }


    public void RespondToSound(Sound sound)
    {
        Debug.Log($"Guard heard sound at {sound.pos} with range {sound.range}");
        mostRecentSoundHeard = sound;
        moveFromDefaultToSoundState = true;
        //move the state switch into a state
        //SwitchState(hearNoiseState);
    }

    //check if the guard is colliding with the player
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            isGuardTouchingPlayer = true;
            //playerNotNullWhenTouched = guardSenses.player;
            print("Guard is touching player");
        } else
        {
            isGuardTouchingPlayer = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {

            if (isGuardBeingTouchedFromBehind == false)
            {
                isGuardBeingTouchedFromBehind = true;
                playerNotNullWhenTouched = guardSenses.player;
                print("Guard is being touched by player");
            }
        }
        else
        {
            isGuardBeingTouchedFromBehind = false;
        }
    }



}
