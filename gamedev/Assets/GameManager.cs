using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    bool gameHasEnded = false;
    [SerializeField] private float delayBeforeRestart = 0.5f;
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

    


}