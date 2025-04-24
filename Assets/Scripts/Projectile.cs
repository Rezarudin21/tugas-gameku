using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public Player owner; // Null jika ini adalah missile dari invader

    private BoxCollider2D boxCollider;
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
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
        // Lewati jika ini missile invader dan menyentuh invader lain
        if (owner == null && other.GetComponent<Invader>() != null)
        {
            return;
        }

        // Cek bunker
        Bunker bunker = other.gameObject.GetComponent<Bunker>();
        if (bunker == null || bunker.CheckCollision(boxCollider, transform.position)) {
            Destroy(gameObject);
            return;
        }

        // Jika peluru ini bukan punya player (misil invader) dan mengenai player
        if (owner == null && other.CompareTag("Player"))
        {
            Player hitPlayer = other.GetComponent<Player>();
            if (hitPlayer != null && GameManager.Instance != null) {
                GameManager.Instance.OnPlayerKilled(hitPlayer);
            }

            Destroy(other.gameObject); // Hapus player
            Destroy(gameObject);       // Hapus missile
        }
    }

    private void OnDestroy()
    {
        // Reset peluru hanya jika ini peluru milik player
        if (owner != null)
        {
            owner.OnLaserDestroyed();
        }
    }
}
