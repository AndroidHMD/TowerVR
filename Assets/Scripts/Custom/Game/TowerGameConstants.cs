namespace TowerVR
{
    public class TurnTimeLimits
    {
        public const float SelectingTowerPiece = 20.0f;
        public const float PlacingTowerPiece = 20.0f;
        public const float TowerReacting = 5.0f;
        
        // No instantiation of class
        private TurnTimeLimits() {}
    }
    
    public class TowerConstants
    {
        public const float MaxTowerVelocity = 0.025f;
        public const float MaxTowerAngVelocity = 0.025f;
        public const float IncreaseHeightTime = 4.0f;
        public const float RestoreTowerTime = 4.0f;
    }
}