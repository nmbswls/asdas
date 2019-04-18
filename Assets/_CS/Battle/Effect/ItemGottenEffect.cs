using UnityEngine;
using System.Collections;

public class ItemGottenEffect : BaseEffect
{
	private Vector3 screenPos;
	public static Vector3 targetPos = new Vector3(1100,100,0);



	protected override void effectAction(int timeInt){
		Vector2 diff = (targetPos - screenPos);
		if (diff.magnitude < 30f) {
			BattleManager.getInstance().mainUIManager.activeTranstion ();
			Release ();
		} else {
			screenPos = Vector3.Lerp (screenPos,targetPos,0.1f);
			transform.position = Camera.main.ScreenToWorldPoint (screenPos);
			transform.position += new Vector3 (0,0,10);
		}
	}



	public void init(Vector3 worldPos){
		this.screenPos = Camera.main.WorldToScreenPoint (worldPos);;
		transform.position = worldPos;
		initialized = true;
		GetComponent<TrailRenderer> ().Clear ();
		targetPos.x = MainUIManager.BuildButtonPosInScreen.x + 20f;
		targetPos.y = Screen.height - MainUIManager.BuildButtonPosInScreen.y - 20f;
		gameObject.SetActive (true);
	}
		
	protected override void OnRelease(){
		screenPos = Vector3.zero;
		GetComponent<TrailRenderer> ().Clear ();
		GameObject.Destroy (gameObject);
	}

}

