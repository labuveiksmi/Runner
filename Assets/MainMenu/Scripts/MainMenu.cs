using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region PRIVATE VARIABLES

    [SerializeField] private Text textMusic;
    [SerializeField] private Text textSounds;

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

    public void OnMusicClick()
    {
        /* Switch music status  */
        isMusicOn = !isMusicOn;

        Debug.Log("Music status: " + isMusicOn);

        UpdateUI();

        /* Saving music status to storage */
        Storage.instance.SaveData(Storage.instance.aliasMusic, isMusicOn ? 1 : 0);
    }

    public void OnSoundsClick()
    {
        /* Switch sounds status  */
        isSoundsOn = !isSoundsOn;

        Debug.Log("Sounds status: " + isSoundsOn);

        UpdateUI();

        /* Saving sounds status to storage */
        Storage.instance.SaveData(Storage.instance.aliasSounds, isSoundsOn ? 1 : 0);
    }

    public void UpdateUI()
    {
        if (textMusic?.enabled == true)
        {
            textMusic.text = "Music: " + (isMusicOn ? "On" : "Off");
        }
        if (textSounds?.enabled == true)
        {
            textSounds.text = "Sounds: " + (isSoundsOn ? "On" : "Off");
        }
    }

    #endregion PUBLIC METHODS

    #region PRIVATE METHODS

    #region NATIVE

    private void Start()
    {
        Initialize();
    }

    #endregion NATIVE

    private void Initialize()
    {
        /* Getting data from storage */
        isMusicOn = Storage.instance.LoadData(Storage.instance.aliasMusic) == 1 ? true : false;
        isSoundsOn = Storage.instance.LoadData(Storage.instance.aliasSounds) == 1 ? true : false;
    }

    #endregion PRIVATE METHODS
}