using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    public float waitToBeCollected = .5f;


    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            AudioManager.instance.PlaySFX(7);
            Destroy(gameObject);
        }
    }
}
