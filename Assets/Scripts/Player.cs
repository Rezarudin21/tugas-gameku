using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;

    public GameObject shieldVisual; // Optional: Assign a shield visual GameObject in inspector
    private bool hasShield = true;

    private void Start()
    {
        ActivateShield();
    }

    private void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        transform.position = position;

        // Bisa tembak tanpa batasan / jeda
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Projectile newLaser = Instantiate(laserPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            newLaser.owner = this;
            newLaser.direction = Vector3.up;
            newLaser.gameObject.layer = LayerMask.NameToLayer("Laser");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            other.CompareTag("MysteryShip")) // Just in case dive-bomb hits
        {
            if (hasShield)
            {
                DeactivateShield();
                Destroy(other.gameObject); // Destroy missile or enemy
                return;
            }

            if (GameManager.Instance != null)
                GameManager.Instance.OnPlayerKilled(this);

            Destroy(gameObject);
        }
    }

    private void ActivateShield()
    {
        hasShield = true;
        if (shieldVisual != null)
            shieldVisual.SetActive(true);
    }

    private void DeactivateShield()
    {
        hasShield = false;
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    public void ResetPlayer()
    {
        ActivateShield();
        gameObject.SetActive(true);
    }
}
