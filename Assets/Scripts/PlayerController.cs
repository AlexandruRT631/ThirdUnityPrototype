using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    public float jumpForce = 10;
    public float gravityModifier = 1;
    public bool isOnGround = true;
    public bool hasSecondJump = true;
    public bool gameOver = false;
    public bool dash = false;
    private int scoreMultiplier = 1;
    public int dashMultiplier = 2;
    private int score = 0;
    public bool startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-5, 0, 0);
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            if (Input.GetKeyDown(KeyCode.Space) && hasSecondJump && !gameOver)
            {
                dirtParticle.Stop();
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (isOnGround)
                {
                    isOnGround = false;
                    playerAnim.SetTrigger("Jump_trig");
                }
                else
                {
                    hasSecondJump = false;
                    playerAnim.Play("Running_Jump", -1, 0.0f);

                }

                playerAudio.PlayOneShot(jumpSound);
            }

            if (Input.GetKey(KeyCode.LeftShift) && isOnGround && !gameOver)
            {
                dash = true;
                scoreMultiplier = dashMultiplier;
                playerAnim.speed = dashMultiplier;
            }
            else if (isOnGround)
            {
                dash = false;
                scoreMultiplier = 1;
                playerAnim.speed = 1;
            }
        }
        else
        {
            dirtParticle.Stop();
            playerAnim.speed = 0.5f;
            transform.Translate(Vector3.forward * (Time.deltaTime * 3));
            if (transform.position.x > 0)
            {
                startGame = true;
                dirtParticle.Play();
                playerAnim.speed = 1;
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            hasSecondJump = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle") && !gameOver)
        {
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetInteger("DeathType_int", 1);
            playerAnim.SetBool("Death_b", true);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound);
        }
    }

    public void IncreaseScore()
    {
        score++;
        Debug.Log("Score: " + score);
    }
}
