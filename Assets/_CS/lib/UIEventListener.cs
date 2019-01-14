using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIEventListener : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler {

	// 定义事件代理
	public delegate void UIEventProxy(GameObject gb,PointerEventData eventData);

	// 鼠标点击事件
	public event UIEventProxy ClickEvent;

	//拖动
	public event UIEventProxy BeginDragEvent;

	public event UIEventProxy OnDragEvent;

	public event UIEventProxy EndDragEvent;

	public bool isDragging = false;

	public void OnPointerClick(PointerEventData eventData){
		if (interval > 0)
			return;
		if (isDragging)
			return;
		if (ClickEvent != null)
			ClickEvent (this.gameObject,eventData);
	}

	public void OnBeginDrag(PointerEventData eventData){
		if (interval > 0)
			return;
		isDragging = true;
		if (BeginDragEvent != null)
			BeginDragEvent (this.gameObject,eventData);
	}

	public void OnDrag(PointerEventData eventData){
		if (interval > 0)
			return;
		if (OnDragEvent != null)
			OnDragEvent (this.gameObject,eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
		if (interval > 0)
			return;
		if (EndDragEvent != null) {
			EndDragEvent (this.gameObject, eventData);
		}
	}

	float interval = 0.3f;
	void Update(){
		if (interval > 0) {
			interval -= Time.deltaTime;
		}
	}

}



