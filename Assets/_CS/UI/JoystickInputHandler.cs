using UnityEngine;
using System.Collections;
using FairyGUI;

public class JoystickInputHandler : MonoBehaviour
{
	JoystickManager _joystick;
	// Use this for initialization
	void Start ()
	{
		_joystick = new JoystickManager(this.GetComponent<UIPanel>().ui);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

