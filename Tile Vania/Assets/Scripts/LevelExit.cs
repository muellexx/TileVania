using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] int timeToWait = 2;
    [SerializeField] GameObject exitVFX;

    [SerializeField] AudioClip winSound;
    [Range(0f, 1f)][SerializeField] float winVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject particles = Instantiate(
            exitVFX,
            transform.position + new Vector3(0, 1, 0),
            Quaternion.Euler(-90,0,0));
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        FindObjectOfType<Player>().StopMovement();
        if (winSound != null)
        {
            GameObject audioListener = GameObject.FindWithTag("AudioListener");
            AudioSource.PlayClipAtPoint(winSound, audioListener.transform.position, winVolume);
        }
        yield return new WaitForSeconds(timeToWait);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
