using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    [Header("Settings")]
    public float JumpForce;
    [Header("References")]

    public Rigidbody2D PlayerRigidBody;
    public Animator playerAnimator;
    public BoxCollider2D PlayerCollider;

    private bool isGrounded = true;
 
    public bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            playerAnimator.SetInteger("State", 1);
        }
    }

    public void KillPlayer()
    {
        PlayerCollider.enabled = false;
        playerAnimator.enabled = false;
        PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
    }

    void Hit()
    {
        if (GameManager.Instance == null) { Debug.LogWarning("GameManager.Instance == null (Hit)"); return; }

        int before = GameManager.Instance.Lives;
        GameManager.Instance.Lives = Mathf.Max(0, before - 1);
        Debug.Log($"[Hit] Lives {before} -> {GameManager.Instance.Lives}");
    }

    void Heal()
    {
        GameManager.Instance.Lives = Mathf.Min(3, GameManager.Instance.Lives + 1);
    }

    void StartInvincible()
    {
        isInvincible = true;
        Invoke("StopInvincible", 5f);
    }

    void StopInvincible()
    {
        isInvincible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Platform")
        {
            if (!isGrounded)
            { playerAnimator.SetInteger("State", 2); 
            }
            isGrounded=true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "enemy"){
            if (!isInvincible) {
                Destroy(collider.gameObject);
                Hit(); 
            }
         
    }
        else if(collider.gameObject.tag == "food"){ 
            Destroy(collider.gameObject);
            Heal();
    }
        else if(collider.gameObject.tag == "golden") {
            Destroy(collider.gameObject);
            StartInvincible();
        }
        
}
   }