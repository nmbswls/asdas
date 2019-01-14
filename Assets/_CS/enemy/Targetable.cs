using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour {


//	public GameLife owner;
//
//
//
//	public Transform targetTransform;
//
//	protected Vector3 m_CurrentPosition, m_PreviousPosition;
//
//	public virtual Vector3 velocity { get; protected set; }
//
//	public Transform targetableTransform
//	{
//		get
//		{
//			return targetTransform == null ? transform : targetTransform;
//		}
//	}
//
//	public override Vector3 position
//	{
//		get { return targetableTransform.position; }
//	}
//
//	/// <summary>
//	/// Initialises any DamageableBehaviour logic
//	/// </summary>
//	protected override void Awake()
//	{
//		ResetPositionData();
//		owner = GetComponentInParent<GameLife> ();
//	}
//
//	/// <summary>
//	/// Sets up the position data so velocity can be calculated
//	/// </summary>
//	protected void ResetPositionData()
//	{
//		m_CurrentPosition = position;
//		m_PreviousPosition = position;
//	}
//
//	/// <summary>
//	/// Calculates the velocity and updates the position
//	/// </summary>
//	void FixedUpdate()
//	{
//		m_CurrentPosition = position;
//		velocity = (m_CurrentPosition - m_PreviousPosition) / Time.fixedDeltaTime;
//		m_PreviousPosition = m_CurrentPosition;
//	}
}
