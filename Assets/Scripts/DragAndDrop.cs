using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject DropZone;

    // Initalize variables for DropZone functionality
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private bool isOverDropZone;
    private bool immovable = false;

    // Initialize variables for dragging functionality
    private bool isDragging = false;
    private float offsetX = 0;
    private float offsetY = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Find the instances of these GameObjects in the scene
        Canvas = GameObject.Find("Canvas");
        DropZone = GameObject.Find("DropZone");
    }

    // This method is called when a collision begins
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }
    // This method is called when a collision ends
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }

    // This method is called when the dragging begins
    public void startDrag()
    {
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;

        // Calculate the offset from the cursor to the center of the card being dragged
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
    }
    // This method is called when the dragging ends
    public void endDrag()
    {
        isDragging = false;

        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
            immovable = true;
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging && !immovable)
        {
            // Update the position of the card (move it)
            transform.position = new Vector2(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY);
            transform.SetParent(Canvas.transform, true);
        }
    }
}