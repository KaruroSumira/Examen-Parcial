using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    public static Movimiento Instance;
    public float runSpeed = 2f;
    public float jumpSpeed = 3f;
    private float originalRunSpeed;
    private float originalJumpSpeed;

    [SerializeField]
    private int lives = 5;

    private Rigidbody2D rb2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Text vidasText;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        originalRunSpeed = runSpeed;
        originalJumpSpeed = jumpSpeed;
        Instance = this;

        if (vidasText == null)
        {
            Debug.LogError("Falta asignar el objeto de texto para mostrar las vidas en el Inspector.");
        }

        UpdateVidasText();
    }

    void FixedUpdate()
    {
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool("run", true);
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool("run", true);
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            animator.SetBool("run", false);
        }
        if (Input.GetKey("space") && Checkground.IsGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
        }
    }

    public void ApplyPowerUp(PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case PowerUpType.SpeedBoost:
                StartCoroutine(SpeedBoost());
                break;
            case PowerUpType.JumpBoost:
                StartCoroutine(JumpBoost());
                break;
        }
    }

    IEnumerator SpeedBoost()
    {
        runSpeed *= 1.2f;
        yield return new WaitForSeconds(5);
        runSpeed = originalRunSpeed;
    }

    IEnumerator JumpBoost()
    {
        jumpSpeed *= 1.2f;
        yield return new WaitForSeconds(8);
        jumpSpeed = originalJumpSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                ApplyPowerUp(powerUp.powerUpType);
                Destroy(collision.gameObject);
            }
        }
    }

    public void DecreaseLives(int amount)
    {
        lives -= amount;
        if (lives <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            UpdateVidasText();
            Debug.Log("El jugador ha perdido una vida. Vidas restantes: " + lives);
        }
    }


    void UpdateVidasText()
    {
        Debug.Log("Actualizando el texto de las vidas. Vidas actuales: " + lives);
        vidasText.text = "Vidas: " + lives.ToString();
    }
    private void OnDestroy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
