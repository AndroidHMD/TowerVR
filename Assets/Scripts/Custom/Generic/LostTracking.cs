using UnityEngine;

namespace Vuforia
{
	/**
	 * Class for handling events when tracking is lost. 
	 * Should save latest position and interpolate when tracking is found again
	 * 
	 * Not used at the moment! 
	 * 
	 **/


	public class LostTracking : MonoBehaviour
	{
		public Cardboard myCardboard;
		public Camera myCamera;


		#region PRIVATE_MEMBER_VARIABLES

		private Transform lastTransform;

		#endregion // PRIVATE_MEMBER_VARIABLES

		void Update ()
		{
			//var rot = Cardboard.SDK.HeadPose.Orientation;
			if (myCardboard.TrackingFound) 
			{
				//lastTransform = myCamera.transform;

				//Debug.Log("FoundTracking");
			}
			else //!myCardboard.TrackingFound
			{
				//myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, lastTransform.position, 1.0f);
				//myCamera.transform.position = lastTransform.position;
				//myCamera.transform.rotation = rot;

				//Debug.Log("LostTracking");
			}

		}

	}
}

