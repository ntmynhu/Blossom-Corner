using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTool : MonoBehaviour
{
    private Vector3 initialPos;
    private float offSet = 2f;
    private float offSetY = 1f;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void Update()
    {
        if (gameObject.activeSelf && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = GetMousePosition();
            if (Vector3.Distance(transform.position, mousePosition) <= offSet || transform.position != initialPos)
            {
                // Dịch công cụ lên trên ngón tay
                mousePosition.y += offSetY;
               
                transform.position = mousePosition;
            }
        }
        else
        {
            transform.position = initialPos;
            ToolManager.Instance.DeSelectTool();
        }
    }

    private Vector2 GetMousePosition()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
