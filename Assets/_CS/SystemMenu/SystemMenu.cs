using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;

public class SystemMenu : Singleton<SystemMenu> {

	int hasSave = 0;
	GComponent _main_menu;
	ShowClickMask _maskLayer;

	GComponent _newHeroPanel;
	GList _hero_list;
	GLoader _start_game;
	GTextField _info;
	GTextField _desp;


	GMovieClip effect;
	int choosedHeroIdx = -1;

	// Use this for initialization
	void Start () {
		UIPackage.AddPackage("FairyGUI/UIMain");
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/ClickShower", typeof(ShowClickMask));
		hasSave = PlayerPrefs.GetInt ("hasSave",0);

		_main_menu = GetComponent<UIPanel> ().ui;
		_main_menu.GetController ("c0").SetSelectedPage ("unsave");
		if (hasSave > 0) {
			_main_menu.GetController ("c0").SetSelectedPage ("save");
		}
		_main_menu.GetChild ("loadGame").onClick.Add (loadGame);
		_main_menu.GetChild ("newGame").onClick.Add (newGame);
		_main_menu.GetChild ("options").onClick.Add (options);
		_main_menu.GetChild ("quit").onClick.Add (quit);

		//_main_menu.onClick.Add (OnClick);
		_main_menu.onTouchBegin.Add (OnClickShow);
		effect = _main_menu.GetChild("effect").asMovieClip;
//		ShowClickMask _maskLayer = (ShowClickMask)UIPackage.CreateObject ("UIMain", "ClickShower").asCom;
//		_maskLayer.SetSize(GRoot.inst.width, GRoot.inst.height);
//		_maskLayer.AddRelation(GRoot.inst, RelationType.Size);
//
//
//
//		GRoot.inst.AddChild(_maskLayer);
//		_main_menu.AddChild (mask);
//		Debug.Log (_main_menu.numChildren);
//		Debug.Log (mask.position);

		_newHeroPanel = _main_menu.GetChild ("newHeroPanel").asCom;
		_info = _newHeroPanel.GetChild ("info").asTextField;
		_desp = _newHeroPanel.GetChild ("desp").asTextField;

		_start_game = _newHeroPanel.GetChild ("startGame").asLoader;
		_start_game.onClick.Add (delegate(EventContext context) {
			PlayerData.getInstance().heroIdx = choosedHeroIdx;
			enterGame();
		});

		_hero_list = _newHeroPanel.GetChild ("hero_list").asList;
		_hero_list.SetVirtualAndLoop();

		_hero_list.itemRenderer = RenderHeroes;
		_hero_list.numItems = GameStaticData.getInstance().heroes.Count;
		_hero_list.scrollPane.onScroll.Add(doHeroScrollEffect);
		_hero_list.scrollPane.onScrollEnd.Add (changeHeroDetail);
		doHeroScrollEffect ();
		changeHeroDetail ();
		{
			_info.text = GameStaticData.getInstance().heroes[choosedHeroIdx].name;
			_desp.text = GameStaticData.getInstance ().heroes [choosedHeroIdx].desp;
		}

	}
	void RenderHeroes(int index, GObject obj)
	{
		GButton item = (GButton)obj;
		item.touchable = false;
		item.SetPivot(0.5f, 0.5f);
		item.icon = "image/hero"+index;
	}


	void changeHeroDetail(){
		choosedHeroIdx = (_hero_list.GetFirstChildInView () + 1) % _hero_list.numItems;
		_info.text = GameStaticData.getInstance ().heroes [choosedHeroIdx].name;
		_desp.text = GameStaticData.getInstance ().heroes [choosedHeroIdx].desp;

		_hero_list.ClearSelection ();
		_hero_list.GetChildAt (1).asButton.selected = true;;

	}


	void doHeroScrollEffect()
	{
//		//change the scale according to the distance to middle
		float midX = _hero_list.scrollPane.posX + _hero_list.viewWidth / 2;
		int cnt = _hero_list.numChildren;
		for (int i = 0; i < cnt; i++)
		{
			GObject obj = _hero_list.GetChildAt(i);
			float dist = Mathf.Abs(midX - obj.x - obj.width / 2);
			if (dist > obj.width) { //no intersection
				obj.SetScale (1, 1);
			}
			else
			{
				float ss = 1 + (1 - dist / obj.width) * 0.36f;
				obj.SetScale(ss, ss);
			}
		}
		

	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnClickShow(EventContext context){
		InputEvent evt = (InputEvent)context.data;
		effect.visible = true;
		Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));
		effect.SetXY (pt.x - effect.width/2,pt.y-effect.height/2);
		effect.SetPlaySettings (0, -1, 1, -1);
		effect.playing = true;
		effect.onPlayEnd.Add (() => {
			effect.visible = false;
		});
	}

	void loadGame(){
		//PlayerData.getInstance ().initPlayerData ();
		if (hasSave>0) {
			_main_menu.GetTransition ("withLoad").Play (onComplete: enterGame);
		} else {
			_main_menu.GetTransition ("withoutLoad").Play (onComplete: enterGame);
		}

	}


	void newGame(){
		
		if (hasSave>0) {
			_main_menu.GetTransition ("withLoad").Play (onComplete: chooseHero);
		} else {
			_main_menu.GetTransition ("withoutLoad").Play (onComplete: chooseHero);
		}
	}



	private AsyncOperation async = null;
	IEnumerator LoadGameAsync()
	{
		async = SceneManager.LoadSceneAsync("Explore",LoadSceneMode.Single);
		yield return async;
	}

	void chooseHero(){
		_main_menu.GetController ("c0").SetSelectedPage ("new_hero");

	}

	void enterGame(){
		Debug.Log ("eee");
		_main_menu.GetController ("c0").SetSelectedPage ("empty");
		StartCoroutine (LoadGameAsync());
	}

	void options(){

	}
	void quit(){

	}
}
