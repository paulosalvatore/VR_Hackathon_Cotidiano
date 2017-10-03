using System;
using UnityEngine;

namespace Kit.Physic
{
	public struct SpherecastData : IEquatable<IRaycastStruct>, IRaycastStruct
	{
		public float m_Radius;

		public static SpherecastData NONE { get { return default(SpherecastData); } }

		public static bool IsNullOrEmpty(SpherecastData obj)
		{
			return ReferenceEquals(null, obj) || obj.Equals(SpherecastData.NONE);
		}

		public SpherecastData(Ray ray, float distance, float radius)
		{
			m_RayBase = new RaycastData(ray, distance);
			m_Radius = radius;
		}

		public SpherecastData(Vector3 origin, Vector3 direction, float distance, float radius)
			: this(new Ray(origin, direction), distance, radius)
		{ }

		public bool Raycast(int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			RaycastHit hit;
			bool rst = Physics.SphereCast(origin, m_Radius, direction, out hit, distance, layerMask, queryTriggerInteraction);
			m_RayBase.hitResult = hit;
			return rst;
		}
		public int RaycastNonAlloc(RaycastHit[] hits, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.SphereCastNonAlloc(origin, m_Radius, direction, hits, distance, layerMask, queryTriggerInteraction);
		}

		public void DrawGizmos(Color color = default(Color), Color hitColor = default(Color))
		{
			m_RayBase.DrawGizmos(color, hitColor);
			Color cache = Gizmos.color;
			Gizmos.color = (hitted)? hitColor : color;
			Gizmos.DrawWireSphere(m_RayBase.GetRayEndPoint(), m_Radius);
			Gizmos.color = cache;
		}
		
		#region Overload
		/// <summary>Overlap will not use the distance information.</summary>
		/// <param name="color"></param>
		/// <param name="hitColor"></param>
		public void DrawOverlapGizmos(ref Collider[] colliderResult, int validArraySize, Color color = default(Color), Color hitColor = default(Color))
		{
			Color cache = Gizmos.color;
			Gizmos.color = color;
			Gizmos.DrawWireSphere(origin, m_Radius);
			Gizmos.color = hitColor;
			for (int i = 0; i < validArraySize && i < colliderResult.Length; ++i)
			{
				Gizmos.DrawLine(origin, colliderResult[i].transform.position);
			}
			Gizmos.color = cache;
		}

		public int Overlap(ref Collider[] results, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.OverlapSphereNonAlloc(origin, m_Radius, results, layerMask, queryTriggerInteraction);
		}

		public void Update(Vector3 _origin, Quaternion _rotation, float _distance, float _radius)
		{
			m_RayBase.Update(_origin, _rotation, _distance);
			m_Radius = _radius;
		}
		#endregion

		#region redirect session
		private RaycastData m_RayBase;
		public Vector3 origin { get { return m_RayBase.origin; } set { m_RayBase.origin = value; } }
		public Vector3 direction { get { return m_RayBase.direction; } set { m_RayBase.direction = value; } }
		public Quaternion rotation { get { return m_RayBase.rotation; } set { m_RayBase.rotation = value; } }
		public float distance { get { return m_RayBase.distance; } set { m_RayBase.distance = value; } }
		public RaycastHit hitResult { get { return m_RayBase.hitResult; } }
		public bool hitted { get { return m_RayBase.hitted; } }
		
		public bool IsHittingSameObject(RaycastHit raycastHit, bool includeNull = false)
		{
			return m_RayBase.IsHittingSameObject(raycastHit, includeNull);
		}

		public void Update(Vector3 _origin, Quaternion _rotation, float _distance)
		{
			m_RayBase.Update(_origin, _rotation, _distance);
		}

		public void Update(Vector3 _origin, Vector3 _direction, float _distance)
		{
			m_RayBase.Update(_origin, _direction, _distance);
		}

		public void Update(IRaycastStruct other, bool hitReferences = false)
		{
			if (other is SpherecastData)
				m_Radius = ((SpherecastData)other).m_Radius;
			m_RayBase.Update(other, hitReferences);
		}

		public void Reset()
		{
			m_RayBase.Reset();
			m_Radius = 0f;
		}

		public bool Equals(IRaycastStruct other)
		{
			return m_RayBase.Equals(other);
		}

		public override bool Equals(object obj)
		{
			return m_RayBase.Equals(obj);
		}

		public override string ToString()
		{
			return m_RayBase.ToString();
		}

		public override int GetHashCode()
		{
			return m_RayBase.GetHashCode();
		}

		public Vector3 GetRayEndPoint()
		{
			return m_RayBase.GetRayEndPoint();
		}
		#endregion
	}
}