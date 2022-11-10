using Mirror;
using UnityEngine;

public class DragAndDrop : NetworkBehaviour
{
    public PlayerManager playerManager;

    // Initalize variables for DropZone functionality
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject Canvas;
    private GameObject dropZone;
    private bool isOverDropZone;

    // Initialize variables for dragging functionality
    private bool isDragging = false;
    private bool isDraggable = false;
    private float offsetX = 0;
    private float offsetY = 0;

    private int maxBoardSize = 8; // Specify the max number of cards each player can have on the board

    // Start is called before the first frame update
    void Start()
    {
        // Find the instances of these GameObjects in the scene
        Canvas = GameObject.Find("Canvas");
        dropZone = GameObject.Find("PlayerDropZone");

        if (hasAuthority)
        {
            isDraggable = true;
        }
    }

    // This method is called when a collision begins
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == dropZone)
        {
            isOverDropZone = true;
        }
    }
    // This method is called when a collision ends
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
    }

    // This method is called when the dragging begins
    public void startDrag()
    {
        if (isDraggable)
        {
            isDragging = true;
            startParent = transform.parent.gameObject;
            startPosition = transform.position;

            // Calculate the offset from the cursor to the center of the card being dragged
            offsetX = transform.position.x - Input.mousePosition.x;
            offsetY = transform.position.y - Input.mousePosition.y;
        }
    }
    // This method is called when the dragging ends
    public void endDrag()
    {
        isDragging = false;

        if (isDraggable)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();

            ThisCard script = gameObject.GetComponent<ThisCard>(); // Access this script from the new card object

            // "Snap" the played card to the dropZone
            if (isOverDropZone && dropZone.transform.childCount < maxBoardSize && playerManager.getGold() >= script.getThis().getCost() && playerManager.myTurn())
            {
                isDraggable = false;
                transform.SetParent(dropZone.transform, false);
                playerManager.CmdPlayCard(gameObject);
            }
            // Otherwise, return it back to the player's hand
            else
            {
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging && isDraggable)
        {
            // Update the position of the card (move it)
            transform.position = new Vector2(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY);
            transform.SetParent(Canvas.transform, true);
        }
    }
}