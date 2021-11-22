using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float moveSpeed;
    public Camera mainCamera;

    public Transform target;
    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
    }
    void Start()
    {

    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

}
