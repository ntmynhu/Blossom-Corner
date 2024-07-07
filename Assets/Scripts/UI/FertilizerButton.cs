using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerButton : HasNumberButton
{
    #region Singleton

    private static FertilizerButton instance;
    public static FertilizerButton Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = Resources.FindObjectsOfTypeAll<FertilizerButton>()[0];
        }
    }
    

    #endregion
}
