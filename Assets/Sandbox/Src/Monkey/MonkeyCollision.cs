using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyCollision
{
    public Collision2D collision { get; }

    /**************************************************************************/
    /* CollisionLocation defines the location where the collider hits Monkey. */
    /* For example:                                                           */
    /* if Monkey falls on ground, CollisionLocation will be Bottom.           */
    /* If Monkey hits a wall on its left, CollisionLocation will be Left.     */
    /**************************************************************************/
    public Direction collisionLocation { get; }

    public MonkeyCollision(
        Collision2D enteringCollision,
        PolygonCollider2D monkeyCollider
    )
    {
        this.collision = enteringCollision;

        if (
            monkeyCollider.bounds.min.y >
            enteringCollision.collider.bounds.max.y
        )
        {
            this.collisionLocation = Direction.BOTTOM;
        }
        else if (
            monkeyCollider.bounds.max.y <
            enteringCollision.collider.bounds.min.y
        )
        {
            this.collisionLocation = Direction.TOP;
        }
        else if (
            monkeyCollider.bounds.min.x >
            enteringCollision.collider.bounds.max.x
        )
        {
            this.collisionLocation = Direction.LEFT;
        }
        else if (
            monkeyCollider.bounds.max.x <
            enteringCollision.collider.bounds.min.x
        )
        {
            this.collisionLocation = Direction.RIGHT;
        }
        else
        {
            /* Should not happen */
        }
    }
}
