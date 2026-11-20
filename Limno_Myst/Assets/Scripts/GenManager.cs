using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GenManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Transform WorldGrid;
    [SerializeField] private List<GameObject> RoomPrefabs;
    [SerializeField] private List<GameObject> LightPrefabs;
    [SerializeField] private int mapSize = 16; //MUST BE A SQUARE NUMBER OR IT BREAKS!!!
    [SerializeField] private GameObject EmptyRoomPrefab; // The prefab for an empty room, used to fill in gaps in the grid
    [SerializeField] private GameObject BarrierPrefab; // The prefab for a barrier, used to block off areas of the grid
    [SerializeField] private GameObject SpawnRoomPrefab, ExitRoomPrefab; // The prefabs for the spawn and exit rooms, used to place them in the grid
    [SerializeField] private GameObject PlayerObject;
    public List<GameObject> GeneratedRooms; // A list to keep track of all the rooms that have been generated in the grid

    [Header("Map Generation Settings")]
    public int mapEmptiness; //the chance of an empty room spawning
    public int mapBrightness; //the chance of a light spawning in a room
    private int mapSizeSquareRoot; // The square root of the map size, used for grid calculations
    private Vector3 currentPosition; // The current position in the grid where the next room will be placed
    public GenState currentState; // The current state of the world generation process
    private float currentPositionX, currentPositionZ, positionTracker, currentRoomIndex; // The current X and Z coordinates in the grid and a tracker for how many rooms have been placed in the current row
    [SerializeField] private float roomSize = 7f; // The size of each room, used to calculate the next position in the grid

    private void Start()
    {
        GenerateWorld();
        SpawnPlayer();
    }

    public void GenerateWorld()
    {
        mapSizeSquareRoot = (int)Mathf.Sqrt(mapSize);
        for (int i = 0; i < mapEmptiness; i++) //Spawns empty rooms based on the mapEmptiness variable
        {
            RoomPrefabs.Add(EmptyRoomPrefab); //Adds the empty room prefab to the list of room prefabs
        }
        for (int state = 0; state < System.Enum.GetNames(typeof(GenState)).Length; state++)
        {
            for (int i = 0; i < mapSize; i++)
            {
                if (positionTracker == mapSizeSquareRoot) //move the postion back to the start of the next row if the current row is full
                {
                    if (currentState == GenState.GeneratingBarrier) //put it to the right
                    {
                        GenerateBarrier();
                    }
                    positionTracker = 0;
                    currentPositionX = 0;
                    currentPositionZ += roomSize;
                    if (currentState == GenState.GeneratingBarrier) //put it to the left
                    {
                        GenerateBarrier();
                    }
                }
                currentPosition = new(currentPositionX, 0, currentPositionZ);
                switch (currentState)
                {
                    case GenState.GeneratingRoom:
                        GeneratedRooms.Add(Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Count)], currentPosition, Quaternion.identity, WorldGrid)); //Instantiates the room prefab at the current position and sets its parent to the WorldGrid
                        break;
                    case GenState.GeneratingLight:
                        int lightChance = Random.Range(-1, mapBrightness); //Generates a random number between 0 and 100 to determine if a light should be spawned
                        if (lightChance == 0) //If the random number is 0, spawn a light
                            GeneratedRooms.Add(Instantiate(LightPrefabs[Random.Range(0, LightPrefabs.Count)], currentPosition, Quaternion.identity, WorldGrid)); //Instantiates the light prefab at the current position and sets its parent to the WorldGrid
                        break;
                    case GenState.GeneratingBarrier:
                        if (currentRoomIndex <= mapSizeSquareRoot && currentRoomIndex >= 0 && currentRoomIndex >= mapSize - mapSizeSquareRoot) //for top and bottom
                        {
                            GenerateBarrier();
                        }
                        break;
                }
                currentRoomIndex++;
                positionTracker++; //keeps track of how many rooms have been placed in the current row
                currentPositionX += roomSize; //Adds more position to the X axis for the next room to be placed
            }
            NextState(); //Moves to the next state in the world generation process
            switch (currentState)
            {
                case GenState.GeneratingSpawnRoom:
                    int spawnRoomReplace = Random.Range(0, GeneratedRooms.Count); //Generates a random number between 0 and the number of generated rooms to determine which room to replace with the spawn room
                    spawnRoom = Instantiate(SpawnRoomPrefab, GeneratedRooms[spawnRoomReplace].transform.position, Quaternion.identity, WorldGrid); //Instantiates the spawn room prefab at the position of the room to be replaced
                    Destroy(GeneratedRooms[spawnRoomReplace]); //Removes the room that is being replaced
                    GeneratedRooms[spawnRoomReplace] = spawnRoom; //Replaces the old room with the new spawn room
                    break;
                case GenState.GeneratingExitRoom:
                    int exitRoomReplace = Random.Range(0, GeneratedRooms.Count); //Generates a random number between 0 and the number of generated rooms to determine which room to replace with the exit room
                    GameObject exitRoom = Instantiate(ExitRoomPrefab, GeneratedRooms[exitRoomReplace].transform.position, Quaternion.identity, WorldGrid); //Instantiates the exit room prefab at the position of the room to be replaced
                    Destroy(GeneratedRooms[exitRoomReplace]); //Removes the room that is being replaced
                    GeneratedRooms[exitRoomReplace] = exitRoom; //Replaces the old room with the new exit room
                    break;
            }
        }
    }

    public void GenerateBarrier()
    {
       currentPosition = new(currentPositionX, 0, currentPositionZ);
        Instantiate(BarrierPrefab, currentPosition, Quaternion.identity, WorldGrid); //Instantiates the barrier prefab at the current position and sets its parent to the WorldGrid
    }

    public GameObject spawnRoom;

    public void SpawnPlayer()
    {
        PlayerObject.SetActive(false); //Deactivates the player object
        PlayerObject.transform.position = new Vector3(spawnRoom.transform.position.x, spawnRoom.transform.position.y + 1, spawnRoom.transform.position.z); //Moves the player object to the position of the spawn room and raises it by 1 unit on the Y axis
        PlayerObject.transform.position = spawnRoom.transform.position; //Moves the player object to the position of the spawn room
        PlayerObject.SetActive(true); //Activates the player object
    }

    public void NextState()
    {
        currentState++;
        currentRoomIndex = 0;
        currentPositionX = 0;
        currentPositionZ = 0;
        positionTracker = 0;
        currentPosition = Vector3.zero;
    }
}
