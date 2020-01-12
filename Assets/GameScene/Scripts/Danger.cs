using UnityEngine;

public class Danger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(ConstantsStrings.PlayerTag))
		{
			Debug.Log("picking up danger");
			GameManager.Instance.TakeLive();
			gameObject.SetActive(false);
		}
	}
}