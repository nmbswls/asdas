using UnityEngine;
using System.Collections;
using FairyGUI;

public class BuildWindow : Window
{

	GList _list;

	BuildDetail _detail;
	GComponent _confirmButton;
	GLoader _close;

	public BuildWindow()
	{
	}

	protected override void OnShown(){
		float interval = 300/(_list.numChildren>8?8:_list.numChildren);
		if (_list.numItems > 0) {
			_list.selectedIndex = 0;
			_detail.visible = true;
			_detail.updateView (((BuildItem)_list.GetChildAt(0)).getTTInfo());
			//_detail.GetChild ("name").asTextField.text = ((BuildItem)(_list.GetChildAt(0))).Name;
		}
		_list.columnGap = (int)interval;
		BattleManager.getInstance ().pause ();
	}

	protected override void OnHide(){
		BattleManager.getInstance ().unPause ();
	}

	public void buildConfirm(){
		int nowSelect = _list.selectedIndex;

		if (nowSelect >= 0) {
			bool res = BattleManager.getInstance ().player.tryBuildTower (nowSelect);
			if(res)this.Hide ();
		}
	}

	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "BuildWindow").asCom;
		this.Center();
		this.modal = true;

		_list = this.contentPane.GetChild("list").asList;
		_close = this.contentPane.GetChild("close").asLoader;
		_close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});
		_list.onClickItem.Add(__clickItem);
		//_list.itemRenderer = RenderListItem;
		_list.EnsureBoundsCorrect();
		_detail = (BuildDetail)this.contentPane.GetChild ("detail").asCom;
		_detail.visible = false;

		_confirmButton = this.contentPane.GetChild ("confirm").asCom;
		_confirmButton.onTouchBegin.Add (buildConfirm);


		foreach (TowerTemplate tt in BattleManager.getInstance ().buildableTowers)
		{
			BuildItem item = (BuildItem)_list.AddItemFromPool("ui://UIMain/BuildItem");
			item.setTowerInfo (tt);

		}
		float interval = 300/(_list.numChildren>8?8:_list.numChildren);
		_list.columnGap = (int)interval;

		//_list.numItems = 3;
	}

	void RenderListItem(int index, GObject obj)
	{
		BuildItem bi = (BuildItem)obj;

		//bi.position - _list.width;
		bi.icon = "image/TowerBigPicture/"+BattleManager.getInstance ().buildableTowers[index].tbase.tid;

	}

//	override protected void DoShowAnimation()
//	{
//		this.SetScale(0.1f, 0.1f);
//		this.SetPivot(0.5f, 0.5f);
//		this.TweenScale(new Vector2(1, 1), 0.3f).OnComplete(this.OnShown);
//	}

//	override protected void DoHideAnimation()
//	{
//		this.TweenScale(new Vector2(0.1f, 0.1f), 0.3f).OnComplete(this.HideImmediately);
//	}

	void __clickItem(EventContext context)
	{
		BuildItem item = (BuildItem)context.data;
		_detail.visible = true;
		_detail.updateView (item.getTTInfo());
		//this.contentPane.GetChild("n11").asLoader.url = item.icon;
		//this.contentPane.GetChild("n13").text = item.icon;
	}
}

