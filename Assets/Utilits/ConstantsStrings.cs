public static class ConstantsStrings
{
    public const string MainMenu = "MainMenu";
    public const string GameScene = "GameScene";
    public const string GameSceneAR = "GameSceneAR";
    public const string PlayerTag = "Player";
    public const string CoinTag = "Coin";
    public const string DangerTag = "Danger";
    public const string TriggerRoadTag = "TriggerRoad";

}

public struct RoadProperties
{
    public const int MIN_ROAD_SIZE = 2;
    public const int MAX_ROAD_SIZE = 100;

    public const float MIN_ROAD_SPEED_3D_GAME = 1;
    public const float MAX_ROAD_SPEED_3D_GAME = 135f;

    public const float MIN_ROAD_SPEED_AR_GAME = MIN_ROAD_SPEED_3D_GAME / 100f;
    public const float MAX_ROAD_SPEED_AR_GAME = MAX_ROAD_SPEED_3D_GAME / 100f;
}