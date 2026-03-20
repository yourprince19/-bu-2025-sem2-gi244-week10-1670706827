using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{

    public float jumpForce;

    public float gravityModifier;

    public ParticleSystem explosionParticle;

    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;

    public AudioClip crashSfx;

    private Rigidbody rb;

    private InputAction jumpAction;

    private int jumpCount = 2;

    private int currentJumpCount = 0;

    private bool isOnGround = true;

    public bool isDashing = false;

    private Animator playerAnim;

    private AudioSource playerAudio;

    public bool gameOver = false;

    void Awake()

    {

        rb = GetComponent<Rigidbody>();

        playerAnim = GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {

        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");

        gameOver = false;

    }

    // Update is called once per frame

    void Update()

    {

        if (jumpAction.triggered && isOnGround && !gameOver && currentJumpCount < jumpCount)

        {

            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);

            playerAnim.SetTrigger("Jump_trig");

            dirtParticle.Stop();

            playerAudio.PlayOneShot(jumpSfx);

            currentJumpCount++;

            if (currentJumpCount == jumpCount && !gameOver)

            {

                isOnGround = false;

                currentJumpCount = 0;

            }

        }

        if (Keyboard.current != null)

        {

            isDashing = Keyboard.current.shiftKey.isPressed;

        }

    }

    private void OnCollisionEnter(Collision collision)

    {

        if (collision.gameObject.CompareTag("Ground"))

        {

            isOnGround = true;

            dirtParticle.Play();

        }

        else if (collision.gameObject.CompareTag("Obstacle"))

        {

            Debug.Log("Game Over!");

            gameOver = true;

            playerAnim.SetBool("Death_b", true);

            playerAnim.SetInteger("DeathType_int", 1);

            explosionParticle.Play();

            dirtParticle.Stop();

            playerAudio.PlayOneShot(crashSfx);

        }

    }

}
