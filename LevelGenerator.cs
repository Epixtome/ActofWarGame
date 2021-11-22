using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelGenerator : MonoBehaviour
{

    public GameObject layoutRoom;
    public Color startColor, endColor, shopColor, gunRoomColor;

    public int distanceToEnd;
    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;
    public bool includeGunRoom;
    public int minDistanceToGunRoom, maxDistanceToGunRoom;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset = 32f, yOffset = 18f;

    public LayerMask whatIsRoom;

    private GameObject endRoom, shopRoom, gunRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutline = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop, centerGunRoom;
    public RoomCenter[] potentialCenters;

    void Start()
    {
        //place first layout
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        //place a layout until the end is reached
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();
            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }


        }

        //add shop
        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;

        }

        //add gunroom
        if (includeGunRoom)
        {
            int gunRoomSelect = Random.Range(minDistanceToGunRoom, maxDistanceToGunRoom + 1);
            gunRoom = layoutRoomObjects[gunRoomSelect];
            layoutRoomObjects.RemoveAt(gunRoomSelect);
            gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;

        }


        //create room outlines
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }
        if (includeGunRoom)
        {
            CreateRoomOutline(gunRoom.transform.position);
        }

        foreach (GameObject outline in generatedOutline)
        {
            bool generateCenter = true;
            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (includeGunRoom)
            {
                if (outline.transform.position == gunRoom.transform.position)
                {
                    Instantiate(centerGunRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }


            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Tab))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.Log("no room");
                break;
            case 1:
                if (roomAbove)
                {
                    generatedOutline.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutline.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutline.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutline.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedOutline.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft)
                {
                    generatedOutline.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutline.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    generatedOutline.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomBelow)
                {
                    generatedOutline.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove)
                {
                    generatedOutline.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if (roomAbove && roomBelow && roomRight)
                {
                    generatedOutline.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft && roomBelow)
                {
                    generatedOutline.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }
                if (roomAbove && roomBelow && roomLeft)
                {
                    generatedOutline.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutline.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }
                break;
            case 4:
                if (roomAbove && roomBelow && roomLeft && roomRight)
                {
                    generatedOutline.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }
                break;


        }


    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
