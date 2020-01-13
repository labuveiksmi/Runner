using System.Collections.Generic;
using UnityEngine;

public class CoinsPooler : MonoBehaviour
{
	//TODO: For testing
	public Transform parent;

	[SerializeField] private Coin coinsPrefab = null;

	private List<Coin> CoinsList = new List<Coin>();

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

	private Coin CreateNewItem()
	{
		Coin coin = Instantiate(coinsPrefab, /*transform*/parent);
		CoinsList.Add(coin);
		coin.gameObject.SetActive(false);
		return coin;
	}

	public Coin GetPooledCoin()
	{
		Coin coin;
		for (int i = 0; i < CoinsList.Count; i++)
		{
			coin = CoinsList[i];
			if (!coin.gameObject.activeInHierarchy)
			{
				return coin;
			}
		}
		//If we have no vacant coin, we creating new one
		coin = CreateNewItem();
		return coin;
	}
}