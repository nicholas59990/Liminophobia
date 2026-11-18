using UnityEngine;
using UnityEngine.SceneManagement;

public class GenManager : MonoBehaviour
{
    [SerializeField] private Transform WorldGrid;
    [SerializeField] private GameObject[] RoomPrefab;

    public int mapSize = 16; //MUST BE A SQUARE NUMBER
    private int mapSizeSquareRoot; // The square root of the map size, used for grid calculations
    private Vector3 currentPosition; // The current position in the grid where the next room will be placed
    private int currentPositionX, currentPositionZ; // The current X and Z coordinates in the grid
    private int positionTracker; // A tracker to keep track of the number of rooms placed in the grid

    private void Update()
    {
        mapSizeSquareRoot = (int)Mathf.Sqrt(mapSize);
    }

    public void ReloadWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GenerateWorld()
    {
        for (int i = 0; i < mapSize; i++)
        {
            
        }
    }
}
