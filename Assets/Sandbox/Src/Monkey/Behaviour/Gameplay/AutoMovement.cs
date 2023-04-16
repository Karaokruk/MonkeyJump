using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// FIXME Changer MonkeyCollision en ray casting comme pour les WJ
public class AutoMovement : MonoBehaviour
{
    /*** Public variables ***/
    public bool verbose = false;

    public GameObject gameManager;

    [Header("Jump")]
    public Vector2 initialJumpForce = new Vector2(1, 2);

    public Vector2 walljumpForce = new Vector2(1, 6);

    /* new */
    public float walljumpTime = 0.2f;

    public float wallDistance = 0.5f;

    private float jumpTime;

    /*** Private variables ***/
    private GameHandler gameHandler;

    private PolygonCollider2D monkeyCollider;

    private UnityEvent monkeyFallsEvent = new UnityEvent();

    private UnityEvent monkeyWinsEvent = new UnityEvent();

    private UnityEvent monkeyWallJumpsEvent = new UnityEvent();

    private Moveset moveset;

    private LayerMask walljumpableLayer;

    private LayerMask walkableLayer;

    void Start()
    {
        this.gameHandler = this.gameManager.GetComponent<GameHandler>();

        this.monkeyCollider = this.GetComponent<PolygonCollider2D>();

        this.moveset = this.GetComponent<Moveset>();

        /* Layer masks */
        this.walljumpableLayer = LayerMask.GetMask("Walljumpables");
        this.walkableLayer = LayerMask.GetMask("Walkables");
    }

    private void FixedUpdate()
    {
        if (this.gameHandler.gameState == GameState.INGAME)
        /* Check wall jumps only during In-Game phase */
        {
            bool canWalljump = this.moveset.checkWalljump(this.wallDistance);

            if (canWalljump)
            {
                this.moveset.Walljump(this.walljumpForce);
                this.monkeyWallJumpsEvent.Invoke();
            }

            /* Check for bottom collision */
            DoubleRaycast doubleRaycast =
                new DoubleRaycast(this.walkableLayer, Direction.BOTTOM, .1f);
            Bounds bounds = this.monkeyCollider.bounds;
            float monkeyX = (bounds.min.x + bounds.max.x) / 2;
            float offset = (bounds.max.x - bounds.min.x) / 2;
            Vector2 raycastOrigin =
                new Vector2(monkeyX, this.monkeyCollider.bounds.min.y);
            if (doubleRaycast.CheckRaycasts(raycastOrigin, offset) > 0)
            {
                this.monkeyFallsEvent.Invoke();
            }
        }
    }

    public void StartAutomovement()
    {
        this.moveset.Jump(this.initialJumpForce);
    }

    public void ManageCollision(Collision2D collision)
    {
        MonkeyCollision monkeyCollision =
            new MonkeyCollision(collision, this.monkeyCollider);

        if (monkeyCollision.collisionLocation != Direction.TOP)
        {
            this.moveset.StopForces();
            switch (collision.gameObject.tag)
            {
                case "Ground":
                    this.monkeyFallsEvent.Invoke();
                    break;
                case "Enemy":
                    // TODO
                    break;
                case "Goal":
                    Debug
                        .Log("[DEBUG] Collision location: " +
                        monkeyCollision.collisionLocation);
                    if (monkeyCollision.collisionLocation == Direction.BOTTOM)
                    /* Monkey reached goal */
                    {
                        this.monkeyWinsEvent.Invoke();
                    }
                    else
                    /* Monkey touches goal gameObject, but did not land on it */
                    {
                        // this.ManageWalljump(monkeyCollision);
                    }
                    break;
                default:
                    // this.ManageWalljump(monkeyCollision);
                    break;
            }
        }
    }

    public void SetMonkeyFallsEvent(UnityAction call)
    {
        this.monkeyFallsEvent.AddListener(call);
    }

    public void SetMonkeyWinsEvent(UnityAction call)
    {
        this.monkeyWinsEvent.AddListener(call);
    }

    public void SetMonkeyWallJumpsEvent(UnityAction call)
    {
        this.monkeyWallJumpsEvent.AddListener(call);
    }
}
