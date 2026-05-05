using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;
    public WaypointManager path;

    public bool spawnFromStart = true;

    [Header("Spawn Settings")]
    public float spawnDelay = 2f;
    public int spawnPerWave = 1;

    [Header("Density Control")]
    public float spawnCheckRadius = 3f;
    public int maxNearbyNPC = 2;

    [Header("Global Limit")]
    public int maxTotalNPC = 5;

    public Transform npcContainer;

    public TimeManager timeManager; // 🔥 thêm

    private float timer;

    void Update()
    {
        // ❌ Không spawn nếu hết ngày
        if (timeManager != null && timeManager.IsDayEnded())
            return;

        timer += Time.deltaTime;

        float dynamicDelay = spawnDelay + GetDensityFactor();

        if (timer >= dynamicDelay)
        {
            timer = 0f;

            for (int i = 0; i < spawnPerWave; i++)
            {
                SpawnNPC();
            }
        }
    }

    float GetDensityFactor()
    {
        // ✅ tối ưu (không dùng FindGameObjectsWithTag nữa)
        int npcCount = npcContainer.childCount;
        return npcCount * 0.05f;
    }

    void SpawnNPC()
    {
        if (npcPrefabs.Length == 0 || path == null || path.Count() == 0)
            return;

        // ❌ Giới hạn tổng NPC
        if (npcContainer.childCount >= maxTotalNPC)
            return;

        Vector3 basePos = spawnFromStart
            ? path.GetWaypoint(0).position
            : path.GetWaypoint(path.Count() - 1).position;

        if (!CanSpawn(basePos)) return;

        Vector3 offset = transform.right * Random.Range(-0.5f, 0.5f);

        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        GameObject npc = Instantiate(prefab, basePos + offset, Quaternion.identity, npcContainer);

        NPCMovement move = npc.GetComponent<NPCMovement>();

        if (move != null)
        {
            move.path = path;
            move.SetStart(spawnFromStart);
            move.speed = Random.Range(1.5f, 2.5f);
        }
    }

    bool CanSpawn(Vector3 pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos, spawnCheckRadius);

        int count = 0;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("NPC"))
            {
                count++;
            }
        }

        return count < maxNearbyNPC;
    }

    public void ResetSpawner()
    {
        timer = 0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (path != null && path.Count() > 0)
        {
            Vector3 basePos = spawnFromStart
                ? path.GetWaypoint(0).position
                : path.GetWaypoint(path.Count() - 1).position;

            Gizmos.DrawWireSphere(basePos, spawnCheckRadius);
        }
    }
}