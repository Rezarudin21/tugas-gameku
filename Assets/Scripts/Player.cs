using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    private Projectile laser;

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

        if (laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            laser = Instantiate(laserPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            laser.owner = this;
            laser.direction = Vector3.up;
            laser.gameObject.layer = LayerMask.NameToLayer("Laser");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {

            if (GameManager.Instance != null)
                GameManager.Instance.OnPlayerKilled(this);

            Destroy(gameObject);
        }
    }

    public void OnLaserDestroyed()
    {
        laser = null;
    }
}
