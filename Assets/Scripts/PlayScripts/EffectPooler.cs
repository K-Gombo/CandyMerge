using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EffectPooler : MonoBehaviour
{
    // 싱글톤 패턴
    public static EffectPooler Instance;

    // 풀링할 오브젝트 정보
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // EffectPool 빈 오브젝트
    public Transform effectPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, effectPool);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // 이펙트가 끝나는 시점에 비활성화
        StartCoroutine(DeactivateAfterSeconds(objectToSpawn, 2f)); // 예: 2초 후에 비활성화

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private IEnumerator DeactivateAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }
}