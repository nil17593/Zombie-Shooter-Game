using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Zombie : MonoBehaviour, IDamagable
{
    [Header("Scriptable object for zombie")]
    public ZombieScriptableObject zombieScriptableObject;

    #region protected properties

    protected float moveSpeed;
    protected float attackCooldown;
    protected int attackDamage;
    public int health;
    protected bool isAttacking;
    protected bool isDead;

    protected Transform playerTransform;
    protected Animator anim;
    protected float attackCooldownTimer;
    protected Rigidbody2D m_rb2D;
    protected Vector2 moveDirection;
    #endregion

    #region Cached data
    protected int m_initialHealth;
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Start()
    {
        Initialize(zombieScriptableObject);
        Debug.Log("HEALTH= " + health);
        //playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    // Initialize zombie stats from the ScriptableObject
    public void Initialize(ZombieScriptableObject zombieScriptableObject)
    {
        health = zombieScriptableObject.health;
        moveSpeed = zombieScriptableObject.moveSpeed;
        attackCooldown = zombieScriptableObject.attackCooldown;
        attackDamage = zombieScriptableObject.damage;
        attackCooldownTimer = attackCooldown;
        isAttacking = false;
        isDead = false;
        m_initialHealth = health;
    }

    public virtual void Update()
    {
        if (isDead) return;

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
            //transform.position = Vector2.MoveTowards(transform.position, dirX , moveSpeed * Time.deltaTime);
            //m_rb2D.velocity = new Vector2(dirX * moveSpeed * Time.fixedDeltaTime, m_rb2D.velocity.y);
            m_rb2D.velocity = new Vector2(-moveSpeed, m_rb2D.velocity.y);
        }
    }

    // Trigger attack when the player enters the zombie's attack range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController!=null && !isAttacking)
        {
            m_rb2D.velocity = Vector2.zero;
            playerController.TakeDamage(attackDamage);
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
    }

    // Take damage and handle zombie death
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        anim.SetTrigger("hurt");
        health -= amount;
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
