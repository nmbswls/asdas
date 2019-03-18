using UnityEngine;
using System.Collections;
using FairyGUI;

public class MemoStoryWindow : Window
{

	GLoader _reward;
	MemoPanel memoPanel;
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject ("UIMain", "StoryBookWindow").asCom;
		this.Center ();
		this.modal = true;
		Debug.Log ("good");

		_reward = this.contentPane.GetChild ("reward").asLoader;
		_reward.visible = true;
		_reward.onTouchEnd.Add (delegate() {
			Hide();
			GameManager.getInstance().showUnlockWindow();
		});

	}
	public MemoStoryWindow(MemoPanel p):base(){
		memoPanel = p;
	}

}

