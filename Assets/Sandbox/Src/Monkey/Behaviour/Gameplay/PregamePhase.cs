using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* TODO Change Monkey orientation when being dragged */
public class PregamePhase : MonoBehaviour
{
    /*** Public variables ***/
    public bool verbose = false;

    [Header("Dragging")]
    [Range(0.01f, 0.1f)]
    public float dragSpeed = 0.05f;

    public float minimumPointerMovementBeforeDragging = 0.1f;

    /*** Private variables ***/
    private Rigidbody2D monkeyRigidbody;

    private PolygonCollider2D monkeyCollider;

    private Moveset moveset;

    private UnityEvent playerClickOnMonkeyEvent = new UnityEvent();

    private UnityEvent notOnGroundEvent = new UnityEvent();

    private bool isPointerDown = false;

    private bool isDraggingMonkey = false;

    private bool isReady = false;

    private bool startedClickOnMonkey = false;

    private Vector2 initialClickPosition = new Vector2();

    void Start()
    {
        this.isDraggingMonkey = false;

        this.monkeyRigidbody = this.GetComponent<Rigidbody2D>();
        this.monkeyCollider = this.GetComponent<PolygonCollider2D>();
        this.moveset = this.GetComponent<Moveset>();
    }

    void Update()
    {
        Vector2 pointerPosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        /* Left click is pressed */
        {
            this.isPointerDown = true;
            this.initialClickPosition = pointerPosition;

            this.startedClickOnMonkey = false;
            foreach (Collider2D
                collider
                in
                Physics2D.OverlapPointAll(pointerPosition)
            )
            {
                if (this.monkeyCollider == collider)
                {
                    this.startedClickOnMonkey = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        /* Left click is released */
        {
            if (this.startedClickOnMonkey)
            /* Player releases click on Monkey */
            {
                if (
                    this
                        .IsMinimumPointerMovementBeforeDraggingReached(pointerPosition)
                )
                /* Player dragged Monkey */
                {
                    /* Stop any forces applied on Monkey before dropping it */
                    this.moveset.StopForces();
                }
                else if (this.isReady)
                /* Player clicked on Monkey */
                {
                    this.playerClickOnMonkeyEvent.Invoke();
                }
            }
            else
            /* player releases click on something else */
            {
                /* Nothing to do */
            }

            this.isPointerDown = false;
            this.startedClickOnMonkey = false;
            this.isDraggingMonkey = false;
        }

        if (this.isPointerDown)
        /* Player keeps pressing left click... */
        {
            if (this.startedClickOnMonkey)
            /* ... on Monkey :D */
            {
                if (
                    this
                        .IsMinimumPointerMovementBeforeDraggingReached(pointerPosition)
                )
                /* Player drags Monkey */
                {
                    /* TODO Play dragging animation */
                    this.isDraggingMonkey = true;
                }
                else
                /* Player keeps pressing the click on Monkey but doesn't drag */
                {
                    /* IDEA Allow Monkey orientation change */
                    /* Nothing to do */
                }
            }
            else
            /* ... on something else :( */
            {
                /* Nothing to do */
            }
        }
        if (this.isDraggingMonkey)
        /* Update Monkey's position and orientation */
        {
            Vector2 newPosition =
                this.GetMonkeyNextPositionTowardsPointer(pointerPosition);

            if (this.transform.position.x > newPosition.x)
            /* Pointer is at Monkey's left */
            {
                this.moveset.SwitchOrientation(Direction.LEFT);
            }
            else
            /* Pointer is at Monkey's right */
            {
                this.moveset.SwitchOrientation(Direction.RIGHT);
            }

            this.monkeyRigidbody.MovePosition(newPosition);
        }
    }

    private bool
    IsMinimumPointerMovementBeforeDraggingReached(Vector2 newPointerPosition)
    {
        bool isMinimumPointerMovementBeforeDraggingReached =
            Mathf.Abs(this.initialClickPosition.x - newPointerPosition.x) >
            this.minimumPointerMovementBeforeDragging ||
            Mathf.Abs(this.initialClickPosition.y - newPointerPosition.y) >
            this.minimumPointerMovementBeforeDragging;

        return isMinimumPointerMovementBeforeDraggingReached;
    }

    private Vector2 GetMonkeyNextPositionTowardsPointer(Vector2 pointerPosition)
    {
        return Vector2
            .Lerp(this.transform.position, pointerPosition, this.dragSpeed);
    }

    public void ManageCollision(Collision2D collision)
    {
        if (!this.isDraggingMonkey)
        {
            /********************************************************************************************************/
            /* Since Monkey is not being dragged, all its forces on the X axis should have previously been stopped, */
            /* therefore Monkey can only collide from top to bottom, hence checking ground collision.               */
            /********************************************************************************************************/
            if (collision.gameObject.tag == "Ground")
            {
                this.isReady = true;
            }
            else
            {
                this.notOnGroundEvent.Invoke();
            }
        }
    }

    public void StopDragging()
    {
        this.isDraggingMonkey = false;
    }

    public void SetPlayerClickEvent(UnityAction call)
    {
        this.playerClickOnMonkeyEvent.AddListener(call);
    }

    public void SetNotOnGroundEvent(UnityAction call)
    {
        this.notOnGroundEvent.AddListener(call);
    }
}
