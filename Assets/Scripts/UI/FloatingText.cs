using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
