using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
	/**
     * An event raised when the master client has changed the game state.
     **/
	public sealed class TowerStateChangedEvent : PhotonNetworkEvent
	{
		public TowerStateChangedEvent(int validTowerState)
		{
			eventCode = NetworkEventCodes.TowerStateChanged;
			setReceivers(ReceiverGroup.All);

			content = validTowerState;
		}

		/**
         * Tries to parse the contents of the event.
         **/
		public static bool TryParse(object obj, out int newTowerState)
		{
			if (obj is int)
			{
				newTowerState = (int) obj;
				return true;
			}

			newTowerState = -1;
			return false;
		}
	}
}