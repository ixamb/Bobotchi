namespace Core.Mechanics.Input.Swipe
{
    public interface ISwipePressedHandler
    {
        void OnSwipePressed();
    }
    
    public interface ILeftSwipeHandler
    {
        /// <summary>
        /// That function will be invoked everytime a left swipe is being detected, according
        /// to the swipe handler properties
        /// </summary>
        void OnLeftSwipe();
    }

    public interface IRightSwipeHandler
    {
        /// <summary>
        /// That function will be invoked everytime a right swipe is being detected, according
        /// to the swipe handler properties
        /// </summary>
        void OnRightSwipe();
    }

    public interface IUpSwipeHandler
    {
        /// <summary>
        /// That function will be invoked everytime an up swipe is being detected, according
        /// to the swipe handler properties
        /// </summary>
        void OnUpSwipe();
    }

    public interface IDownSwipeHandler
    {
        /// <summary>
        /// That function will be invoked everytime a down swipe is being detected, according
        /// to the swipe handler properties
        /// </summary>
        void OnDownSwipe();
    }
}