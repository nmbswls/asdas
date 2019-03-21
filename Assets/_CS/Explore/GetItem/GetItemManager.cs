using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class GetItemManager : Window
{
	public static int MAX_TO_CHOOSE = 3;

	GList _new_item_list;
	GButton _confirm;
	GRichTextField _skip;

	int nowIndx = 0;
	List<string> items = new List<string>();
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "GetNewItem").asCom;
		this.Center();
		this.modal = true;

		_new_item_list = this.contentPane.GetChild("list").asList;
		_new_item_list.EnsureBoundsCorrect();


		_confirm = this.contentPane.GetChild ("confirm").asButton;
		_confirm.onTouchBegin.Add (chooseItem);
		_confirm.visible = false;

		_skip = this.contentPane.GetChild ("skip").asRichTextField;
		_skip.onTouchBegin.Add (skip);

	}

	void chooseItem(){
		int idx = _new_item_list.selectedIndex;
		//Debug.Log (idx);
		PlayerData.getInstance ().gainComponent (items [idx]);
		Vector3 posLocal = _new_item_list.GetChildAt (idx).position;
		GameManager.getInstance ().initGetItemEffect (_new_item_list.LocalToGlobal (posLocal),items[idx]);
		GameManager.getInstance ().finishItemGet ();
		this.Hide ();
	}

	void skip(){
		this.Hide ();
	}



	public void func(){
		string[] rewards = new string[6];
		for (int i = 0; i < 6; i++) {
		
		}
	}



	public void initAndShow(List<string> items){
		this.items = items;
		Show ();
	}

	protected override void OnShown(){

		_new_item_list.ClearSelection();
		_new_item_list.RemoveChildrenToPool();


		for(int i=0;i<items.Count;i++)
		{
			NewItem item = (NewItem)_new_item_list.AddItemFromPool();
			item.init (items[i]);
			item.onTouchBegin.Add (delegate() {
				if(_new_item_list.selectedIndex!=-1){
					_confirm.visible = true;
				}
			});
			item.GetChild ("detail").onTouchBegin.Add (delegate() {
				Debug.Log("Show Detail");
			});
		}


		_confirm.visible = false;

	}
}

