namespace TowerVR
{
    public class TurnTimeLimits
    {
        public const float SelectingTowerPiece = 2.0f;
        public const float PlacingTowerPiece = 3.0f;
        public const float TowerReacting = 5.0f;
        
        // No instantiation of class
        private TurnTimeLimits() {}
    }
    
    public class TowerConstants
    {
        public const float MaxTowerVelocity = 0.2f;
        public const float MaxTowerAngVelocity = 0.1f;
    }
}