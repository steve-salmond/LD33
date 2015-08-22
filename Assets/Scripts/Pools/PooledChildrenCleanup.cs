using UnityEngine;
using System.Collections;

public class PooledChildrenCleanup : MonoBehaviour 
{

    private void OnDisable()
    {
        if (!ObjectPool.HasInstance)
            return;

        var n = transform.childCount;
        for (var i = n - 1; i >= 0; i--)
            ObjectPool.Instance.ReturnObject(transform.GetChild(i).gameObject);
    }
}
