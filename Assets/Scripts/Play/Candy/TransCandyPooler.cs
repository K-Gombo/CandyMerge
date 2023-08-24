using UnityEngine;
using System.Collections.Generic;

public class TransCandyPooler : MonoBehaviour
{
    public static TransCandyPooler Instance; // 싱글톤 인스턴스

    [SerializeField]
    private GameObject transparentCandyPrefab; // 투명한 오브젝트 프리팹
    [SerializeField]
    private int poolSize = 10; // 풀 크기
    private Queue<GameObject> transparentCandyPool; // 투명한 오브젝트 풀
    public Transform transCandyPool;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        transparentCandyPool = new Queue<GameObject>();

        // 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(transparentCandyPrefab, transCandyPool); // 빈 오브젝트의 자식으로 생성
            obj.SetActive(false);
            transparentCandyPool.Enqueue(obj);
        }
    }

    // 투명한 오브젝트를 풀에서 가져오기
    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        if (transparentCandyPool.Count == 0)
        {
            // 풀이 비어있으면 새로운 오브젝트 생성
            GameObject obj = Instantiate(transparentCandyPrefab);
            obj.SetActive(false);
            transparentCandyPool.Enqueue(obj);
        }

        GameObject objectToSpawn = transparentCandyPool.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    // 투명한 오브젝트를 풀로 반환하기
    public void ReturnToPool(GameObject obj)
    {
        obj.transform.SetParent(transCandyPool); // transCandyPool의 자식으로 설정
        obj.transform.localPosition = Vector3.zero; // 로컬 위치 초기화
        obj.SetActive(false);
        transparentCandyPool.Enqueue(obj);
    }
}