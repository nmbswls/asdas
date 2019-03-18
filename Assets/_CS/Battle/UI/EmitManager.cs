using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class EmitManager
{
	public static int MAX_POOL_NUMBER = 50;
	static EmitManager _instance;
	public static EmitManager inst
	{
		get
		{
			if (_instance == null)
				_instance = new EmitManager();
			return _instance;
		}
	}

	public string hurtFont1;
	public string hurtFont2;
	public string criticalSign;

	public GComponent view { get; private set; }

	private readonly Stack<EmitComponent> _componentPool = new Stack<EmitComponent>();

	public EmitManager()
	{
		hurtFont1 = "ui://EmitNumbers/number1";
		hurtFont2 = "ui://EmitNumbers/number2";
		criticalSign = "ui://EmitNumbers/critical";

		view = new GComponent();
		GRoot.inst.AddChild(view);
	}

	public void Emit(Transform owner, int type, long hurt, bool isPlayer = false, bool critical=false)
	{
		EmitComponent ec;
		if (_componentPool.Count > 0)
			ec = _componentPool.Pop();
		else
			ec = new EmitComponent();
		ec.SetHurt(owner, type, hurt, isPlayer,critical);
	}



	public void ReturnComponent(EmitComponent com)
	{
		if (_componentPool.Count < MAX_POOL_NUMBER) {
			_componentPool.Push (com);
		}
	}
}

