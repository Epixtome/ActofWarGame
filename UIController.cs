using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Slider healthSlider;
    public Text healthText, coinText;
    public GameObject deathScreen;
    public Image fadeScreen;
    public float fadeSpeed;
    public bool fadeToBlack, fadeOutBlack;
    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu;

    public Image currentWeapon;
    public Text weaponText;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        currentWeapon.sprite = PlayerController.instance.availableWeapons[PlayerController.instance.currentWeapon].weaponUI;
        weaponText.text = PlayerController.instance.availableWeapons[PlayerController.instance.currentWeapon].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }

}
