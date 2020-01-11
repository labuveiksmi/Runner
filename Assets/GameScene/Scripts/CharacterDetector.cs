using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    public bool isShowDebugMessages = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantsStrings.PlayerTag))
        {
            if (isShowDebugMessages)
            {
                Debug.Log("The player entered to the trigger");
            }

            GameManager.Instance.ExtendRoad(true);
        }
        else
        {
            if (isShowDebugMessages)
            {
                Debug.Log("The trigger was activated");
            }
        }
    }
}