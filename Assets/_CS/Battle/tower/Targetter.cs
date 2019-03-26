using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Targetter : MonoBehaviour {

//	public MapObject owner;
//	float searchRate = 0.25f;
//	float m_SearchTimer = 0;
////
//	private GameLife m_CurrrentTargetable;
//	private bool m_HadTarget;
////
//	public event Action<GameLife> onTargetChosen;
//	public event Action lostTarget;
////
////
////	// Use this for initialization
//	void Start () {
//		m_SearchTimer = searchRate;
//		owner = GetComponent<MapObject> ();
//	}
//
//	public void ResetTargetter()
//	{
//		m_CurrrentTargetable = null;
//	}
//
//	public void Tick (int timeInt) {
//
//		m_SearchTimer -= Time.deltaTime;
//
//		if (m_CurrrentTargetable != null) {
//			if (!m_CurrrentTargetable.IsAlive) {
//				m_CurrrentTargetable = null;
//			}
//		}
//
//		if (m_SearchTimer <= 0.0f && m_CurrrentTargetable == null )
//		{
//			m_CurrrentTargetable = MapManager.getInstance().getClosestEnemy (owner);
//			if (m_CurrrentTargetable != null)
//			{
//				if (onTargetChosen != null)
//				{
//					onTargetChosen(m_CurrrentTargetable);
//				}
//				m_SearchTimer = searchRate;
//			}
//		}
//
//		m_HadTarget = m_CurrrentTargetable != null;
//	}
//
//
//	GameLife getNearestValidTarget(int rangeInt){
//		GameLife possibleTarget = MapManager.getInstance().getClosestEnemy (owner);
//		if (possibleTarget != null ) {
//			Vector3 Diff2d = (transform.position - possibleTarget.transform.position);
//			Diff2d.z = 0;
//			int dis = (int)(Diff2d.magnitude * 1000);
//			if (dis > rangeInt) {
//				possibleTarget = null;
//			}
//		}
//		return possibleTarget;
//	}
//



//	void OnTargetRemoved(Targetable target)
//	{
//		//target.removed -= OnTargetRemoved;
//		if (m_CurrrentTargetable != null && target.configuration == m_CurrrentTargetable.configuration)
//		{
//			if (lostTarget != null)
//			{
//				lostTarget();
//			}
//			m_HadTarget = false;
//			m_TargetsInRange.Remove(m_CurrrentTargetable);
//			m_CurrrentTargetable = null;
//		}
//		else //wasnt the current target, find and remove from targets list
//		{
//			for (int i = 0; i < m_TargetsInRange.Count; i++)
//			{
//				if (m_TargetsInRange[i].configuration == target.configuration)
//				{
//					m_TargetsInRange.RemoveAt(i);
//					break;
//				}
//			}
//		}
//	}



}
