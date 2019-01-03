namespace Kinemat.Core
{
    /// <summary>
    /// Represents the status of a Kinect player.
    /// </summary>
    public enum PlayerStatus
    {
        /// <summary>
        /// A new player has started interacting with Kinect sensor.
        /// </summary>
        Joined,

        /// <summary>
        /// A player has stopped interacting with Kinect sensor.
        /// </summary>
        Left,

        /// <summary>
        /// Data for a current player has been updated.
        /// </summary>
        Updated
    }
}
