using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession)
        {
            Destroy(gameSession.gameObject);
        }
    }
}
