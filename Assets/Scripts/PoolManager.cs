using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        
        for(int index = 0; index < pools.Length; ++index )
        {
            pools[index] = new List<GameObject>();
        }
    }

    public void ResetPoolManager()
    {
        #region Ǯ���� ��� ������Ʈ ��Ʈ����(����,��������(?),�Ѿ� ��)
        int x = prefabs.Length;
        for (int i = 0; i < x; i++)
        {
            // �� ����Ʈ�� ���̸� ������
            int y = pools[i].Count;

            // pools[i][j]�� �����ϱ� ���� ���� ����
            for (int j = 0; j < y; j++)
            {
                // pools[i][j]�� �����Ͽ� ���ϴ� �۾� ����
                GameObject currentObject = pools[i][j];
                Destroy(currentObject);
                currentObject = null;
            }
        }
        #endregion

        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; ++index)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 1. ������ Ǯ�� ��Ȱ��ȭ �� ���ӿ�����Ʈ ����
        // 2. �߰��ϸ� select ������ �Ҵ�
        // 3. �� ã������
        // 4. ���Ӱ� �����ϰ� select ������ �Ҵ�
        
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(select == null)
        {
            //transform�� ������ ���̾��Ű�� PoolManager�ȿ��� ������
            //�������� �� �� ���̾��Ű�� �ָ������� ����
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
