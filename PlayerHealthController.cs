using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currentHealth, maxHealth;

    public float damageInvincLength = 1f;
    private float invincCount;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;
        //currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime;

            if (invincCount <= 0)
            {
                PlayerController.instance.body.color = new Color(PlayerController.instance.body.color.r, PlayerController.instance.body.color.g, PlayerController.instance.body.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {

        if (invincCount <= 0)
        {

            currentHealth--;
            AudioManager.instance.PlaySFX(11);

            invincCount = damageInvincLength;

            PlayerController.instance.body.color = new Color(PlayerController.instance.body.color.r, PlayerController.instance.body.color.g, PlayerController.instance.body.color.b, .5f);

            if (currentHealth <= 0)
            {
                AudioManager.instance.PlaySFX(9);
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.PlayGameOver();
            }


            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void MakeInvincible(float length)
    {
        invincCount = length;
        PlayerController.instance.body.color = new Color(PlayerController.instance.body.color.r, PlayerController.instance.body.color.g, PlayerController.instance.body.color.b, .5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
