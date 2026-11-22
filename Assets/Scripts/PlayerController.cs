using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction TalkAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    public int maxHealth = 5;
    public int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    public UIHandler uiHandler;

    public NonPlayerCharacter lastNonPlayerCharacter;

    void Start()
    {
        MoveAction.Enable();
        TalkAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            if(lastNonPlayerCharacter == null)
            {
                lastNonPlayerCharacter = hit.collider.GetComponent<NonPlayerCharacter>();
                lastNonPlayerCharacter.dialogueBuddle.SetActive(true);
            }

            if (TalkAction.WasPressedThisFrame())
            {
                uiHandler.DisplayDialogue(lastNonPlayerCharacter.message);
            }
        }
        else if(lastNonPlayerCharacter != null)
        {
            lastNonPlayerCharacter.dialogueBuddle.SetActive(false);
            lastNonPlayerCharacter = null;
        }
    }

    void OnDrawGizmos()
    {
        Vector2 start = (Vector2)transform.position + Vector2.up * 0.2f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, start + moveDirection * 1.5f);
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * 3.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
   	        if (isInvincible)
   	        {
     		    return;
   	        }
   	        isInvincible = true;
   	        damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }   

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        uiHandler.SetHealthValue(currentHealth / (float) maxHealth);
        Debug.Log("Health: " + currentHealth + "/" + maxHealth);
    }
}
