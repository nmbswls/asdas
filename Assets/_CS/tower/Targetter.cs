using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Targetter : MonoBehaviour {

//	float searchRate = 0.25f;
//
//	float m_SearchTimer = 0;
//
//	private Targetable m_CurrrentTargetable;
//	private List<Targetable> m_TargetsInRange;
//	private bool m_HadTarget;
//
//	public event Action<Targetable> acquiredTarget;
//	public event Action lostTarget;
//
//
//	// Use this for initialization
//	void Start () {
//		m_SearchTimer = searchRate;
//	}
//	public void ResetTargetter()
//	{
//		m_TargetsInRange.Clear();
//		m_CurrrentTargetable = null;
//
//
//		acquiredTarget = null;
//		lostTarget = null;
//	}
//	// Update is called once per frame
//	void Update () {
//
//		m_SearchTimer -= Time.deltaTime;
//
//		if (m_SearchTimer <= 0.0f && m_CurrrentTargetable == null && m_TargetsInRange.Count > 0)
//		{
//			m_CurrrentTargetable = GetNearestTargetable();
//			if (m_CurrrentTargetable != null)
//			{
//				if (acquiredTarget != null)
//				{
//					acquiredTarget(m_CurrrentTargetable);
//				}
//				m_SearchTimer = searchRate;
//			}
//		}
//
//		//AimTurret();
//
//		m_HadTarget = m_CurrrentTargetable != null;
//	}
//
//	void GetNearestTargetable(){
//		int length = m_TargetsInRange.Count;
//
//		if (length == 0)
//		{
//			return null;
//		}
//
//		Targetable nearest = null;
//		float distance = float.MaxValue;
//		for (int i = length - 1; i >= 0; i--)
//		{
//			Targetable targetable = m_TargetsInRange[i];
//			if (targetable == null || !targetable.owner.IsAlive)
//			{
//				m_TargetsInRange.RemoveAt(i);
//				continue;
//			}
//			float currentDistance = Vector3.Distance(transform.position, targetable.position);
//			if (currentDistance < distance)
//			{
//				distance = currentDistance;
//				nearest = targetable;
//			}
//		}
//
//		return nearest;
//	}
//
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
