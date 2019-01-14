using UnityEngine;
using System.Collections;
using FairyGUI;

public class MainUIManager : MonoBehaviour
{
	GComponent _mainView;
	JoystickManager _joystick;
	SkillComponent _skillView;
	GComponent _buildBtn;

	BuildWindow _buildWin;

	public static Vector3 BuildButtonPosInScreen;
	void Awake(){
		UIPackage.AddPackage("FairyGUI/UIMain");
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/BuildItem", typeof(BuildItem));
		//_build.onClick.Add(() => { _bagWindow.Show(); });
	}

	// Use this for initialization
	void Start ()
	{
		_mainView = this.GetComponent<UIPanel> ().ui;
		_joystick = new JoystickManager(_mainView);
		_skillView = new SkillComponent(_mainView);
		_buildBtn = _mainView.GetChild ("build").asCom;
		_buildWin = new BuildWindow();
		_buildBtn.onClick.Add(() => { _buildWin.Show(); Debug.Log("?");});

		GRoot.inst.onSizeChanged.Add (onWindowResize);
		BuildButtonPosInScreen = GRoot.inst.LocalToGlobal (_buildBtn.position);


	}

	void onWindowResize(){
		BuildButtonPosInScreen = GRoot.inst.LocalToGlobal (_buildBtn.position);
		Debug.Log (BuildButtonPosInScreen);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("r")) {
			_skillView.Rotate ();
		}
	}

	public void activeTranstion(){
		_buildBtn.GetTransition ("t0").Play ();
	}


}

