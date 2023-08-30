using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    public CandyController candyController; // CandyController에 대한 참조

    // MixBox에 캔디가 있는지 확인하고 있다면 원래 Box로 이동
    public bool CheckCandiesExistInMixBox()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox"); // 모든 MixBox를 찾음
        bool candyExistsInAnyMixBox = false;

        foreach (GameObject mixBox in mixBoxes)
        {
            Transform[] allChildren = mixBox.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("Candy"))
                {
                    candyExistsInAnyMixBox = true;
                    break;
                }
            }
            if (candyExistsInAnyMixBox)
            {
                break;
            }
        }

        return candyExistsInAnyMixBox;
    }
}
