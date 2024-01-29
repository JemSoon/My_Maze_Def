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
        #region 풀링된 모든 오브젝트 디스트로이(몬스터,근접무기(?),총알 등)
        int x = prefabs.Length;
        for (int i = 0; i < x; i++)
        {
            // 각 리스트의 길이를 가져옴
            int y = pools[i].Count;

            // pools[i][j]에 접근하기 위한 이중 포문
            for (int j = 0; j < y; j++)
            {
                // pools[i][j]에 접근하여 원하는 작업 수행
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

        // 1. 선택한 풀의 비활성화 된 게임오브젝트 접근
        // 2. 발견하면 select 변수에 할당
        // 3. 못 찾았으면
        // 4. 새롭게 생성하고 select 변수에 할당
        
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
            //transform을 적으면 하이어라키에 PoolManager안에서 생성됨
            //안적으면 맨 앞 하이어라키에 주르르르륵 생김
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
