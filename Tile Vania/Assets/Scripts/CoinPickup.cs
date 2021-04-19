using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinValue = 1;

    [SerializeField] AudioClip coinSound;
    [Range(0f, 1f)][SerializeField] float coinVolume = 1f;

    // GameSession gameSession;

    bool wasCollected = false;

    private void Start()
    {
        // gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickupCoin();
    }

    private void PickupCoin()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        if(wasCollected) return;
        if (coinSound != null)
        {
            GameObject audioListener = GameObject.FindWithTag("AudioListener");
            AudioSource.PlayClipAtPoint(coinSound, audioListener.transform.position, coinVolume);
        }
        gameSession.AddCoins(coinValue);
        wasCollected = true;
        Destroy(gameObject);
    }
}
