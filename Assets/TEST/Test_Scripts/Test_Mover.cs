using UnityEngine;

public class Test_Mover : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            transform.Translate(GameManager.Instance.RoadMoovingSpeed * Time.deltaTime);
        }
    }
}