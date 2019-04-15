using UnityEngine;
using System.Collections;
using FairyGUI;

public class GuideLayer : GComponent
{

	GObject window;
	Controller posController;
	GTextField txtLb;
	GTextField txtRt;
	GObject mark;
	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		window = this.GetChild("window");
		posController = this.GetController ("arrow");
		txtLb = this.GetChild ("hint_lb").asTextField;
		txtRt = this.GetChild ("hint_rt").asTextField;

		mark = this.GetChild ("mark");
	}

	public void setStyleRt(){
		posController.SetSelectedPage ("rt");
	}

	public void setStyleLb(){
		posController.SetSelectedPage ("lb");
	}

	public void setWindow(Rect rect){
		window.size = new Vector2((int)rect.size.x, (int)rect.size.y);
		window.position = new Vector2 ((int)rect.position.x, (int)rect.position.y);
	}

	public void setHint(string hint){
		txtLb.text = hint;
		txtRt.text = hint;
	}

	public void setMark(Vector2 pos){
		mark.position = pos;
	}
	public void setWindowTouchable(EventCallback0 callback){
		window.touchable = true;
		window.onClick.Add (callback);
	}
	public void setWindowUntouchable(){
		window.touchable = false;
		window.onClick.Clear ();
	}

}

