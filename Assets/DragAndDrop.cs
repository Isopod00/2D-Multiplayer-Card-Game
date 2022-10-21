using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    public GameObject Canvas;

    private bool isDragging = false;
    private float offsetX = 0;
    private float offsetY = 0;

    // Start is called before the first frame update
    void Start() {
        Canvas = GameObject.Find("Canvas");
    }

    // This method is called when the dragging begins
    public void startDrag() {
        isDragging = true;
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
    }
    // This method is called when the dragging ends
    public void endDrag() {
        isDragging = false;
    }

    // Update is called once per frame
    void Update() {
        if (isDragging) {
            transform.position = new Vector2(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY);
            transform.SetParent(Canvas.transform, true);
        }
    }
}