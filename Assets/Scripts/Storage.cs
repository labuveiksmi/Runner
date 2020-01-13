using UnityEngine;

public enum GAME_TYPE { NOT_SELECTED = 0, SIMPLE_GAME, AR_GAME }

public class Storage : MonoBehaviour
{
    #region PUBLIC VARIABLES

    public static Storage instance = null;

    public readonly string aliasIsNotFirtTime = "Is not first time";
    public readonly string aliasMusic = "Music";
    public readonly string aliasSounds = "Sounds";
    public readonly string aliasHighScore = "High score";
    public readonly string aliasGameType = "Game type";

    #endregion PUBLIC VARIABLES

    #region PUBLIC METHODS

    public void SaveData(string alias, int value)
    {
        PlayerPrefs.SetInt(alias, value);

        Debug.LogFormat("{0} was saved with {1} value", alias, value);
    }

    public int LoadData(string alias)
    {
        Debug.LogFormat("{0} was returned with {1} value", alias, (PlayerPrefs.GetInt(alias, 0)));

        return PlayerPrefs.GetInt(alias, 0);
    }

    #endregion PUBLIC METHODS

    #region PRIVATE METHODS

    #region NATIVE

    private void Awake()
    {
        Singleton();

        InitializeData();
    }

    #endregion NATIVE

    private void Singleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void InitializeData()
    {
        if(LoadData(aliasIsNotFirtTime) == 0)
        {
            Debug.Log("Hello! New player!");

            SaveData(aliasIsNotFirtTime, 1);
            SaveData(aliasMusic, 1);
            SaveData(aliasSounds, 1);
            SaveData(aliasHighScore, 0);
            SaveData(aliasGameType, (int)GAME_TYPE.NOT_SELECTED);
        }
    }

    #endregion PRIVATE METHODS
}