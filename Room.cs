using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;
    public GameObject[] doors;
    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }

            }
            roomActive = true;

            mapHider.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            roomActive = false;
        }
    }
}

