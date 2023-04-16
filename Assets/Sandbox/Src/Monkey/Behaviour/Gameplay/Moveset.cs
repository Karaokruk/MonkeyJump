using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO Change walljump mecanics to Raycast */
public class Moveset : MonoBehaviour
{
    /*** Public variables ***/
    public bool verbose = false;

    public float moveSpeed = 3f;

    [Header("Raycasting")]
    public bool walljumpRayTrace = false;

    public float raycastYOffest = 0.0f;

    /*** Private variables ***/
    private Rigidbody2D monkeyRigidbody;

    private PolygonCollider2D monkeyCollider;

    private Direction direction;

    private LayerMask walljumpableLayer;

    void Start()
    {
        /** Monkey components */
        this.monkeyRigidbody = this.GetComponent<Rigidbody2D>();
        this.monkeyCollider = this.GetComponent<PolygonCollider2D>();

        /** Raycasting **/
        this.walljumpableLayer = LayerMask.GetMask("Walljumpables");

        /* Ignore Playing Area when raycasting  */
        Physics2D.queriesHitTriggers = false;

        /** Monkey movements **/
        this.ResetOrientation();
    }

    private void OnOrientationChange()
    {
        this.transform.localScale =
            new Vector2((this.direction == Direction.LEFT)
                    ? Mathf.Abs(this.transform.localScale.x) * -1
                    : Mathf.Abs(this.transform.localScale.x),
                this.transform.localScale.y);
    }

    public void SwitchOrientation(Direction orientation)
    {
        this.direction = orientation;
        this.OnOrientationChange();
    }

    public void SwitchOrientation()
    {
        this.SwitchOrientation((Direction)((float) this.direction * -1));
    }

    public void ResetOrientation()
    {
        this.SwitchOrientation(Direction.RIGHT);
    }

    public void StopForces()
    {
        this.monkeyRigidbody.velocity = Vector2.zero;
    }

    public void Jump(Vector2 jumpForce)
    {
        Vector2 orientedJumpForce =
            new Vector2(jumpForce.x * (float) this.direction, jumpForce.y);
        this
            .monkeyRigidbody
            .AddForce(orientedJumpForce * this.moveSpeed, ForceMode2D.Impulse);
    }

    public void Walljump(Vector2 walljumpForce)
    {
        /* First stop any previous force... */
        this.StopForces();

        /* ... then walljump! */
        this.SwitchOrientation();
        this.Jump(walljumpForce);
    }

    public bool checkWalljump(float wallDistance)
    {
        /********************************************************************************/
        /* Two rays are casts from Monkey to Wall,                                      */
        /* respectively from the top and bottom of its collider (modulo raycastYOffest) */
        /********************************************************************************/
        bool canWalljump = false;

        RaycastHit2D walljumpCheckHit;

        if (this.direction == Direction.LEFT)
        {
            walljumpCheckHit =
                Physics2D
                    .Raycast(new Vector2(this.monkeyCollider.bounds.min.x,
                        this.monkeyCollider.bounds.max.y - this.raycastYOffest),
                    new Vector2(-wallDistance, 0),
                    wallDistance,
                    this.walljumpableLayer);
            if (walljumpCheckHit)
            {
                canWalljump = true;
            }
            else
            {
                walljumpCheckHit =
                    Physics2D
                        .Raycast(new Vector2(this.monkeyCollider.bounds.min.x,
                            this.monkeyCollider.bounds.min.y +
                            this.raycastYOffest),
                        new Vector2(-wallDistance, 0),
                        wallDistance,
                        this.walljumpableLayer);
                if (walljumpCheckHit)
                {
                    canWalljump = true;
                }
            }
        }
        else
        /* (this.direction == Direction.RIGHT) */
        {
            walljumpCheckHit =
                Physics2D
                    .Raycast(new Vector2(this.monkeyCollider.bounds.max.x,
                        this.monkeyCollider.bounds.max.y - this.raycastYOffest),
                    new Vector2(wallDistance, 0),
                    wallDistance,
                    this.walljumpableLayer);
            if (walljumpCheckHit)
            {
                canWalljump = true;
            }
            else
            {
                walljumpCheckHit =
                    Physics2D
                        .Raycast(new Vector2(this.monkeyCollider.bounds.max.x,
                            this.monkeyCollider.bounds.min.y +
                            this.raycastYOffest),
                        new Vector2(wallDistance, 0),
                        wallDistance,
                        this.walljumpableLayer);
                if (walljumpCheckHit)
                {
                    canWalljump = true;
                }
            }
        }

        return canWalljump;
    }
}
