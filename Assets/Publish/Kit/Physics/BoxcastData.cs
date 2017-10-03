using System;
using UnityEngine;
using Kit.Extend;

namespace Kit.Physic
{
	public struct BoxcastData : IEquatable<IRaycastStruct>, IRaycastStruct
	{
		public Vector3 m_HalfExtends;
		public Quaternion m_LocalRotation;

		public static BoxcastData NONE { get { return default(BoxcastData); } }

		public static bool IsNullOrEmpty(BoxcastData obj)
		{
			return ReferenceEquals(null, obj) || obj.Equals(BoxcastData.NONE);
		}

		public BoxcastData(Ray ray, float distance, Vector3 size, Quaternion localRotation)
		{
			m_RayBase = new RaycastData(ray, distance);
			m_HalfExtends = size;
			m_LocalRotation = localRotation;
		}

		public BoxcastData(Vector3 origin, Vector3 direction, float distance, Vector3 size, Quaternion localRotation)
			: this(new Ray(origin, direction), distance, size, localRotation)
		{ }

		public bool Raycast(int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			RaycastHit hit;
			bool rst = Physics.BoxCast(origin, m_HalfExtends, direction, out hit, m_LocalRotation, distance, layerMask, queryTriggerInteraction);
			m_RayBase.hitResult = hit;
			return rst;
		}

		public int RaycastNonAlloc(RaycastHit[] hits, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.BoxCastNonAlloc(origin, m_HalfExtends, direction, hits, m_LocalRotation, distance, layerMask, queryTriggerInteraction);
		}

		public void DrawGizmos(Color color = default(Color), Color hitColor = default(Color))
		{
			m_RayBase.DrawGizmos(color, hitColor);
			Color cache = Gizmos.color;
			GizmosExtend.DrawBoxCastBox(m_RayBase.origin, m_HalfExtends, m_LocalRotation, m_RayBase.direction, m_RayBase.distance, color);
			if (m_RayBase.hitted)
				GizmosExtend.DrawBoxCastOnHit(m_RayBase.origin, m_HalfExtends, m_LocalRotation, m_RayBase.direction, m_RayBase.hitResult.distance, hitColor);
			Gizmos.color = cache;
		}

		#region Overload
		/// <summary>Overlap will not use the distance information.</summary>
		/// <param name="color"></param>
		/// <param name="hitColor"></param>
		public void DrawOverlapGizmos(ref Collider[] colliderResult, int validArraySize, Color color = default(Color), Color hitColor = default(Color))
		{
			Color cache = Gizmos.color;
			GizmosExtend.DrawBoxCastBox(m_RayBase.origin, m_HalfExtends, m_LocalRotation, m_RayBase.direction, 0f, color);
			Gizmos.color = hitColor;
			for (int i = 0; i < validArraySize && i < colliderResult.Length; ++i)
			{
				Gizmos.DrawLine(origin, colliderResult[i].transform.position);
			}
			Gizmos.color = cache;
		}
		public int Overlap(ref Collider[] results, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.OverlapBoxNonAlloc(origin, m_HalfExtends, results, m_LocalRotation, layerMask, queryTriggerInteraction);
		}
		public void Update(Vector3 _origin, Quaternion _rotation, float _distance, Vector3 _halfExtends, Quaternion _localRotation)
		{
			m_RayBase.Update(_origin, _rotation, _distance);
			m_HalfExtends = _halfExtends;
			m_LocalRotation = _localRotation;
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
			if (other is BoxcastData)
				m_HalfExtends = ((BoxcastData)other).m_HalfExtends;
			m_RayBase.Update(other, hitReferences);
		}

		public void Reset()
		{
			m_RayBase.Reset();
			m_HalfExtends = Vector3.zero;
			m_LocalRotation = Quaternion.identity;
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