using UnityEngine;

public class Test_MainScript : MonoBehaviour
{
    public GameObject panelActivatorGameObjects;


    private bool _isActive_PanelActivatorGameObjects = true;

    public void SwitchStatusActivation(GameObject target)
    {
        bool isTurnOn = true;

        target.SetActive(target.activeInHierarchy ? !isTurnOn : isTurnOn);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isActive_PanelActivatorGameObjects = !_isActive_PanelActivatorGameObjects;

            CheckerStatus_PanelActivatorGameObjects(isActivate: _isActive_PanelActivatorGameObjects);
        }
    }

    private void CheckerStatus_PanelActivatorGameObjects(bool isActivate)
    {
        if (isActivate)
        {
            panelActivatorGameObjects.SetActive(true);
        }
        else
        {
            panelActivatorGameObjects.SetActive(false);
        }
    }


}