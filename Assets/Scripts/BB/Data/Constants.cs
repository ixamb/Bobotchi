namespace BB.Data
{
    public static class Constants
    {
        public static class SceneNames
        {
            public const string Base = "Base_Scene";
            public const string Game = "Game_Scene";
            public const string Onboarding = "Onboarding_Scene";
        }

        public static class PlayerPrefKeyParameters
        {
            public const string SfxVolume = "Sfx_Volume";
            public const string MusicVolume = "Music_Volume";
            public const string BackgroundColor = "Background_Color";
        }

        public static class ProgressStatStateThresholds
        {
            public const float Minimum = 0f;
            public const float Maximum = 100f;
        }

        public static class FurnitureDimension
        {
            public const uint XMinimum = 1;
            public const uint XMaximum = 3;
            public const uint YMinimum = 1;
            public const uint YMaximum = 3;
        }

        public static class LocalSaveEntries
        {
            public const string PlayInformation = "Play_Information";
            public const string PlayerInformation = "Player_Information";
            public const string CharacterBalances = "Character_Balances";
            public const string CharacterStateStats = "Character_State_Stats";
            public const string Inventory = "Inventory";
            public const string PropPlacements = "Prop_Placements";
            public const string RunningMission =  "Running_Mission";
        }
    }
}