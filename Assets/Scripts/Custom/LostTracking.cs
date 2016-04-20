using UnityEngine;

namespace Vuforia
{
	public class LostTracking : MonoBehaviour
	{
		public Cardboard myCardboard;
		public Camera myCamera;


		#region PRIVATE_MEMBER_VARIABLES

		private Transform lastTransform;

		#endregion // PRIVATE_MEMBER_VARIABLES

		void Update ()
		{
			var rot = Cardboard.SDK.HeadPose.Orientation;
			if (myCardboard.TrackingFound) 
			{
				//lastTransform = myCamera.transform;

				//Debug.Log("FoundTracking");
			}
			else //!myCardboard.TrackingFound
			{
				//myCamera.transform.position = lastTransform.position;
				//myCamera.transform.rotation = rot;

				//Debug.Log("LostTracking");
			}

		}

	}
}

