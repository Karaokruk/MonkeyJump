using UnityEngine;

public class Hitbox
{
    /** Hitbox is described as Vector4(left, top, right, bottom) **/
    private Vector4 monkeyHitbox;

    public float Left
    {
        get
        {
            return monkeyHitbox.x;
        }
    }

    public float Top
    {
        get
        {
            return monkeyHitbox.y;
        }
    }

    public float Right
    {
        get
        {
            return monkeyHitbox.z;
        }
    }

    public float Bottom
    {
        get
        {
            return monkeyHitbox.w;
        }
    }

    public Hitbox()
    {
        monkeyHitbox = new Vector4(0, 0, 0, 0);
    }

    public Hitbox(Vector4 hitbox)
    {
        monkeyHitbox = hitbox;
    }

    public Hitbox(float left, float top, float right, float bottom)
    {
        monkeyHitbox = new Vector4(left, top, right, bottom);
    }
}
