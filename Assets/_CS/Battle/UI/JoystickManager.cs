﻿using UnityEngine;
using System.Collections;
using FairyGUI;

public class JoystickManager : EventDispatcher
{


	float _InitX;
	float _InitY;
	float _startStageX;
	float _startStageY;
	float _lastStageX;
	float _lastStageY;
	GButton _joystick_button;
	GObject _touchArea;
	GObject _thumb;
	GObject _center;
	int touchId;
	GTweener _tweener;

	public EventListener onMove { get; private set; }

	public EventListener onEnd { get; private set; }

	public int radius { get; set; }

	public JoystickManager (GComponent mainView)
	{
		onMove = new EventListener (this, "onMove");
		onEnd = new EventListener (this, "onEnd");

		_joystick_button = mainView.GetChild ("joystick").asButton;
		_joystick_button.changeStateOnClick = false;
		_thumb = _joystick_button.GetChild ("thumb");
		_touchArea = mainView.GetChild ("joystick_touch");
		_center = mainView.GetChild ("joystick_center");

		_InitX = _center.x + _center.width / 2;
		_InitY = _center.y + _center.height / 2;
		touchId = -1;
		radius = 100;

		_touchArea.onTouchBegin.Add (this.OnTouchBegin);
		_touchArea.onTouchMove.Add (this.OnTouchMove);
		_touchArea.onTouchEnd.Add (this.OnTouchEnd);
	}
		

	public void Trigger(EventContext context)
	{
		OnTouchBegin(context);
	}

	private void OnTouchBegin(EventContext context)
	{
		if (touchId == -1)//First touch
		{
			InputEvent evt = (InputEvent)context.data;
			touchId = evt.touchId;

			if (_tweener != null)
			{
				_tweener.Kill();
				_tweener = null;
			}
			Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));
			float bx = pt.x;
			float by = pt.y;

			if (bx < 0)
				bx = 0;
			else if (bx > _touchArea.width)
				bx = _touchArea.width;

			if (by > GRoot.inst.height)
				by = GRoot.inst.height;
			else if (by < _touchArea.y)
				by = _touchArea.y;

			_lastStageX = bx;
			_lastStageY = by;
			_startStageX = _InitX;
			_startStageY = _InitY;

			_center.visible = true;
			//_center.SetXY(bx - _center.width / 2, by - _center.height / 2);
			_joystick_button.SetXY(bx - _joystick_button.width / 2, by - _joystick_button.height / 2);

			float deltaX = bx - _InitX;
			float deltaY = by - _InitY;
			float degrees = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
			_thumb.rotation = degrees + 90;
			_thumb.visible = true;
			_joystick_button.visible = true;
			context.CaptureTouch();

			handleInput (bx,by);
		}
	}

	private void OnTouchEnd(EventContext context)
	{
		InputEvent inputEvt = (InputEvent)context.data;
		if (touchId != -1 && inputEvt.touchId == touchId)
		{
			touchId = -1;
			_thumb.rotation = _thumb.rotation + 180;
			//_center.visible = false;
			_tweener = _joystick_button.TweenMove(new Vector2(_InitX - _joystick_button.width / 2, _InitY - _joystick_button.height / 2), 0.3f).OnComplete(() =>
				{
					_tweener = null;
					_thumb.visible = true;
					_thumb.rotation = 0;
					//_center.visible = true;
					_joystick_button.visible = false;
					_center.SetXY(_InitX - _center.width / 2, _InitY - _center.height / 2);
				}
			);

			this.onEnd.Call();
		}
	}

	private void OnTouchMove(EventContext context)
	{
		InputEvent evt = (InputEvent)context.data;
		if (touchId != -1 && evt.touchId == touchId)
		{
			Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));

			float bx = pt.x;
			float by = pt.y;

			handleInput (bx, by);


		}
	}


	private void handleInput(float bx, float by){
		if (new Vector2 (bx - _InitX, by - _InitY).magnitude > radius) {

			float offsetX = bx - _startStageX;
			float offsetY = by - _startStageY;
			float rad = Mathf.Atan2(offsetY, offsetX);
			float degree = rad * 180 / Mathf.PI;



			float maxX = radius * Mathf.Cos(rad);
			float maxY = radius * Mathf.Sin(rad);
			if (Mathf.Abs(offsetX) > Mathf.Abs(maxX))
				offsetX = maxX;
			if (Mathf.Abs(offsetY) > Mathf.Abs(maxY))
				offsetY = maxY;

			float buttonX = _startStageX + offsetX;
			float buttonY = _startStageY + offsetY;
			if (buttonX < 0)
				buttonX = 0;
			if (buttonY > GRoot.inst.height)
				buttonY = GRoot.inst.height;

			_joystick_button.SetXY(buttonX - _joystick_button.width / 2, buttonY - _joystick_button.height / 2);


			this.onMove.Call (new float[]{ degree, offsetX * offsetX + offsetY * offsetY });
		} else {
			//float moveX = bx - _lastStageX;
			//float moveY = by - _lastStageY;
			_lastStageX = bx;
			_lastStageY = by;
			//float buttonX = _joystick_button.x + moveX;
			//float buttonY = _joystick_button.y + moveY;
			float buttonX = bx - _joystick_button.width / 2;
			float buttonY = by - _joystick_button.height / 2;
			float offsetX = buttonX + _joystick_button.width / 2 - _startStageX;
			float offsetY = buttonY + _joystick_button.height / 2 - _startStageY;

			float rad = Mathf.Atan2(offsetY, offsetX);
			float degree = rad * 180 / Mathf.PI;


			//_thumb.rotation = degree + 90;

			float maxX = radius * Mathf.Cos(rad);
			float maxY = radius * Mathf.Sin(rad);
			if (Mathf.Abs(offsetX) > Mathf.Abs(maxX))
				offsetX = maxX;
			if (Mathf.Abs(offsetY) > Mathf.Abs(maxY))
				offsetY = maxY;

			buttonX = _startStageX + offsetX;
			buttonY = _startStageY + offsetY;
			if (buttonX < 0)
				buttonX = 0;
			if (buttonY > GRoot.inst.height)
				buttonY = GRoot.inst.height;

			_joystick_button.SetXY(buttonX - _joystick_button.width / 2, buttonY - _joystick_button.height / 2);

			this.onMove.Call(new float[]{degree,offsetX*offsetX+offsetY*offsetY});
		}
	}
}



