using UnityEngine;

/// <summary>
/// This can be used for both keyboard input(digital) or joystick input(analog).
/// 1. In your MonoBehaviour script(In Update method), you must call ProcessInput to update horizontal input every frame.
/// 2. To check left or right input double clicked, just get RightClickCount, LeftClickCount and check it is bigger than 1.
/// </summary>
public class HorizontalInputDoubleClickChecker
{
	/// <summary>
	/// You must set chance time for clicking and double clicking. I recommand (0.1f,0.2f).
	/// </summary>
	/// <param name="_clickChanceTime">chance time for moving value to left end or right end</param>
	/// <param name="_doubleClickChanceTime">chance time for maintaining click count</param>
	public HorizontalInputDoubleClickChecker(float _clickChanceTime, float _doubleClickChanceTime)
	{
		clickChanceTime = _clickChanceTime;
		doubleClickChanceTime = _doubleClickChanceTime;
	}

	/// <summary>
	/// if LeftClickCount >= 2, you can see that left double click occured.
	/// </summary>
	public int LeftClickCount => leftClickCount;
	/// <summary>
	/// if RightClickCount >= 2, you can see that right double click occured.
	/// </summary>
	public int RightClickCount => rightClickCount;

	private float clickChanceTime;
	private float doubleClickChanceTime;

	/// <summary>
	/// Save the last time the click was counted, and initialize count if there is no next count in doubleClickChanceTime.
	/// </summary>
	private float lastClickCountedTime;
	/// <summary>
	/// Save the last time the x value in middle or right side.
	/// </summary>
	private float lastNotInLeftSideTime;
	/// <summary>
	/// Save the last time the x value in middle or left side.
	/// </summary>
	private float lastNotInRightSideTime;
	/// <summary>
	/// Current left click count.
	/// </summary>
	private int leftClickCount;
	/// <summary>
	/// Current right click count.
	/// </summary>
	private int rightClickCount;
	/// <summary>
	/// If x value is in middle. it changed to true. If x value is in end of both side, it changed to false.
	/// If it is not true, click count isn't added.
	/// </summary>
	private bool returnedToMiddle;

	/// <summary>
	/// If you want to reset both click count to 0, use this method.
	/// </summary>
	public void ResetClickCount() 
	{
		leftClickCount = 0;
		rightClickCount = 0;
	}

	/// <summary>
	/// You must call this in your MonoBehaviour script's Update method.
	/// </summary>
	/// <param name="inputX">Put your input value here.</param>
	public void ProcessInput(float inputX)
	{
		//If you want to test LeftClickCount or RightClickCount, use this line.
		//Debug.Log("LC Count: " + leftClickCount + " RC Count: " + rightClickCount);

		if (inputX < -0.8f)
		{
			//In left end
			lastNotInRightSideTime = Time.time;
			rightClickCount = 0;

			if (Time.time < lastNotInLeftSideTime + clickChanceTime && returnedToMiddle)
			{
				leftClickCount++;
				lastClickCountedTime = Time.time;
			}

			returnedToMiddle = false;
		}
		else if (inputX < -0.2f)
		{
			//In left side
			lastNotInRightSideTime = Time.time;
		}
		else if (inputX < 0.2f)
		{
			//In middle
			lastNotInLeftSideTime = Time.time;
			lastNotInRightSideTime = Time.time;
			returnedToMiddle = true;
		}
		else if (inputX < 0.8f)
		{
			//In right side
			lastNotInLeftSideTime = Time.time;
		}
		else
		{
			//In right end
			lastNotInLeftSideTime = Time.time;
			leftClickCount = 0;

			if (Time.time < lastNotInRightSideTime + clickChanceTime && returnedToMiddle)
			{
				rightClickCount++;
				lastClickCountedTime = Time.time;
			}

			returnedToMiddle = false;
		}

		//Process double click check
		if (Time.time > lastClickCountedTime + doubleClickChanceTime)
		{
			leftClickCount = 0;
			rightClickCount = 0;
		}
	}
}
