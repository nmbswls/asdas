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

	int idx;

	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject ("UIMain", "TowerComponentPanel").asCom;
		this.Center ();
		this.modal = true;

		_components = this.contentPane.GetChild ("components").asList;
		_confirm = this.contentPane.GetChild ("n2").asButton;
		_close = this.contentPane.GetChild ("close").asLoader;

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

			obj.onClick.Add (delegate() {
				choose = -1;
				changeDetailView();
			});
		}
		int idx = 0;
		foreach (TowerComponent tc in PlayerData.getInstance ().bagComponents) {
			AccesoryView obj = (AccesoryView)_components.AddItemFromPool ();
			obj.updateView (tc);
			int ii = idx;
			obj.onClick.Add (delegate() {
				choose = ii;
				changeDetailView();
			});
			idx++;
		}


	}

	void changeDetailView(){
		string p = "";
		p += "";
		Dictionary<string,string[]> diff = new Dictionary<string,string[]> ();
		diff ["name"] = new string[2]{"无     ","     无"};
		if (nowComponent != null) {
			diff ["name"][0] = nowComponent.cname.PadLeft(6,' ');
			foreach (TowerComponentEffect effect in nowComponent.effects) {
				if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
					diff ["atk"] = new string[2]{effect.x+"","0"};
				}else if(effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE){
					diff ["atk_range"] = new string[2]{effect.x+"","0"};
				}else if(effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE){
					diff ["atk_spd"] = new string[2]{effect.x+"","0"};
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ATK){
					diff ["atk_"+effect.x] = new string[2]{effect.y+"","0"};
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ABILITY){
					diff ["extra" + effect.extra] = new string[2]{ effect.x+"", "0" };
				}
			}
		}
		if (choose != -1) {
			TowerComponent tc = PlayerData.getInstance ().bagComponents [choose];
			diff ["name"][1] = tc.cname.PadLeft(6,' ');
			foreach (TowerComponentEffect effect in tc.effects) {
				if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
					if (diff.ContainsKey ("atk")) {
						diff ["atk"] [1] = effect.x+"";
					} else {
						diff ["atk"] = new string[2]{"0",effect.x+""};
					}
				}else if(effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE){
					if (diff.ContainsKey ("atk_range")) {
						diff ["atk_range"] [1] = effect.x+"";
					} else {
						diff ["atk_range"] = new string[2]{"0",effect.x+""};
					}
				}else if(effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE){
					if (diff.ContainsKey ("atk_spd")) {
						diff ["atk_spd"] [1] = effect.x+"";
					} else {
						diff ["atk_spd"] = new string[2]{"0",effect.x+""};
					}
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ATK){
					if (diff.ContainsKey ("atk_"+effect.x)) {
						diff ["atk_"+effect.x] [1] = effect.y+"";
					} else {
						diff ["atk_"+effect.x] = new string[2]{"0",effect.y+""};
					}
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ABILITY){
					if (diff.ContainsKey ("extra" + effect.extra)) {
						diff ["extra" + effect.extra] [1] = effect.x+"";
					} else {
						diff ["extra" + effect.extra] = new string[2]{"0",effect.x+""};
					}
				}
			}
		}

		p += diff ["name"] [0];
		p += " << ";
		p += diff ["name"] [1];
		p += '\n';

		foreach(var kv in diff){
			if (kv.Key == "name")
				continue;
			p += kv.Key+"  ";
			p += kv.Value [0];
			p += " << ";
			p += kv.Value [1];
			p += '\n';
		}



		_txt_now.text = p;

	}

}

