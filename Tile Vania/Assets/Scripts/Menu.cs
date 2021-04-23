using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // bool audioResumed = false;

    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // public void ResumeAudio()
    // {
    //     if (!audioResumed)
    //     {
    //         var result = FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
    //         Debug.Log(result);
    //         result = FMODUnity.RuntimeManager.CoreSystem.mixerResume();
    //         Debug.Log(result);
    //         audioResumed = true;
    //     }
    // }
}
