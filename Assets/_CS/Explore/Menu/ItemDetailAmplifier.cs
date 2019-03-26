using UnityEngine;
using System.Collections;
using FairyGUI;

public class ItemDetailAmplifier : Window
{
	GLoader close;
	GTextField _desp;
	GTextField _name;
	GLoader _pic;

	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "ItemDetail").asCom;
		this.Center();
		this.modal = true;

		_desp = this.contentPane.GetChild("desp").asTextField;
		_name = this.contentPane.GetChild("name").asTextField;
			
		_pic = this.contentPane.GetChild ("pic").asLoader;

		close = this.contentPane.GetChild("close").asLoader;
		close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});
	}
	string contentType;
	object content;
	public void setInfo(string contentType,object content){
		this.contentType = contentType;
		this.content = content;
	}

	public void setInfo(Scar scar){
		_pic.url = "image/Scar/scar" + scar.scarId;
		_desp.text = GameStaticData.getInstance ().getScarInfo (scar.scarId).scarName;
	}

	protected override void OnShown(){
		if (contentType == "scar") {
			Scar scar = (Scar)content;
			_pic.url = "image/Scar/" + scar.scarId;
			ScarStaticInfo sinfo = GameStaticData.getInstance ().getScarInfo (scar.scarId);
			_name.text = sinfo.scarName;
			_desp.text = sinfo.scarDesp;
		}else if (contentType == "potion") {
			Potion potion = (Potion)content;
			_pic.url = "image/Potion/" + potion.pid;
			PotionStaticInfo pinfo = GameStaticData.getInstance ().getPotionInfo (potion.pid);
			_name.text = pinfo.pname;
			_desp.text = pinfo.pname;
		}else if (contentType == "talent") {
			HeroTalent talent = (HeroTalent)content;
			_pic.url = "image/Talent/" + talent.talentId;
			_name.text = GameStaticData.getInstance ().talents[int.Parse(talent.talentId)].talentName;
			_desp.text = GameStaticData.getInstance ().talents[int.Parse(talent.talentId)].talentDesp;
		}else if (contentType == "tower_skill") {
			TowerSkillState skill = (TowerSkillState)content;
			_pic.url = "image/TowerSkill/" + skill.skillId;
			_name.text = GameStaticData.getInstance ().getTowerSkillInfo(skill.skillId).skillName;
			_desp.text = GameStaticData.getInstance ().getTowerSkillInfo(skill.skillId).skillDesp;
		}


		//_pic.url = "";
	}
}

