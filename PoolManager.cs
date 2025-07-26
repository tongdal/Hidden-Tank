using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 1. ��������� ������ �Լ�
    public GameObject[] prefabs;
    // 2. Ǯ ��� �ϴ� ����Ʈ��
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
        // ������ Ǯ�� ��� �ִ� ���� ������Ʈ ����
        foreach (GameObject pool in pools[index]) {
            // �߰��ϸ� ���� �Ҵ�
            if (!pool.activeSelf) {
                select = pool;
                select.SetActive(true);
                break;
            }
        }
        // ������
        if (!select) {
            //���Ӱ� �����ϰ� ���� �Ҵ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
