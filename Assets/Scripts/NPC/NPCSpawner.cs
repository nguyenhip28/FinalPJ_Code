using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;
    public WaypointManager path;

    public bool spawnFromStart = true;

    [Header("Spawn Settings")]
    public float spawnDelay = 2f;   // ⏱️ thời gian giữa mỗi lần spawn
    public int spawnPerWave = 1;    // số NPC mỗi lần spawn

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            timer = 0f;

            for (int i = 0; i < spawnPerWave; i++)
            {
                SpawnNPC();
            }
        }
    }

    void SpawnNPC()
    {
        if (npcPrefabs.Length == 0) return;

        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        Vector3 offset = new Vector3(
            Random.Range(-1.5f, 1.5f),
            0,
            Random.Range(-1.5f, 1.5f)
        );

        GameObject npc = Instantiate(prefab, transform.position + offset, Quaternion.identity);

        NPCMovement move = npc.GetComponent<NPCMovement>();

        if (move != null)
        {
            move.path = path;
            move.SetStart(spawnFromStart);
            move.speed = Random.Range(1.5f, 2.5f);
        }
    }
}