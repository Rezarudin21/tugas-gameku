using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public Player owner; // Null jika ini adalah misil dari invader
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Gerakkan peluru ke arah tertentu
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        // 1. Lewati jika ini missile invader dan menyentuh invader lain
        if (owner == null && other.GetComponent<Invader>() != null)
            return;

        // 2. Lewati jika menyentuh pemilik peluru sendiri (contoh: player)
        if (owner != null && other.gameObject == owner.gameObject)
            return;

        // 3. Cek bunker
        Bunker bunker = other.GetComponent<Bunker>();
        if (bunker == null || bunker.CheckCollision(boxCollider, transform.position))
        {
            Destroy(gameObject);
            return;
        }

        // 4. Jika peluru ini bukan milik player, dan mengenai player
        if (owner == null && other.CompareTag("Player"))
        {
            Player hitPlayer = other.GetComponent<Player>();
            if (hitPlayer != null && GameManagershooter.Instance != null)
            {
                GameManagershooter.Instance.OnPlayerKilled(hitPlayer);
            }

            Destroy(other.gameObject); // Hapus player
            Destroy(gameObject);       // Hapus missile
            return;
        }

        // Tambahan: bisa ditambahkan logika mengenai collision lainnya jika diperlukan

        // Debug (opsional, untuk testing):
        // Debug.Log("Projectile hit: " + other.gameObject.name);
    }

    private void OnDestroy()
    {
        // Tidak perlu handle khusus untuk peluru dihapus
    }
}
