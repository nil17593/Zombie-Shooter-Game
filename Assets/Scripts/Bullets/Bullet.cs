using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    #region private properties
    private Vector2 m_initialiPos;
    private Rigidbody2D rb;
    #endregion

    void OnEnable()
    {
        // Get Rigidbody2D and set bullet's velocity when activated
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
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);           
        }
        transform.position = m_initialiPos;
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        
    }
}
