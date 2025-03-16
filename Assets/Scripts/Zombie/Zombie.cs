using System.Collections;
using UnityEngine;
using SuperGaming.ZombieShooter.Events;
using SuperGaming.ZombieShooter.Player;
using SuperGaming.ZombieShooter.Controllers;
using SuperGaming.ZombieShooter.UI;

namespace SuperGaming.ZombieShooter.Zombie
{
    /// <summary>
    /// this is the base zombie class
    /// takes scriptbale object data to create zombie
    /// </summary>
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
        protected Transform playerTransform; // Reference to player's position
        #endregion

        #region Cached data
        protected int m_initialHealth;
        PlayerController PlayerController;
        #endregion

        private void Awake()
        {
            anim = GetComponent<Animator>();
            m_rb2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            Initialize(zombieScriptableObject);
            PlayerController = FindObjectOfType<PlayerController>();
            if (PlayerController != null)
            {
                playerTransform = PlayerController.transform;  // Cache player's transform for movement and attack
            }
        }

        public void Initialize(ZombieScriptableObject zombieScriptableObject)
        {
            health = zombieScriptableObject.health;
            moveSpeed = zombieScriptableObject.moveSpeed;
            attackCooldown = zombieScriptableObject.attackCooldown;
            attackDamage = zombieScriptableObject.damage;
            attackRange = zombieScriptableObject.attackRange;
            isAttacking = false;
            isDead = false;
            attackCooldownTimer = attackCooldown;
            m_initialHealth = health;
            healthBar.SetHealth(health, m_initialHealth);
        }

        protected virtual void Update()
        {
            if (isDead) return;
            HandleAttack();
        }

        protected virtual void FixedUpdate()
        {
            if (isDead) return;
            HandleMovement();
        }

        protected virtual void HandleMovement()
        {
            if (!isAttacking && playerTransform != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

                // Check if zombie is within attack range
                if (distanceToPlayer > attackRange)
                {
                    // Move towards player
                    Vector2 direction = (playerTransform.position - transform.position).normalized;
                    m_rb2D.velocity = new Vector2(direction.x * moveSpeed, m_rb2D.velocity.y);
                    anim.SetBool("Walk", true);
                }
                else
                {
                    // Stop moving if within attack range
                    anim.SetBool("Walk", false);
                    m_rb2D.velocity = Vector2.zero;
                    InitiateAttack();
                }
            }
        }

        // Initiates the attack sequence
        protected virtual void InitiateAttack()
        {
            isAttacking = true;
            PlayAttackAnimation();
        }

        protected virtual void HandleAttack()
        {
            if (isAttacking)
            {
                attackCooldownTimer -= Time.deltaTime;

                if (attackCooldownTimer <= 0)
                {
                    attackCooldownTimer = attackCooldown;
                    ExecuteAttack();
                }

                // If player moves out of range, stop attacking and start moving again
                if (playerTransform != null)
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                    if (distanceToPlayer > attackRange)
                    {
                        StopAttack();
                    }
                }
            }
        }

        protected virtual void ExecuteAttack()
        {
            // Trigger the attack animation on every attack cycle
            PlayAttackAnimation();
        }

        public void DealDamage()
        {
            if (PlayerController != null && !isDead)
            {
                PlayerController.TakeDamage(attackDamage);
            }
        }

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

        protected virtual void Die()
        {
            isDead = true;
            isAttacking = false;
            EventManager.TriggerZombieKilledEvent(this);
            StartCoroutine(DeactivateAfterDeath());
        }

        private IEnumerator DeactivateAfterDeath()
        {
            PlayDeathAnimation();
            yield return new WaitForSeconds(1f);
            GameplayController.Instance.CheckForGameWin();
            transform.rotation = Quaternion.identity;
            ResetToUseFromPoolAgain();
            gameObject.SetActive(false);
        }

        protected virtual void ResetToUseFromPoolAgain()
        {
            health = m_initialHealth;
            m_rb2D.velocity = Vector2.zero;
            anim.Rebind();
            isAttacking = false;
            isDead = false;
        }

        #region Animations
        protected virtual void StopAttack()
        {
            isAttacking = false;
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
        }

        protected virtual void PlayAttackAnimation()
        {
            // Trigger the attack animation every time ExecuteAttack is called
            anim.SetTrigger("attack");
        }

        protected virtual void PlayDeathAnimation()
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", false);
            anim.SetTrigger("death_02");
        }
        #endregion
    }
}
