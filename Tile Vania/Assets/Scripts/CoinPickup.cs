using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
    [Range(0f, 1f)][SerializeField] float coinVolume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickupCoin();
    }

    private void PickupCoin()
    {
        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(
                coinSound,
                Camera.main.transform.position,
                coinVolume);
        }
        Destroy(gameObject);
    }
}
