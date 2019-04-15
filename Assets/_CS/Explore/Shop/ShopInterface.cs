using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class ShopInterface : Window
{

	GLoader close;

	GList _components;
	GList _potions;

	GGraph _remove_scar;


	int componentChoice = -1;

	List<TowerComponent> componentsToSell = new List<TowerComponent>(); 

	protected override void OnInit()
	{


		this.contentPane = UIPackage.CreateObject ("UIMain", "ShopInterface").asCom;
		this.Center ();
		this.modal = true;


		_components = this.contentPane.GetChild ("components").asList;
		_potions = this.contentPane.GetChild ("potions").asList;

		_remove_scar = this.contentPane.GetChild ("remove_scar").asGraph;
		_remove_scar.onClick.Add (delegate() {
				
		});
		close = this.contentPane.GetChild("close").asLoader;
		close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});
	}


	public void initShop(){
		componentChoice = -1;
		componentsToSell.Clear ();

		componentsToSell.Add (GameStaticData.getInstance().getComponentInfo("c02"));
		componentsToSell.Add (GameStaticData.getInstance().getComponentInfo("c01"));
		componentsToSell.Add (GameStaticData.getInstance().getComponentInfo("c03"));
	}

	protected override void OnShown(){
	
		_components.RemoveChildrenToPool ();
		//_components.numItems = 3;
		for (int i = 0; i < componentsToSell.Count; i++) {
			AccesoryView obj = (AccesoryView)_components.AddItemFromPool ();
			obj.updateView (componentsToSell[i]);
			int idx = i;
			obj.onClick.Set (delegate() {
				_components.ClearSelection();
				obj.selected = true;
				componentChoice = idx;
				changeDetailView();
			});
		}
	}

	private void changeDetailView(){
		Debug.Log (componentChoice);
	}
}

