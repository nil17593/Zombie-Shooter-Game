using System.Collections;
using UnityEngine;
using SuperGaming.ZombieShooter.Event;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Zombie : MonoBehaviour, IDamagable
{
    [Header("Scriptable object for zombie")]
    public ZombieScriptableObject zombieScriptableObject;
    public HealthBar healthBar;
    public LayerMask whatIsPlayer;


    #region protected properties

    protected float moveSpeed;
    protected float attackCooldown;
    protected int attackDamage;
    protected int health;
    protected float attackRange;
    protected bool isAttacking;
    protected bool isDead;
    protected Animator anim;
    protected float attackCooldownTimer;
    protected Rigidbody2D m_rb2D;
    protected Vector2 moveDirection;
    #endregion

    #region Cached data
    protected int m_initialHealth;
    #endregion
    private float checkInterval = 0.1f;
    private float checkTimer = 0f;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Start()
    {
        Initialize(zombieScriptableObject);
    }

    // Initialize zombie stats from the ScriptableObject
    public void Initialize(ZombieScriptableObject zombieScriptableObject)
    {
        health = zombieScriptableObject.health;
        moveSpeed = zombieScriptableObject.moveSpeed;
        attackCooldown = zombieScriptableObject.attackCooldown;
        attackDamage = zombieScriptableObject.damage;
        attackCooldownTimer = attackCooldown;
        attackRange = zombieScriptableObject.attackRange;
        isAttacking = false;
        isDead = false;
        m_initialHealth = health;
        healthBar.SetHealth(health, m_initialHealth);
    }

    public virtual void Update()
    {
        if (isDead) return;
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0)
        {
            CheckForPlayerInRange();
            checkTimer = checkInterval;
        }
        HandleAttack();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        HandleMovement();
    }

    // Move the zombie toward the player
    protected void HandleMovement()
    {
        if (!isAttacking)
        {          
            m_rb2D.velocity = new Vector2(-moveSpeed, m_rb2D.velocity.y);
        }
    }

    // Check if the player is in attack range and start the attack
    protected void CheckForPlayerInRange()
    {
        // Using Physics2D.OverlapCircle to check if the player is within the attack range
        Collider2D playerInRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        if (playerInRange != null && !isAttacking)
        {
            m_rb2D.velocity = Vector2.zero; // Stop moving when attacking
            StartAttack();
        }
    }

    // Start attacking the player
    protected void StartAttack()
    {
        isAttacking = true; 
        PlayAttackAnimation();
        attackCooldownTimer = attackCooldown;  // Reset attack cooldown timer
    }

    // Handle attack cooldown and logic
    protected void HandleAttack()
    {
        if (isAttacking)
        {
            attackCooldownTimer -= Time.deltaTime;

            if (attackCooldownTimer <= 0)
            {
                Attack();
                attackCooldownTimer = attackCooldown;  // Reset for next attack
            }
        }
    }

    // Attack the player (can be customized in variants)
    public virtual void Attack()
    {
        //custom logic for attack seprately
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsPlayer);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            playerToDamage[i].GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    // Take damage and handle zombie death
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        anim.SetTrigger("hurt");
        health -= amount;
        healthBar.SetHealth(health, m_initialHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle zombie death
    public virtual void Die()
    {
        isDead = true;
        isAttacking = false;
        GameplayController.Instance.TotalZombiesKilled++;
        EventManager.TriggerZombieKilledEvent();
        StartCoroutine(DeactivateAfterDeath());
    }

    // Coroutine to deactivate zombie after death animation
    private IEnumerator DeactivateAfterDeath()
    {
        PlayDeathAnimation();
        yield return new WaitForSeconds(1f);
        transform.rotation = Quaternion.identity;
        ResetToUseFromPoolAgain();
        gameObject.SetActive(false);
    }


    protected virtual void PlayAttackAnimation()
    {
        anim.SetBool("Attack", true);
        anim.SetBool("Walk", false);
    }

    protected virtual void PlayDeathAnimation()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Walk", false);
        anim.SetTrigger("death_02");
    }

    //reset evrything so that we can use this agin from object pool
    protected virtual void ResetToUseFromPoolAgain()
    {
        health = m_initialHealth;
        anim.Rebind();
    }
}
