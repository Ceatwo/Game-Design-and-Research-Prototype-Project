using UnityEngine;

// Places random trees from a list of prefabs onto the ground when the game starts.
// Put it on an empty object in the middle of the area you want trees in.
public class TreeSpawner : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public int numberOfTrees = 200;
    public float areaSize = 100f;

    // Keeps trees away from the middle so the player doesn't spawn inside one
    public float clearRadius = 5f;

    public float minScale = 0.8f;
    public float maxScale = 1.3f;

    // Same seed = same forest every time
    public int seed = 123;

    public LayerMask groundMask;

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        if (treePrefabs.Length == 0)
        {
            Debug.LogWarning("No tree prefabs added to the TreeSpawner!");
            return;
        }

        Random.InitState(seed);

        for (int i = 0; i < numberOfTrees; i++)
        {
            // Pick a random spot in the square area
            float x = Random.Range(-areaSize / 2f, areaSize / 2f);
            float z = Random.Range(-areaSize / 2f, areaSize / 2f);

            // Skip spots too close to the middle
            if (new Vector2(x, z).magnitude < clearRadius)
            {
                continue;
            }

            // Shoot a ray down from high up to find the ground height
            Vector3 rayStart = transform.position + new Vector3(x, 100f, z);
            RaycastHit hit;

            if (Physics.Raycast(rayStart, Vector3.down, out hit, 200f, groundMask))
            {
                GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                GameObject tree = Instantiate(prefab, hit.point, rotation, transform);
                tree.transform.localScale = prefab.transform.localScale * Random.Range(minScale, maxScale);
            }
        }
    }
}
