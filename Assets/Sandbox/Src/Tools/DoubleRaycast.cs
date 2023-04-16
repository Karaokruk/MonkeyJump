using UnityEngine;

public class DoubleRaycast
{
    private LayerMask layerMask;

    private float distance;

    private struct RaycastsConfig
    {
        public Vector2 origin1;

        public Vector2 origin2;

        public Vector2 directionVector;
    }

    private delegate RaycastsConfig
        GetRaycastsConfigDelegate(Vector2 origin, float offset);

    private GetRaycastsConfigDelegate GetRaycastsConfig;

    public DoubleRaycast(
        LayerMask layerMask,
        Direction direction,
        float distance
    )
    {
        this.layerMask = layerMask;
        this.distance = distance;

        switch (direction)
        {
            case Direction.LEFT:
                this.GetRaycastsConfig = (origin, offset) =>
                    GetRaycastsConfigHorizontal(origin, offset, isLeft: true);
                break;
            case Direction.TOP:
                this.GetRaycastsConfig = (origin, offset) =>
                    GetRaycastsConfigVertical(origin, offset, isTop: true);
                break;
            case Direction.RIGHT:
                this.GetRaycastsConfig = (origin, offset) =>
                    GetRaycastsConfigHorizontal(origin, offset, isLeft: false);
                break;
            default: // case Direction.BOTTOM:
                this.GetRaycastsConfig = (origin, offset) =>
                    GetRaycastsConfigVertical(origin, offset, isTop: false);
                break;
        }
    }

    // FIXME return the collider instead (to check if ground/end/... etc.)
    ///<summary>
    ///<para>Given an origin and an offset, casts two rays towards the class direction and check if it hits a target belonging to the class layerMask.</para>
    ///<para>Note: if the offset is equal to 0, the two rays will be the exact same.</para>
    ///<return>Returns the number of rays that hit (either 0, 1 or 2)</return>
    ///</summary>
    public int CheckRaycasts(Vector2 origin, float offset)
    {
        RaycastsConfig config = this.GetRaycastsConfig(origin, offset);

        RaycastHit2D raycastHit1 =
            Physics2D
                .Raycast(config.origin1,
                config.directionVector,
                this.distance,
                this.layerMask);


        RaycastHit2D raycastHit2 =
            Physics2D
                .Raycast(config.origin2,
                config.directionVector,
                this.distance,
                this.layerMask);

        // Debug.DrawRay(config.origin1, config.directionVector, Color.magenta, .5f, depthTest: false);
        // Debug.DrawRay(config.origin2, config.directionVector, Color.yellow, .5f, depthTest: false);

        int nbHits = 0;
        if (raycastHit1)
        {
            nbHits++;
        }
        if (raycastHit2)
        {
            nbHits++;
        }
        return nbHits;
    }

    private static RaycastsConfig
    GetRaycastsConfigHorizontal(Vector2 origin, float offset, bool isLeft)
    {
        Vector2 topOrigin = new Vector2(origin.x, origin.y - offset);
        Vector2 bottomOrigin = new Vector2(origin.x, origin.y + offset);

        Vector2 directionVector = new Vector2(isLeft ? -1 : 1, 0);

        return new RaycastsConfig {
            origin1 = topOrigin,
            origin2 = bottomOrigin,
            directionVector = directionVector
        };
    }

    private static RaycastsConfig
    GetRaycastsConfigVertical(Vector2 origin, float offset, bool isTop)
    {
        Vector2 leftOrigin = new Vector2(origin.x - offset, origin.y);
        Vector2 rightOrigin = new Vector2(origin.x + offset, origin.y);

        Vector2 directionVector = new Vector2(0, isTop ? 1 : -1);

        return new RaycastsConfig {
            origin1 = leftOrigin,
            origin2 = rightOrigin,
            directionVector = directionVector
        };
    }
}
