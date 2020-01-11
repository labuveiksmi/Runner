using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private RoadPooler roadPooler;
    [SerializeField] private CoinsPooler coinsPooler;
    [SerializeField] private DangerPooler dangerPooler;

    public void CreateRoad(Vector3 position)
    {
        GameObject road = roadPooler.GetPooledRoad();
        road.transform.position = position;
        road.SetActive(true);
    }

    public void PopCoin(Vector3 position)
    {
        Coin coin = coinsPooler.GetPooledCoin();
        coin.transform.position = position;
        coin.gameObject.SetActive(true);
    }

    public void PopDanger(Vector3 position)
    {
        Danger danger = dangerPooler.GetPooledDanger();
        danger.transform.position = position;
        danger.gameObject.SetActive(true);
    }
}