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

        // Start attacking the player
        protected void StartAttack()
        {
            isAttacking = true;
            PlayAttackAnimation();
            attackCooldownTimer = 0;  // Reset attack cooldown timer
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
            PlayerController.TakeDamage(attackDamage);
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
            EventManager.TriggerZombieKilledEvent(this);
            StartCoroutine(DeactivateAfterDeath());
        }

        // Coroutine to deactivate zombie after death animation
        private IEnumerator DeactivateAfterDeath()
        {
            PlayDeathAnimation();
            yield return new WaitForSeconds(1f);
            GameplayController.Instance.CheckForGameWin();
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
            m_rb2D.velocity = Vector2.zero;
            anim.Rebind();
            isAttacking = false;
            isDead = false;
        }

        // Detect player entering attack range using trigger
        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && !isAttacking)
            {
                PlayerController = playerController;
                StartAttack();
            }
        }
    }
}