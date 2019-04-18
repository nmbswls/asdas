using UnityEngine;
using System.Collections;
using FairyGUI;

public class GuidePanel : Window
{


	GLoader _close;
	GGraph _prev;
	GGraph _next;
	GLoader _picture;

	int nowIndex = 0;


	protected override void OnHide(){
		if (BattleManager.getInstance () != null) {
			BattleManager.getInstance ().unPause ();
		}

	}


	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "GuidePanel").asCom;
		this.Center();
		this.modal = true;

		_close = this.contentPane.GetChild("close").asLoader;
		_close.onClick.Add (delegate(EventContext context) {
			PlayerPrefs.SetInt("isFirstBattle",0);
			this.Hide();
		});

		_prev = this.contentPane.GetChild ("prev").asGraph;
		_prev.onClick.Add (prevGuide);
		_next = this.contentPane.GetChild ("next").asGraph;
		_next.onClick.Add (nextGuide);

		_picture = this.contentPane.GetChild ("picture").asLoader;
	}

	protected override void OnShown(){
		if (BattleManager.getInstance () != null) {
			BattleManager.getInstance ().pause ();
		}
		nowIndex = 0;
		_picture.url = "guide/" + nowIndex;
	}

	public void nextGuide(){
		if (nowIndex < 2) {
			nowIndex++;
		}
		_picture.url = "guide/" + nowIndex;
	}

	public void prevGuide(){
		if (nowIndex > 0) {
			nowIndex--;
		}
	}
}

