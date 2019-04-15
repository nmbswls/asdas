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
		_confirm.onClick.Add (chooseItem);
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

		if (PlayerData.getInstance ().guideStage == 11) {
			GuideManager.getInstance ().showGuideDetail ();
			PlayerData.getInstance ().guideStage = 12;
		}

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
			item.onClick.Add (delegate() {
				if(_new_item_list.selectedIndex!=-1){
					_confirm.visible = true;
				}
				if (PlayerData.getInstance ().guideStage == 10) {
					GuideManager.getInstance ().showGuideConfirmChooseItem ();
					PlayerData.getInstance ().guideStage = 11;
				}
			});
			item.GetChild ("detail").onTouchBegin.Add (delegate() {
				//Debug.Log("Show Detail");
			});
		}


		_confirm.visible = false;

		if (PlayerData.getInstance ().guideStage == 4) {
			GuideManager.getInstance ().showGuideChooseItem ();
			PlayerData.getInstance ().guideStage = 10;
		}
	}



//	public GObject getFirstChoice(){
//		Rect rect = _new_item_list.GetChildAt(0).TransformRect(new Rect(0, 0, 100, 100), GRoot.inst);
//
//		return _new_item_list;
//	}
	public Rect getFirstChoice(){
		GObject firstItem = _new_item_list.GetChildAt (0);
		Rect rect = firstItem.TransformRect(new Rect(0, 0, firstItem.width, firstItem.height), GRoot.inst);
		_new_item_list.EnsureBoundsCorrect ();

		//Rect rectGlobal = _new_item_list.LocalToGlobal (rect);
		Vector2 center = _new_item_list.LocalToGlobal(new Vector3(firstItem.position.x+firstItem.width/2,firstItem.position.y+firstItem.height/2,0));
		Rect trueRect = new Rect (center.x-rect.size.x/2-10,center.y-rect.size.y/2,rect.size.x+20,rect.size.y+20);
		//GuideManager.getInstance ()._guideLayer.setMark (GuideManager.getInstance ()._guideLayer.GlobalToLocal(center));
		return trueRect;
	}

	public GObject getConfirmButton(){
		return _confirm;
	}
}

