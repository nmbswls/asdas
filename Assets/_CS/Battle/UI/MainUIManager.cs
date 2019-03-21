using UnityEngine;
using System.Collections;
using FairyGUI;

public class MainUIManager : MonoBehaviour
{
	public GComponent _mainView;
	JoystickManager _joystick;
	SkillView _skillView;
	GComponent _buildBtn;

	public GTextField _enemy_left;
	public GTextField _coins;

	BuildWindow _buildWin;
	BattleFinishWindow _battleFinishWindow;

	GComponent[] _potions = new GComponent[3];
	GComponent _hp_bar;

	public float degree;
	public float offset;

	public static Vector3 BuildButtonPosInScreen;

	void Awake(){
		

		//_build.onClick.Add(() => { _bagWindow.Show(); });
	}

	// Use this for initialization
	void Start ()
	{

		UIPackage.AddPackage("FairyGUI/UIMain");
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/BuildItem", typeof(BuildItem));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/SkillItem", typeof(SkillItem));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/DetailPanel", typeof(BuildDetail));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerDamageItem", typeof(TowerDamageItem));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerSkillItem", typeof(TowerSkillItem));
		_mainView = this.GetComponent<UIPanel> ().ui;
		_joystick = new JoystickManager(_mainView);
		_joystick.onMove.Add(handleMove);
		_joystick.onEnd.Add (handleMoveEnd);


		_skillView = new SkillView(_mainView);
		_buildBtn = _mainView.GetChild ("build").asCom;
		_buildWin = new BuildWindow();
		_hp_bar = _mainView.GetChild ("hpBar").asCom;
		_buildBtn.onClick.Add(() => { _buildWin.Show();});

		_potions [0] = _mainView.GetChild ("potion_0").asCom;
		_potions [1] = _mainView.GetChild ("potion_1").asCom;
		_potions [2] = _mainView.GetChild ("potion_2").asCom;
		for (int i = 0; i < 3; i++) {
			int ii = i;
			_potions [i].onTouchBegin.Add (delegate() {
				BattleManager.getInstance().useItem(ii);
			});
		}
		_battleFinishWindow = new BattleFinishWindow ();


		GRoot.inst.onSizeChanged.Add (onWindowResize);
		BuildButtonPosInScreen = GRoot.inst.LocalToGlobal (_buildBtn.position);

		_enemy_left = _mainView.GetChild ("enemy_left").asTextField;
		_coins = _mainView.GetChild ("coins").asTextField;


		updatePotions ();
		updateHp ();
	}

	void handleMove(EventContext context){
		float[] data = (float[])context.data;
		degree = data [0];
		offset = data [1];
		//BattleManager.getInstance().player.gl.pCtrl.
	}

	void handleMoveEnd()
	{
		degree = 0;
		offset = 0;
	}

	void onWindowResize(){
		BuildButtonPosInScreen = GRoot.inst.LocalToGlobal (_buildBtn.position);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("r")) {
			_skillView.Rotate ();
		}
		_skillView.updateSkillView ();
	}

	public void activeTranstion(){
		_buildBtn.GetTransition ("t0").Play ();
	}

	public void showBattleFinishPanel(){
		_battleFinishWindow.Show ();
	}

	public void updatePotions(){
		for (int i = 0; i < 3; i++) {
			Potion potion = BattleManager.getInstance ().potionInBattle [i];
			if (potion != null) {
				_potions [i].GetChild ("n1").asLoader.url = "image/apple";
			} else {
				_potions [i].GetChild ("n1").asLoader.url = "";
			}

		}
	}

	public void updateHp(){
		_hp_bar.GetChild ("process").asImage.fillAmount = BattleManager.getInstance().Hp * 1.0f / BattleManager.getInstance().maxHP;
		_hp_bar.GetChild ("v").asTextField.text = BattleManager.getInstance ().Hp + "/" + BattleManager.getInstance ().maxHP;
	}
}

