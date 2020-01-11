using UnityEngine;

public class Test_MainScript : MonoBehaviour
{
    public void SwitchStatusActivation(GameObject target)
    {
        bool isTurnOn = true;

        target.SetActive(target.activeInHierarchy ? !isTurnOn : isTurnOn);
    }

}