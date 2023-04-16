using UnityEngine;

public class MonkeyAnimation : MonoBehaviour
{
    /*** Public variables ***/
    [Header("Sprites")]
    public Sprite monkeyStandsSprite;

    public Sprite monkeyFallsSprite;

    /*** Private variables ***/
    private SpriteRenderer spriteRenderer;

    private Collider2D spriteCollider;

    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.spriteCollider = this.GetComponent<Collider2D>();
    }

    public void MonkeyStands()
    {
        this.spriteRenderer.sprite = this.monkeyStandsSprite;
        this.spriteCollider.offset =
            new Vector2(this.spriteCollider.offset.x, 0f);
    }

    public void MonkeyFalls()
    {
        this.spriteRenderer.sprite = this.monkeyFallsSprite;
        this.spriteCollider.offset =
            new Vector2(this.spriteCollider.offset.x, 0.35f);
    }
}
