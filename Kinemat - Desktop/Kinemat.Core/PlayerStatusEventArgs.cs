using System;

namespace Kinemat.Core
{
    /// <summary>
    /// Event arguments for PlayerTracker.PlayerStatusChanged event.
    /// </summary>
    /// <typeparam name="TPlayer">
    /// Type used to represent a Kinect player.
    /// </typeparam>
    public class PlayerStatusEventArgs<TPlayer> : EventArgs
    {
        /// <summary>
        /// Player whose status has changed.
        /// </summary>
        public TPlayer Player { get; set; }

        /// <summary>
        /// Current player status.
        /// </summary>
        public PlayerStatus Status { get; set; }
    }
}
