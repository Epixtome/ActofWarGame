using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public Weapon theWeapon;
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
            bool hasWeapon = false;
            foreach (Weapon weaponToCheck in PlayerController.instance.availableWeapons)
            {
                if (theWeapon.weaponName == weaponToCheck.weaponName)
                {
                    hasWeapon = true;
                }
            }

            if (!hasWeapon)
            {
                Weapon weaponClone = Instantiate(theWeapon);
                weaponClone.transform.parent = PlayerController.instance.weaponArm;
                weaponClone.transform.position = PlayerController.instance.weaponArm.position;
                weaponClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                weaponClone.transform.localScale = Vector3.one;

                PlayerController.instance.availableWeapons.Add(weaponClone);
                PlayerController.instance.currentWeapon = PlayerController.instance.availableWeapons.Count - 1;
                PlayerController.instance.SwitchWeapon();
            }



            AudioManager.instance.PlaySFX(7);
            Destroy(gameObject);
        }
    }
}
