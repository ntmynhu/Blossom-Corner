using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBackground : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the main camera  
        mainCamera = Camera.main;

        // Get the SpriteRenderer component of the background image
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Call the method to resize the background image
        ResizeBackgroundImage();
    }

    private void ResizeBackgroundImage()
    {
        if (mainCamera == null || spriteRenderer == null)
        {
            Debug.LogWarning("Main camera or sprite renderer not found.");
            return;
        }

        // Get the size of the background image
        float bgWidth = spriteRenderer.bounds.size.x;
        float bgHeight = spriteRenderer.bounds.size.y;

        // Get the size of the camera's viewport
        float camHeight = mainCamera.orthographicSize * 2f;
        float camWidth = camHeight * mainCamera.aspect;

        // Calculate the scale needed to fit the background image into the camera's viewport while preserving aspect ratio
        float scaleX = camWidth / bgWidth;
        float scaleY = camHeight / bgHeight;
        float scale = Mathf.Max(scaleX, scaleY);

        // Apply the scale to the background image
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
