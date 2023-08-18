using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public GameObject ContainedCandy { get; private set; }

    public void SetCandy(GameObject candy)
    {
        ContainedCandy = candy;
    }

    public void ClearCandy()
    {
        ContainedCandy = null;
    }
}

