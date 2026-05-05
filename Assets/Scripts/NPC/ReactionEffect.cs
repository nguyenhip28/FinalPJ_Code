using UnityEngine;

public class ReactionEffect : MonoBehaviour
{
    public float speed = 1.5f;
    public float lifeTime = 1f;

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = lifeTime;

        // 👉 scale nhỏ lại (backup nếu quên chỉnh prefab)
        transform.localScale = Vector3.one * 0.3f;
    }

    void Update()
    {
        // ❌ BỎ localPosition
        // transform.localPosition += ...

        // ✅ DÙNG world position
        transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;

        // quay về camera
        transform.forward = Camera.main.transform.forward;

        // fade
        timer -= Time.deltaTime;
        if (sr != null)
        {
            Color c = sr.color;
            c.a = timer / lifeTime;
            sr.color = c;
        }

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}