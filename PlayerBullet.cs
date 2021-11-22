using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public float speed = 10f;

    public Rigidbody2D rb;

    public GameObject bulletImpact;
    public int damageToGive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * speed;


    }

    private void OnTriggerEnter2D(Collider2D other)

    {
        Instantiate(bulletImpact, transform.position, transform.rotation);
        AudioManager.instance.PlaySFX(4);
        Destroy(gameObject);

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
