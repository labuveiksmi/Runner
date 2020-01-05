using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region PRIVATE VARIABLES

    private bool isMusicOn = true;
    private bool isSoundsOn = true;

    #endregion PRIVATE VARIABLES

    #region PUBLIC METHODS

    public void OnStartGame()
    {
        Debug.Log("The player selected simple game");

        SceneManager.LoadScene(ConstantsStrings.GameScene);
    }

    public void OnStartARGame()
    {
        Debug.Log("The player selected AR game");
    }

    public void OnExitFromGame()
    {
#if UNITY_EDITOR

        Debug.Log("The player left the game");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnMusicClick(Text uiText)
    {
        /* Switch music status  */
        isMusicOn = !isMusicOn;

        Debug.Log("Music status: " + isMusicOn);

        /* Set text of music status to UI */
        uiText.text = "Music: " + (isMusicOn ? "On" : "Off");

        //TODO: Add saving music status to storage
        //...
        //Storage.Save(aliasMusic, isMusicOn);
    }

    public void OnSoundsClick(Text uiText)
    {
        /* Switch sounds status  */
        isSoundsOn = !isSoundsOn;

        Debug.Log("Sounds status: " + isSoundsOn);

        /* Set text of sounds status to UI */
        uiText.text = "Sounds: " + (isSoundsOn ? "On" : "Off");

        //TODO: Add saving sounds status to storage
        //...
        //Storage.Save(aliasSounds, isSoundsOn);
    }

    #endregion PUBLIC METHODS
}