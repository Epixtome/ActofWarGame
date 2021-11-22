using UnityEngine;

public class GunChest : MonoBehaviour
{
    public GunPickup[] potentialGuns;
    public SpriteRenderer sr;
    public Sprite chestOpen;
    private bool canOpen, isOpen;
    public Transform spawnPoint;

    void Start()
    {

    }

    void Update()
    {
        if (canOpen && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int gunSelect = Random.Range(0, potentialGuns.Length);

                Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);

                sr.sprite = chestOpen;

                isOpen = true;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
        }
    }
}
