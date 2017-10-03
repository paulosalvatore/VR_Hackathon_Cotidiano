using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kit.Physic
{
	/// <summary>A helper class for visualize common Phyical raycast and overlap.
	/// also provide the Gizmos call for debug usage.</summary>
	public class RaycastHelper : MonoBehaviour
	{
		#region setting
		public enum eRayType
		{
			Raycast = 0,
			SphereCast = 10,
			SphereOverlap,
			BoxCast = 20,
			BoxOverlap,
		}

		RaycastData m_RayData;
		SpherecastData m_SphereRayData;
		BoxcastData m_BoxcastData;

		public eRayType m_RayType = eRayType.Raycast;
		public float m_Distance = 1f;

		// Sphere
		public float m_Radius = 1f;

		// Boxcast
		public Vector3 m_HalfExtends = Vector3.one;
		public bool m_SyncRotation = false;
		public Vector3 m_LocalRotation = Vector3.zero;

		[Header("Physics")]
		public bool m_FixedUpdate = true;
		public LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		public QueryTriggerInteraction m_QueryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
		
		// Overlap
		Collider[] m_OverlapColliders;
		[SerializeField] int m_MemoryArraySize = 0;
		int m_OverlapHittedCount = 0;
		
		[Header("Debug")]
		[SerializeField] Color m_Color = Color.white;
		[SerializeField] Color m_HitColor = Color.red;

		[Header("Events")]
		public HitEvent OnHitEnter;
		public HitEvent OnHit;
		public UnityEvent OnHitLeave;
		Transform m_LastHitObject = null;
		[System.Serializable] public class HitEvent : UnityEvent<Transform> { }
		#endregion

		#region System
		void OnValidate()
		{
			m_Distance = Mathf.Clamp(m_Distance, 0f, float.MaxValue);
			m_Radius = Mathf.Clamp(m_Radius, 0f, float.MaxValue);
			m_MemoryArraySize = Mathf.Max(m_MemoryArraySize, 0);
			m_HalfExtends.x = Mathf.Clamp(m_HalfExtends.x, 0f, float.MaxValue);
			m_HalfExtends.y = Mathf.Clamp(m_HalfExtends.y, 0f, float.MaxValue);
			m_HalfExtends.z = Mathf.Clamp(m_HalfExtends.z, 0f, float.MaxValue);
		}

		void Awake()
		{
			Init();
		}

		void Init()
		{
			if (m_RayType != eRayType.Raycast && !RaycastData.IsNullOrEmpty(m_RayData))
				m_RayData.Reset();
			if (!(m_RayType == eRayType.SphereCast || m_RayType == eRayType.SphereOverlap) && !SpherecastData.IsNullOrEmpty(m_SphereRayData))
				m_SphereRayData.Reset();
			if (!(m_RayType == eRayType.BoxCast || m_RayType == eRayType.BoxOverlap) && !BoxcastData.IsNullOrEmpty(m_BoxcastData))
				m_BoxcastData.Reset();
			if (m_RayType == eRayType.SphereOverlap || m_RayType == eRayType.BoxOverlap)
				m_OverlapColliders = new Collider[m_MemoryArraySize];

			CheckPhysic();
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				Init();

			switch (m_RayType)
			{
				case eRayType.Raycast:
				case eRayType.SphereCast:
				case eRayType.BoxCast:
					GetRayData().DrawGizmos(m_Color, m_HitColor);
					break;

				case eRayType.SphereOverlap:
					m_SphereRayData.DrawOverlapGizmos(ref m_OverlapColliders, m_OverlapHittedCount, m_Color, m_HitColor);
					break;
				case eRayType.BoxOverlap:
					m_BoxcastData.DrawOverlapGizmos(ref m_OverlapColliders, m_OverlapHittedCount, m_Color, m_HitColor);
					break;
			}
		}

		void FixedUpdate()
		{
			if (!m_FixedUpdate)
				return;
			
			bool hitRst = CheckPhysic();

			if (!Application.isPlaying)
				return;

			IRaycastStruct obj = GetRayData();
			if (hitRst)
			{
				if (m_LastHitObject != obj.hitResult.transform || m_LastHitObject == null)
				{
					OnHitEnter.Invoke(obj.hitResult.transform);
					m_LastHitObject = obj.hitResult.transform;
				}
			}
			else if (!hitRst && m_LastHitObject != null)
			{
				m_LastHitObject = null;
				OnHitLeave.Invoke();
			}
		}
		#endregion

		#region API
		[ContextMenu("Check Physic")]
		/// <summary>Check physic, hit anything.</summary>
		/// <returns>true = hit something.</returns>
		public bool CheckPhysic()
		{
			bool hitRst = false;
				
			if (m_RayType == eRayType.Raycast)
			{
				m_RayData.Update(transform.position, transform.rotation, m_Distance);
				hitRst = m_RayData.Raycast(m_LayerMask, m_QueryTriggerInteraction);
				OnHit.Invoke(m_RayData.hitResult.transform);
			}
			else if (m_RayType == eRayType.SphereCast)
			{
				m_SphereRayData.Update(transform.position, transform.rotation, m_Distance, m_Radius);
				hitRst = m_SphereRayData.Raycast(m_LayerMask, m_QueryTriggerInteraction);
				OnHit.Invoke(m_SphereRayData.hitResult.transform);
			}
			else if (m_RayType == eRayType.SphereOverlap)
			{
				m_SphereRayData.Update(transform.position, transform.rotation, m_Distance, m_Radius);
				m_OverlapHittedCount = m_SphereRayData.Overlap(ref m_OverlapColliders, m_LayerMask, m_QueryTriggerInteraction);
				hitRst = m_OverlapHittedCount > 0;
			}
			else if (m_RayType == eRayType.BoxCast)
			{
				m_BoxcastData.Update(transform.position, transform.rotation, m_Distance, m_HalfExtends, (m_SyncRotation) ? transform.rotation : Quaternion.Euler(m_LocalRotation));
				hitRst = m_BoxcastData.Raycast(m_LayerMask, m_QueryTriggerInteraction);
				OnHit.Invoke(m_BoxcastData.hitResult.transform);
			}
			else if (m_RayType == eRayType.BoxOverlap)
			{
				m_BoxcastData.Update(transform.position, transform.rotation, m_Distance, m_HalfExtends, (m_SyncRotation) ? transform.rotation : Quaternion.Euler(m_LocalRotation));
				m_OverlapHittedCount = m_BoxcastData.Overlap(ref m_OverlapColliders, m_LayerMask, m_QueryTriggerInteraction);
				hitRst = m_OverlapHittedCount > 0;
			}
			else
			{
				throw new System.NotImplementedException();
			}

			if (hitRst && (m_RayType == eRayType.SphereOverlap || m_RayType == eRayType.BoxOverlap))
			{
				foreach (Collider collider in GetOverlapColliders())
				{
					OnHit.Invoke(collider.transform);
				}
			}

			return hitRst;
		}

		public void OnHitDebug(Transform obj)
		{
			Debug.DrawLine(GetRayData().origin, obj.position, m_HitColor, .3f);
		}

		/// <summary>Get colliders which are overlap the preset area. only work on Overlap type.</summary>
		/// <returns></returns>
		public IEnumerable<Collider> GetOverlapColliders()
		{
			if (m_MemoryArraySize > 0 || m_RayType == eRayType.BoxOverlap || m_RayType == eRayType.SphereOverlap)
			{
				for (int i = 0; i < m_OverlapHittedCount && i < m_OverlapColliders.Length; ++i)
					yield return m_OverlapColliders[i];
			}
			else
				throw new System.Exception(GetType().Name + " cannot use without alloc memory. set m_RayType to Overlap methods.");
		}

		public IRaycastStruct GetRayData()
		{
			if (m_RayType == eRayType.Raycast)
				return m_RayData;
			else if (m_RayType == eRayType.SphereCast || m_RayType == eRayType.SphereOverlap)
				return m_SphereRayData;
			else if (m_RayType == eRayType.BoxCast || m_RayType == eRayType.BoxOverlap)
				return m_BoxcastData;
			else
				throw new System.NotImplementedException();
		}
		#endregion
	}
}