using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerPooler : MonoBehaviour
{
    [SerializeField] private Danger dangerPrefab = null;

    private List<Danger> dangersList = new List<Danger>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < 5; i++)
        {
            CreateNewItem();
        }
    }

    private Danger CreateNewItem()
    {
        Danger danger = Instantiate(dangerPrefab, transform);
        dangersList.Add(danger);
        danger.gameObject.SetActive(false);
        return danger;
    }

    public Danger GetPooledDanger()
    {
        Danger danger;
        for (int i = 0; i < dangersList.Count; i++)
        {
            danger = dangersList[i];
            if (!danger.gameObject.activeInHierarchy)
            {
                return danger;
            }
        }
        //If we have no vacant danger, we creating new one
        danger = CreateNewItem();
        return danger;
    }
}