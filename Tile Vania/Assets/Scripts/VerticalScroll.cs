using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [Tooltip ("Game units per second")]
    [SerializeField] float scrollRate = 1f;
    [SerializeField] float maxHeight = 3f;
    [SerializeField] float fastDistance = 10f;
    [SerializeField] float fastMultiplier = 5f;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= maxHeight) return;
        float yMove = scrollRate * Time.deltaTime;
        if (player.transform.position.y - transform.position.y >= fastDistance)
        {
            yMove *= fastMultiplier;
        }
        transform.Translate(new Vector2(0f, yMove));
        Debug.Log(transform.position.y);
    }
}
