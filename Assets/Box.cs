using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private int myCandyLevel;

    private void Start()
    {
        InitBox();
    }

    void InitBox()
    {
        if (ES3.KeyExists(gameObject.name))
        {
            myCandyLevel = ES3.Load(gameObject.name, 0);

            GameObject transparentObject = TransCandyPooler.Instance.SpawnFromPool(transform.position, Quaternion.identity);
            transparentObject.transform.SetParent(transform);
            GameObject candy = CandyManager.instance.SpawnFromPool(transform.position, Quaternion.identity);
            candy.GetComponent<CandyStatus>().boxName = transform.gameObject.name;
            candy.GetComponent<CandyStatus>().level = myCandyLevel;

            transform.GetComponent<Box>().SetCandy(myCandyLevel);
            candy.transform.localScale = Vector3.one;
            candy.transform.position = transform.position;
            candy.transform.SetParent(transform); // 최종 위치에 도달하면 부모를 설정
            candy.transform.localPosition = Vector3.zero; // 로컬 위치를 0으로 설정
            candy.transform.localScale = Vector3.one; // 로컬 크기를 1로 설정
            transparentObject.transform.SetParent(null); // 부모 관계 끊기
            TransCandyPooler.Instance.ReturnToPool(transparentObject); // 투명한 오브젝트 풀로 반환

            CandyManager.instance.AddCount();
            CandyManager.instance.SetAppearance(candy);
        }
    }

    public void SetCandy(int candyLevel)
    {
        myCandyLevel = candyLevel;
        ES3.Save(gameObject.name, myCandyLevel);
    }

    public int GetCandy()
    {
        return myCandyLevel;
    }

    public void CandyNull()
    {
        ES3.Save(gameObject.name, null);
    }
}