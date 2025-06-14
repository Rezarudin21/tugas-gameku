using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int score = 10;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animationSprites.Length > 0)
            spriteRenderer.sprite = animationSprites[0];
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    private void AnimateSprite()
    {
        animationFrame++;
        if (animationFrame >= animationSprites.Length)
            animationFrame = 0;

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            if (GameManagershooter.Instance != null)
                GameManagershooter.Instance.OnInvaderKilled(this);

            Destroy(gameObject);
            Destroy(other.gameObject); // hancurkan laser juga
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            if (GameManagershooter.Instance != null)
                GameManagershooter.Instance.OnBoundaryReached();
        }
    }
}
