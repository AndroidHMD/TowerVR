using UnityEngine;

/**
 * 	Singleton is a design pattern for ensuring that a class only ever has a single instance. Each class that inherits
 * 	from this abstract base class will automatically be a singleton, given that an instance of the class is attached to
 * 	a GameObject.
 * 
 * 	The singleton instance is retrieved by calling "ClassName".Instance
 * */
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;

	/**
	 * Returns the instance of this singleton
	 */
	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				Debug.LogError("An instance of " + typeof(T) + 
					" is needed in the scene, but there is none.");
			}

			return instance;
		}
	}

	/**
	 * Assign the singleton instance reference on script loading
	 */
	void Awake()
	{
		foreach(var manager in GameObject.FindGameObjectsWithTag("Manager"))
		{
			var singletonInstance = manager.GetComponent<T>();
			if (singletonInstance != null)
			{
				instance = singletonInstance;
				return;
			}
		}

	}
}