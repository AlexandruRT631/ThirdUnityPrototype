using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 20f;
    private PlayerController playerControllerScript;
    public float leftBound = -15f;
    private bool passed = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.startGame)
        {
            if (!playerControllerScript.gameOver)
            {
                if (playerControllerScript.dash)
                {
                    transform.Translate(Vector3.left *
                                        (Time.deltaTime * speed * playerControllerScript.dashMultiplier));
                }
                else
                {
                    transform.Translate(Vector3.left * (Time.deltaTime * speed));
                }
            }

            if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }

            if (!passed && transform.position.x < 0 && !playerControllerScript.gameOver &&
                gameObject.CompareTag("Obstacle"))
            {
                passed = true;
                playerControllerScript.IncreaseScore();
            }
        }
    }
}
