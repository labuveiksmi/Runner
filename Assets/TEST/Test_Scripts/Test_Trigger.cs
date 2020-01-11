using UnityEngine;

public class Test_Trigger : MonoBehaviour
{
    public delegate void OnEnter(GameObject target);
    public static event OnEnter OnEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnEnterTrigger?.Invoke(other.gameObject);
    }
}