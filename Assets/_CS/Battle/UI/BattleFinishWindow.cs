using UnityEngine;
using System.Collections;
using FairyGUI;

public class BattleFinishWindow : Window
{

	GList _list;
	GComponent _detail;
	GComponent _confirmButton;

	public BattleFinishWindow()
	{
	}

	protected override void OnShown(){
		
	}

	public void closePanel(){
		BattleManager.getInstance ().battleFinish ();
		this.Hide ();
	}

	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "BattleFinishWindow").asCom;
		this.Center();
		this.modal = true;

		_list = this.contentPane.GetChild("list").asList;
		_list.onClickItem.Add(__clickItem);
		_list.itemRenderer = RenderListItem;
		_list.EnsureBoundsCorrect();


		_confirmButton = this.contentPane.GetChild ("confirm").asCom;
		_confirmButton.onTouchBegin.Add (closePanel);


	}

	void RenderListItem(int index, GObject obj)
	{
		BuildItem bi = (BuildItem)obj;

	}



	void __clickItem(EventContext context)
	{
		
	}
}

