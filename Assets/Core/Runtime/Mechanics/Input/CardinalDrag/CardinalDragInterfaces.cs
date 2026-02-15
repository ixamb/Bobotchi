namespace Core.Runtime.Mechanics.Input.CardinalDrag
{
    public interface ILeftCardinalDragHandler
    {
        /// <summary>
        /// That function will be invoked everytime a left drag is being detected, according
        /// to the drag handler properties
        /// </summary>
        void OnLeftCardinalDrag(int dragUnits);
    }
    
    public interface IRightCardinalDragHandler
    {
        /// <summary>
        /// That function will be invoked everytime a right drag is being detected, according
        /// to the drag handler properties
        /// </summary>
        void OnRightCardinalDrag(int dragUnits);
    }
    
    public interface IUpCardinalDragHandler
    {
        /// <summary>
        /// That function will be invoked everytime an up drag is being detected, according
        /// to the drag handler properties
        /// </summary>
        void OnUpCardinalDrag(int dragUnits);
    }
    
    public interface IDownCardinalDragHandler
    {
        /// <summary>
        /// That function will be invoked everytime a down drag is being detected, according
        /// to the drag handler properties
        /// </summary>
        void OnDownCardinalDrag(int dragUnits);
    }

    public interface ICardinalDragPressedHandler
    {
        void OnCardinalDragPressed();
    }

    public interface ICardinalDragReleasedHandler
    {
        void OnCardinalDragReleased();
    }
}