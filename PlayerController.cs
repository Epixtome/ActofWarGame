using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D rb;

    public Transform weaponArm;


    public Animator anim;



    public static PlayerController instance;

    public SpriteRenderer body;
    [HideInInspector]
    public float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvincibility = .5f;

    private float dashCoolCounter;
    [HideInInspector]
    public float dashCounter;

    [HideInInspector]
    public bool canMove = true;

    public List<Weapon> availableWeapons = new List<Weapon>();
    [HideInInspector]
    public int currentWeapon;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        activeMoveSpeed = moveSpeed;
        UIController.instance.currentWeapon.sprite = availableWeapons[currentWeapon].weaponUI;
        UIController.instance.weaponText.text = availableWeapons[currentWeapon].weaponName;
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove && !LevelManager.instance.isPaused)
        {


            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();
            //transform.position += new Vector3(moveInput.x * Time.deltaTime, moveInput.y * Time.deltaTime, 0f) * moveSpeed;

            rb.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                weaponArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                weaponArm.localScale = Vector3.one;
            }

            /*rotate gun arm. tutorial said this rotating should happen on 
            the player. this means that every weapon will rotate the way 
            designed.*/

            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            weaponArm.rotation = Quaternion.Euler(0, 0, angle);

            //Gun switching section

            if (Input.mouseScrollDelta.y < 0)
            {
                if (availableWeapons.Count > 0)
                {
                    currentWeapon++;
                    if (currentWeapon >= availableWeapons.Count)
                    {
                        currentWeapon = 0;
                    }
                    SwitchWeapon();
                }
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                if (availableWeapons.Count > 0)
                {
                    currentWeapon--;
                    if (currentWeapon < 0)
                    {
                        currentWeapon = availableWeapons.Count - 1;
                    }
                    SwitchWeapon();
                }
                else
                {
                    Debug.Log("no guns");
                }
            }

            #region Dashing
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {

                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                    AudioManager.instance.PlaySFX(8);
                    anim.SetTrigger("dash");
                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;

                    dashCoolCounter = dashCooldown;
                }
            }
            #endregion

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }


            #region Animation
            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
            #endregion


        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }

    public void SwitchWeapon()
    {
        foreach (Weapon theWeapon in availableWeapons)
        {
            theWeapon.gameObject.SetActive(false);
        }
        availableWeapons[currentWeapon].gameObject.SetActive(true);
        UIController.instance.currentWeapon.sprite = availableWeapons[currentWeapon].weaponUI;
        UIController.instance.weaponText.text = availableWeapons[currentWeapon].weaponName;
    }
}
