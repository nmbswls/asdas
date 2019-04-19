using UnityEngine;
using System.Collections;
using FairyGUI;

public class RolePanel : GComponent
{
	GList l0;
	GList l1;
	GComponent _property;
	GTextField _san;
	GTextField _hp;
	GTextField _money;

	GLoader[] _talents = new GLoader[3];
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		l0 = this.GetChild ("potions").asList;
		l1 = this.GetChild ("scars").asList;
		_property = this.GetChild ("property").asCom;
		_san = _property.GetChild ("san").asTextField;
		_hp = _property.GetChild ("hp").asTextField;
		_money = _property.GetChild ("money").asTextField;

		_talents [0] = this.GetChild ("talent0").asLoader;
		_talents [1] = this.GetChild ("talent1").asLoader;
		_talents [2] = this.GetChild ("talent2").asLoader;
		updateView ();

	}

	public void updateView(){
		_hp.text = PlayerData.getInstance ().hp+"";
		_san.text = PlayerData.getInstance ().san+"";
		_money.text = PlayerData.getInstance ().money+"";

		UsableHeroInfo heroinfo = GameStaticData.getInstance ().heroes [PlayerData.getInstance ().heroIdx];
		for (int i = 0; i < heroinfo.talent.Length; i++) {
			int ii = i;
			_talents [i].onClick.Add (delegate() {
				if (heroinfo.talent [ii] == null) {
					GameManager.getInstance ().showDetailAmplifier ("无天赋",null);
				} else {
					GameManager.getInstance ().showDetailAmplifier ("talent",heroinfo.talent [ii]);
				}
			});

		}
//		for (int i = 0; i < _talents.Length; i++) {
//			_talents [i].onTouchBegin.Add (delegate() {
//				UsableHeroInfo heroinfo = GameStaticData.getInstance().heroes[PlayerData.getInstance().heroIdx]
//				GameManager.getInstance().showDetailAmplifier("减少生命上限");
//			});
//		}
		l0.RemoveChildrenToPool ();
		l1.RemoveChildrenToPool ();

		foreach (Potion p in PlayerData.getInstance().potions) {
			PotionSmall item = (PotionSmall)l0.AddItemFromPool ();
			item.setInfo (p);
		}
		foreach (Scar scar in PlayerData.getInstance().scars) {
			ScarSmallIcon item = (ScarSmallIcon)l1.AddItemFromPool ();
			item.setInfo (scar);
		}
	}
}

