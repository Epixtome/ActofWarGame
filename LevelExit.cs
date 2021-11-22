using UnityEngine;

public class LevelExit : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //SceneManager.LoadScene(levelToLoad);
            Debug.Log("Touched");
            StartCoroutine(LevelManager.instance.LevelEnd());
        }
    }
}
