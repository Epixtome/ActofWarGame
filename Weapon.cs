using UnityEngine;


public class Weapon : MonoBehaviour
{
    [Header("Guns")]
    public bool isGunType;
    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;

    [Header("Swords")]
    public bool isSwordType;
    public float timeBetweenSwing;
    private float swingCounter;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int daggerDamage;
    public float specialCounter;
    public float timeBetweenSpecial = 8f;

    [Header("MISC")]
    public string weaponName;
    public Sprite weaponUI;
    public float specialLength;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if (isGunType)
            {
                if (shotCounter > 0)
                {
                    shotCounter -= Time.deltaTime;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                    {
                        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                        AudioManager.instance.PlaySFX(12);
                        shotCounter = timeBetweenShots;
                    }
                }
            }

            if (isSwordType)
            {
                if (swingCounter > 0)
                {
                    swingCounter -= Time.deltaTime;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                    {
                        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

                        for (int i = 0; i < enemiesToDamage.Length; i++)
                        {
                            if (enemiesToDamage[i].tag == "Breakables")
                            {
                                enemiesToDamage[i].GetComponent<Breakables>().DamageEnemy(daggerDamage);
                            }
                            
                            if (enemiesToDamage[i].tag == "Enemy")
                            {
                                enemiesToDamage[i].GetComponent<EnemyController>().DamageEnemy(daggerDamage);
                            }
                        }
                        PlayerController.instance.anim.SetTrigger("meleeSwing");
                        //AudioManager.instance.PlaySFX(12);
                        swingCounter = timeBetweenShots;
                    }
                }

                if (specialCounter > 0)
                {
                    specialCounter -= Time.deltaTime;
                }
                else
                {
                    if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
                    {
                        AudioManager.instance.PlaySFX(8);
                        //PlayerController.instance.anim.SetTrigger("dash");
                        PlayerHealthController.instance.MakeInvincible(specialLength);
                        specialCounter = timeBetweenSpecial;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
