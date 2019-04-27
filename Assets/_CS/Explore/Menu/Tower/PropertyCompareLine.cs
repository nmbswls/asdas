using UnityEngine;
using System.Collections;
using FairyGUI;

public class PropertyCompareLine : GComponent
{

	GTextField _p_name;
	GLoader _p_loader;
	GTextField _p_value;
	PropertyModify _propertyModify;

	GTextField _skill_name;
	GTextField _plus;
	GTextField _minus;

	Controller _c0;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		_c0 = this.GetController ("type");

		_p_name = this.GetChild ("PropertyName").asTextField;
		_p_loader = this.GetChild ("prop").asLoader;
		_p_value = this.GetChild ("PropertyValue").asTextField;

		_propertyModify = (PropertyModify)this.GetChild ("PropertyModify").asCom;

		_skill_name = this.GetChild ("SkillName").asTextField;
		_plus = this.GetChild ("n14").asTextField;
		_minus = this.GetChild ("n14").asTextField;

	}

	public void setAsProperty(string pname,string value, string change){
		_p_name.text = pname;
		_p_value.text = value;
		_propertyModify.setValue (change);
		_c0.selectedIndex = 0;
		_p_loader.visible = false;
	}

	public void setAsAtkProperty(eProperty prop, string value, string change){
		_p_name.text = "Atk";
		_p_loader.visible = true;
		_p_loader.url = GameConstant.PROPERTY_ICON_PATH + prop.ToString();
		_p_value.text = value;
		_propertyModify.setValue (change);

		_c0.selectedIndex = 0;
	}

	public void setAsSkill(string sname, int beforeLv, int afterLv){
		_skill_name.text = sname;
		_c0.selectedIndex = 1;
	}

}

