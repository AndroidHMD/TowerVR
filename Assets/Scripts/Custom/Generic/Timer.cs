using UnityEngine;
using System.Collections;

/**
 * Simple timer class. Is a Unity component that automatically updates its internal timer.
 **/
public class Timer : MonoBehaviour 
{
	/**
     * The timer's value, in seconds.
     **/
    public double time
    { get; private set; }
	
    private bool isActive;

	// Use this for initialization
	void Start () 
	{
		isActive = false;
        time = 0.0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActive)
        {
            time += Time.deltaTime; 
        }
	}
	
	
    /**
     * Start the timer. Returns its current timer count.
     **/
    public double start()
    {
        isActive = true;
        return time;
    }
    
    /**
     * Stops the timer. Returns its current timer count.
     **/
    public double stop()
    {
        isActive = false;
        return time;
    }
    
    /**
     * Clears the timer. Returns the timer count before setting it to zero.
     **/
    public double clear()
    {
        stop();
        
        var prev = time;
        time = 0.0;
        
        return prev;
    }
}
