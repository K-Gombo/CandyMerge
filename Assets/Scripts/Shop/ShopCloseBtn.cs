using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCloseBtn : MonoBehaviour
{
 
    public GameObject ShopPanel;    // UpgradePanel
    public CandyController candyController;

    public void OnButtonClick()
    {
        ShopPanel.SetActive(false);
        candyController.EnableDrag(true);
    }
}
