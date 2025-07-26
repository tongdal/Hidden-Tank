using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 1. 프리펨들을 보관할 함수
    public GameObject[] prefabs;
    // 2. 풀 담당 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++) {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject GetObj(int index)
    {
        GameObject select = null;
        // 선택한 풀의 놀고 있는 게임 오브젝트 접근
        foreach (GameObject pool in pools[index]) {
            // 발견하면 변수 할당
            if (!pool.activeSelf) {
                select = pool;
                select.SetActive(true);
                break;
            }
        }
        // 없으면
        if (!select) {
            //새롭게 생성하고 변수 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
