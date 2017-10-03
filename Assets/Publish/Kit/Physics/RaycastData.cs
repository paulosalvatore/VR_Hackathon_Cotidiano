using System;
using UnityEngine;
using Kit.Extend;

namespace Kit.Physic
{
	public struct RaycastData : IEquatable<IRaycastStruct>, IRaycastStruct
	{
		public static RaycastData NONE { get { return default(RaycastData); } }

		#region Core & constructor
		private Ray m_Ray;
		public Vector3 origin { get { return m_Ray.origin; } set { m_Ray.origin = value; } }
		public Vector3 direction { get { return m_Ray.direction; } set { m_Ray.direction = value; } }
		public Quaternion rotation
		{
			get { return Quaternion.Euler(m_Ray.direction); }
			set { m_Ray.direction = value * Vector3.forward; }
		}
		public float distance { get; set; }
		private RaycastHit hit;
		public RaycastHit hitResult { get { return hit; } set { hit = value; } }
		public bool hitted { get { return hit.transform != null; } }
		

		public RaycastData(Ray ray, float distance)
		{
			m_Ray = ray;
			this.distance = distance;
			hit = new RaycastHit();
		}

		public RaycastData(Vector3 origin, Vector3 direction, float distance)
			: this(new Ray(origin, direction), distance)
		{ }
		#endregion

		#region Util tools
		public static bool IsNullOrEmpty(RaycastData obj)
		{
			return ReferenceEquals(null, obj) || obj.Equals(RaycastData.NONE);
		}

		/// <summary>Depend on RaycastHit result, to locate the suspend point.</summary>
		/// <returns></returns>
		public Vector3 GetRayEndPoint()
		{
			return ((hitted) ? hitResult.distance : distance) * direction + origin;
		}

		/// <summary>Physics.Raycast</summary>
		/// <param name="layerMask">default = Everything</param>
		/// <param name="queryTriggerInteraction">default = UseGlobal</param>
		/// <returns>return true when hitted.</returns>
		public bool Raycast(int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.Raycast(m_Ray, out hit, distance, layerMask, queryTriggerInteraction);
		}

		public int RaycastNonAlloc(RaycastHit[] hits, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.RaycastNonAlloc(m_Ray, hits, distance, layerMask, queryTriggerInteraction);
		}

		/// <summary>Compare with hit result are hitting same object</summary>
		/// <param name="raycastHit"></param>
		/// <param name="includeNull"></param>
		/// <returns></returns>
		public bool IsHittingSameObject(RaycastHit raycastHit, bool includeNull = false)
		{
			return (includeNull || hitted) ? hit.transform == raycastHit.transform : false;
		}

		public void DrawGizmos(Color color = default(Color), Color hitColor = default(Color))
		{
			if (color == default(Color))
				return;
			Color cache = Gizmos.color;
			if (hitted)
			{
				if (hitColor == default(Color))
				{
					hitColor = color;
					color.a = Mathf.Lerp(0f, color.a, .5f);
				}
			}

			Gizmos.color = color;
			Gizmos.DrawLine(m_Ray.origin, origin + direction * distance);
#if UNITY_EDITOR
			Gizmos.DrawWireCube(m_Ray.origin, Vector3.one * 0.1f * UnityEditor.HandleUtility.GetHandleSize(m_Ray.origin));
#endif
			if (hitted)
			{
				Gizmos.color = hitColor;
				Gizmos.DrawLine(m_Ray.origin, GetRayEndPoint());
			}
			Gizmos.color = cache;
		}


		/// <summary>Reuse the RayData for something else instead of alloc new memory</summary>
		/// <param name="_origin"></param>
		/// <param name="_rotation"></param>
		/// <param name="_distance"></param>
		/// <param name="reset"></param>
		public void Update(Vector3 _origin, Quaternion _rotation, float _distance)
		{
			origin = _origin;
			rotation = _rotation;
			distance = _distance;
		}

		/// <summary>Reuse the RayData for something else instead of alloc new memory</summary>
		/// <param name="_origin"></param>
		/// <param name="_direction"></param>
		/// <param name="_distance"></param>
		/// <param name="reset"></param>
		public void Update(Vector3 _origin, Vector3 _direction, float _distance)
		{
			origin = _origin;
			direction = _direction;
			distance = _distance;
		}

		/// <summary>Reuse the RayData for something else instead of alloc new memory</summary>
		/// <param name="other"></param>
		/// <param name="hitReferences">include the RaycastHit reference.</param>
		public void Update(IRaycastStruct other, bool hitReferences = false)
		{
			Update(other.origin, other.direction, other.distance);
			if (hitReferences)
				hit = other.hitResult;
		}

		public void Reset()
		{
			origin = direction = Vector3.zero;
			distance = 0f;
			hit = new RaycastHit();
		}
		#endregion

		#region overload methods
		/// <summary>Approximately Equal, based on Ray and distance, exclude hit result.</summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(IRaycastStruct other)
		{
			return
				Mathf.Approximately(distance, other.distance) &&
				m_Ray.origin.EqualSimilar(other.origin) &&
				m_Ray.direction.EqualSimilar(other.direction);
		}

		public override bool Equals(object obj)
		{
			return Equals(NONE) ? obj == null : Equals((IRaycastStruct)obj);
		}

		public override string ToString()
		{
			return (hitted) ?
				string.Format("{0}, Distance: {1:F2}, Hit: {2} ({3:F2})", m_Ray.ToString("F2"), distance, hit.transform.name, hit.point) :
				string.Format("{0}, Distance: {1:F2}, Hit: None", m_Ray.ToString("F2"), distance);
		}

		public override int GetHashCode()
		{
			return m_Ray.GetHashCode();
		}
		#endregion
	}

	public interface IRaycastStruct
	{
		bool Raycast(int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
		int RaycastNonAlloc(RaycastHit[] hits, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
		void DrawGizmos(Color color = default(Color), Color hitColor = default(Color));


		#region should redirect to RayData
		Vector3 origin { get; set; }
		Vector3 direction { get; set; }
		Quaternion rotation { get; set; }
		float distance { get; set; }
		RaycastHit hitResult { get; }
		bool hitted { get; }

		bool IsHittingSameObject(RaycastHit raycastHit, bool includeNull = false);
		Vector3 GetRayEndPoint();
		void Update(Vector3 _origin, Quaternion _rotation, float _distance);
		void Update(Vector3 _origin, Vector3 _direction, float _distance);
		void Update(IRaycastStruct other, bool hitReferences = false);
		void Reset();
		string ToString();
		int GetHashCode();
		bool Equals(object obj);
		bool Equals(IRaycastStruct other);
		#endregion
	}
}