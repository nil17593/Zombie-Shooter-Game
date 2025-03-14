using UnityEngine;

namespace SuperGaming.ZombieShooter.Weapons
{
    /// <summary>
    /// this class is attached on each bullet
    /// bullets instantiating inside object pool and can be reuse
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [Header("Bullet settings")]
        [SerializeField] private float speed;
        [SerializeField] private int damage;

        #region private properties
        private Vector2 m_initialiPos;
        private Rigidbody2D rb;
        #endregion

        void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        public void SetupBullet(float newSpeed, int newDamage)
        {
            speed = newSpeed;
            damage = newDamage;
            m_initialiPos = transform.position;
            rb.velocity = Vector2.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Zombie.Zombie zombie = other.GetComponent<Zombie.Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
            transform.position = m_initialiPos;
            gameObject.SetActive(false);
        }
    }
}