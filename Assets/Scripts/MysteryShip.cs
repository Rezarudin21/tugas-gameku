using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 25;
    public GameObject explosionPrefab;

    private Vector2 leftDestination;
    private Vector2 rightDestination;
    private int direction = 1;
    private int currentHealth;

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        leftDestination = new Vector2(leftEdge.x + 1f, transform.position.y);
        rightDestination = new Vector2(rightEdge.x - 1f, transform.position.y);

        currentHealth = maxHealth;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.right * direction;

        if (transform.position.x >= rightDestination.x || transform.position.x <= leftDestination.x)
        {
            direction *= -1; // balik arah
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            currentHealth--;

            if (currentHealth <= 0)
            {
                Explode();
                GameManager.Instance.OnMysteryShipKilled(this);
            }
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
 