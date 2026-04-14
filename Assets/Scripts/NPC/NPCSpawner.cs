using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public WaypointManager path;

    public bool spawnFromStart = true;
    public float spawnRate = 2f;

    // 🔥 THÊM 2 DÒNG NÀY
    public int maxNPC = 10;
    private int currentNPC = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnNPC), 1f, spawnRate);
    }

    void SpawnNPC()
    {
        if (currentNPC >= maxNPC) return;

        Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        GameObject npc = Instantiate(npcPrefab, transform.position + offset, Quaternion.identity);

        NPCMovement move = npc.GetComponent<NPCMovement>();
        move.path = path;

        move.SetStart(spawnFromStart);
        move.speed = Random.Range(1.5f, 2.5f);

        currentNPC++;

        npc.GetComponent<NPCMovement>().OnDestroyCallback = () => currentNPC--;
    }
}