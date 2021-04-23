using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int coins = 0;
    [SerializeField] int coinsPerLive = 2;

    [SerializeField] Text livesText;
    [SerializeField] Text coinText;

    [SerializeField] AudioClip liveUpSound;
    [Range(0f, 1f)][SerializeField] float liveUpVolume = 1f;

    GameObject audioListener;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioListener = GameObject.FindWithTag("AudioListener");
        UpdateLivesText();
        UpdateCoinsText();
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
        if (coins >= coinsPerLive)
        {
            playerLives++;
            UpdateLivesText();
            coins -= coinsPerLive;
            if (liveUpSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    liveUpSound,
                    audioListener.transform.position,
                    liveUpVolume);
            }
        }
        UpdateCoinsText();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        playerLives--;
        UpdateLivesText();
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void UpdateLivesText()
    {
        if (!livesText) return;
        livesText.text = playerLives.ToString();
    }

    private void UpdateCoinsText()
    {
        if (!coinText) return;
        coinText.text = coins.ToString();
    }
}
