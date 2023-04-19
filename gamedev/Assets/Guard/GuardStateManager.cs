using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateManager : MonoBehaviour
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

    public float currentAlertness = 0.0f;
    public float maxAlertness;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        guardAnimator = GetComponent<Animator>();
        currentWaypoint = 0;
        guardSenses = new GuardSenses(this);
        maxAlertness = guardSenses.maxAlertness;
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
        currentAlertness = guardSenses.GetGuardAlertness();
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

    
}
