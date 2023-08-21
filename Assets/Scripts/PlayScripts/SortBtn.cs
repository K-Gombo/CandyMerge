using UnityEngine;
using System.Collections.Generic;

public class SortBtn : MonoBehaviour
{
    public Transform boxTile; // 에디터에서 직접 할당

    public void SortCandies()
    {
        // 모든 캔디를 저장할 리스트
        List<CandyStatus> allCandies = new List<CandyStatus>();

        // 모든 박스에서 캔디를 찾아 리스트에 추가
        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform child = boxTile.GetChild(i);
            if (child.CompareTag("Box") && child.childCount > 0)
            {
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform grandChild = child.GetChild(j);
                    if (grandChild.CompareTag("Candy")) // "Candy" 태그를 가진 자식 찾기
                    {
                        allCandies.Add(grandChild.GetComponent<CandyStatus>());
                        break; // 캔디를 찾았으므로 루프 종료
                    }
                }
            }
        }

        // 레벨에 따라 정렬 (내림차순)
        allCandies.Sort((c1, c2) => c2.level.CompareTo(c1.level));

        // 정렬된 순서대로 캔디를 박스에 배치
        int boxIndex = 0;
        foreach (CandyStatus candy in allCandies)
        {
            Transform targetBox = boxTile.GetChild(boxIndex);
            if (targetBox.CompareTag("Box"))
            {
                candy.transform.SetParent(targetBox);
                candy.transform.localPosition = Vector3.zero; // 로컬 위치를 0으로 설정
                boxIndex++;
            }
        }
    }
}