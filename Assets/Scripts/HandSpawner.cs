using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpawner : MonoBehaviour
{
    HandController[] hands;

    public int poolSize = 30;
    public GameObject handPrefab;

    void Awake()
    {
        InitializePool();
    }

    void InitializePool()
    {
        hands = new HandController[poolSize];
        for (int i = 0; i < poolSize; ++i)
        {
            hands[i] = Instantiate(handPrefab, transform).GetComponent<HandController>();
            hands[i].gameObject.SetActive(false);
        }
    }

    public HandController GetHand()
    {
        for (int i = 0; i < poolSize; ++i)
        {
            if (!hands[i].isHandActive)
            {
                return hands[i];
            }
        }
        return null;
    }
}
