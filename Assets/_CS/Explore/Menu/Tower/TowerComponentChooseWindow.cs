using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class TowerComponentChooseWindow : Window
{

	TowerPanel tp;
	GList _components;
	GButton _confirm;
	GLoader _close;

	GTextField _txt_now;
	GTextField _txt_after;

	GTextField _c_before;
	GTextField _c_after;

	GList _changes;

	GButton _switch;
	bool isShownDetail = false;

	int idx;

	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject ("UIMain", "TowerComponentPanel").asCom;
		this.Center ();
		this.modal = true;

		_changes = this.contentPane.GetChild ("changes").asList;

		_c_before = this.contentPane.GetChild ("c_before").asTextField;
		_c_after = this.contentPane.GetChild ("c_after").asTextField;

		_components = this.contentPane.GetChild ("components").asList;
		_confirm = this.contentPane.GetChild ("n2").asButton;
		_close = this.contentPane.GetChild ("close").asLoader;

		_switch = this.contentPane.GetChild ("switch").asButton;
		_switch.onClick.Add (delegate() {


			isShownDetail = !isShownDetail;
			if (isShownDetail) {
				for (int i = 0; i < _components.numChildren; i++) {
					AccesoryView v = (AccesoryView)_components.GetChildAt (i);
					v.showDetail ();
				}
			}else {
				for (int i = 0; i < _components.numChildren; i++) {
					AccesoryView v = (AccesoryView)_components.GetChildAt (i);
					v.hideDetail ();
				}

			}

		});
		_switch.selected = true;

		_txt_now = this.contentPane.GetChild ("txt_now").asTextField;
		_txt_after = this.contentPane.GetChild ("txt_after").asTextField;
			
		_close.onTouchEnd.Add (delegate() {
			Hide();
		});

		_confirm.onTouchEnd.Add (delegate() {
			if(choose==-1){
				tp.unequip(idx);
			}else{
				tp.changeComponent(idx,choose);
			}
			Hide();
		});
	}

	public TowerComponentChooseWindow(TowerPanel tp):base(){
		this.tp = tp;
	}


	public void SetAndShow(int idx,TowerComponent nowTc){
		this.idx = idx;
		this.nowComponent = nowTc;
		Show ();
	}

	int choose = -1;
	TowerComponent nowComponent;

	protected override void OnShown ()
	{
		
		_components.RemoveChildrenToPool ();
		{
			AccesoryView obj = (AccesoryView)_components.AddItemFromPool ();

			obj.setUneuip (nowComponent);

			obj.onClick.Set (delegate() {
				_components.ClearSelection();
				obj.selected = true;
				choose = -1;
				changeDetailView();
			});
		}
		int idx = 0;
		foreach (TowerComponent tc in PlayerData.getInstance ().bagComponents) {
			AccesoryView obj = (AccesoryView)_components.AddItemFromPool ();
			obj.updateView (tc);
			int ii = idx;



			obj.onClick.Set (delegate() {
				_components.ClearSelection();
				obj.selected = true;
				choose = ii;
				changeDetailView();
			});
			idx++;
		}


	}

	void clickAccesoryView(int choose){
		
		this.choose = choose;
		changeDetailView();
	}

	void changeDetailView(){


		_changes.RemoveChildrenToPool ();

		string p = "";
		p += "";
		Dictionary<string,int[]> diff = new Dictionary<string,int[]> ();

		_c_before.text = "无";
		_c_after.text = "无";

		//diff ["name"] = new string[2]{"无     ","     无"};
		if (nowComponent != null) {
			_c_before.text = nowComponent.cname.PadLeft(6,' ');

			//diff ["name"][0] = nowComponent.cname.PadLeft(6,' ');
			foreach (TowerComponentEffect effect in nowComponent.effects) {
				if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
					diff ["atk"] = new int[2]{effect.x,0};
				}else if(effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE){
					diff ["atk_range"] = new int[2]{effect.x,0};
				}else if(effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE){
					diff ["atk_spd"] = new int[2]{effect.x,0};
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ATK){
					diff ["atk_p_"+effect.x] = new int[2]{effect.y,0};
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ABILITY){
					diff ["extra_" + effect.extra] = new int[2]{ effect.x, 0 };
				}
			}
		}
		if (choose != -1) {
			
			TowerComponent tc = PlayerData.getInstance ().bagComponents [choose];
			_c_after.text = tc.cname.PadLeft(6,' ');

			//diff ["name"][1] = tc.cname.PadLeft(6,' ');
			foreach (TowerComponentEffect effect in tc.effects) {
				if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
					if (diff.ContainsKey ("atk")) {
						diff ["atk"] [1] = effect.x;
					} else {
						diff ["atk"] = new int[2]{0,effect.x};
					}
				}else if(effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE){
					if (diff.ContainsKey ("atk_range")) {
						diff ["atk_range"] [1] = effect.x;
					} else {
						diff ["atk_range"] = new int[2]{0,effect.x};
					}
				}else if(effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE){
					if (diff.ContainsKey ("atk_spd")) {
						diff ["atk_spd"] [1] = effect.x;
					} else {
						diff ["atk_spd"] = new int[2]{0,effect.x};
					}
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ATK){
					if (diff.ContainsKey ("atk_"+effect.x)) {
						diff ["atk_p_"+effect.x] [1] = effect.y;
					} else {
						diff ["atk_p_"+effect.x] = new int[2]{0,effect.y};
					}
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ABILITY){
					if (diff.ContainsKey ("extra" + effect.extra)) {
						diff ["extra_" + effect.extra] [1] = effect.x;
					} else {
						diff ["extra_" + effect.extra] = new int[2]{0,effect.x};
					}
				}
			}
		}


		p += '\n';

		foreach(var kv in diff){
			
			if (kv.Key == "name")
				continue;
			if (kv.Key.StartsWith ("extra")) {
				PropertyCompareLine pline = (PropertyCompareLine)_changes.AddItemFromPool ();
				string sid = kv.Key.Substring (6);
				TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo (sid);
				pline.setAsProperty (ts.skillName, "lv"+kv.Value [1], (kv.Value [1]-kv.Value [0])+"");
			} else if (kv.Key.StartsWith ("atk_p")) {
				PropertyCompareLine pline = (PropertyCompareLine)_changes.AddItemFromPool ();
				string prop = kv.Key.Substring (6);
				eProperty pp = (eProperty)int.Parse (prop);
				string sv = string.Format ("{0:f1}", kv.Value [1]*0.001f);
				string schange = string.Format("{0:f1}", (kv.Value [1] - kv.Value [0]) * 0.001f);

				pline.setAsAtkProperty (pp, sv, schange);
			} else {
				PropertyCompareLine pline = (PropertyCompareLine)_changes.AddItemFromPool ();
				string trueKey = kv.Key.Replace ("_","\n");
				pline.setAsProperty (trueKey, kv.Value [1]+"", (kv.Value [1] - kv.Value [0])+"");
			}

			p += kv.Key+"  ";
			p += kv.Value [0];
			p += " << ";
			p += kv.Value [1];
			p += '\n';
		}



		_txt_now.text = p;

	}

}

