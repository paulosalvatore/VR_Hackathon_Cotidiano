//#define BULLET_TIME
// ^ enable this if you want to visualize the progress with delay.

using UnityEngine;
using System.Collections;

namespace Kit.Physic
{
	public class EdgeFinder : MonoBehaviour
	{
		[Header("Ray Setting")]
		[SerializeField]
		int m_Density = 150;
		[SerializeField]
		float m_Distance = 10f;
		[SerializeField]
		float m_Threshold = float.Epsilon;
		const float MAX_THRESHOLD = 1f;

		[Header("Physics")]
		[SerializeField]
		LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
		[SerializeField]
		QueryTriggerInteraction m_QueryTriggerInteraction = QueryTriggerInteraction.UseGlobal;

		[Header("Debug")]
		[SerializeField]
		Color m_Color = Color.white;
		[SerializeField]
		Color m_HitColor = Color.red;
#if BULLET_TIME
		[SerializeField] bool m_PreviewInEditor = false;
		[Range(-float.Epsilon, 0.1f)]
		[SerializeField] float m_WaitSec = 0.1f;
#endif

		RaycastData[] m_Sensors;
		RaycastData m_Finder;

		void OnValidate()
		{
			m_Density = Mathf.Clamp(m_Density, 0, int.MaxValue);
			m_Distance = Mathf.Clamp(m_Distance, 0f, float.MaxValue);
			m_Threshold = Mathf.Clamp(m_Threshold, float.Epsilon, MAX_THRESHOLD);
			Init();
		}

		void Awake()
		{
			Init();
		}

		void Init()
		{
			m_Sensors = new RaycastData[m_Density];
#if BULLET_TIME && UNITY_EDITOR
			if (m_PreviewInEditor)
			{
				StopAllCoroutines();
				Start();
			}
#else
			CacheRayResult(transform.position, transform.forward, transform.up);
#endif
		}

		public void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				Init();

			for (int i = 0; i < m_Sensors.Length; i++)
			{
				if (!RaycastData.IsNullOrEmpty(m_Sensors[i]))
					m_Sensors[i].DrawGizmos(m_Color, m_HitColor);
			}
		}

#if BULLET_TIME
		public void Start()
		{
			StartCoroutine(IntervalTrigger());
		}
		private IEnumerator IntervalTrigger()
		{
			while (true)
			{
				yield return GetBisectionEdge(transform.position, transform.forward, transform.up, m_Threshold);
			}
		}
#else
		void FixedUpdate()
		{
			GetBisectionEdge(transform.position, transform.forward, transform.up, m_Threshold);
		}
#endif

		private void CacheRayResult(Vector3 point, Vector3 startDirection, Vector3 normal, bool raycastTest = false)
		{
			Vector3 dir;
			float currAngle = 0f;
			float angleStep = 360f / m_Density;
			for (int i = 0; i < m_Density; i++)
			{
				dir = Quaternion.AngleAxis(currAngle, normal) * startDirection;
				m_Sensors[i].Update(point, dir, m_Distance);
				if (raycastTest)
					m_Sensors[i].Raycast(m_LayerMask, m_QueryTriggerInteraction);
				currAngle += angleStep;
			}
		}


#if BULLET_TIME
		private IEnumerator GetBisectionEdge(Vector3 point, Vector3 startDirection, Vector3 normal, float threshold)
#else
		private void GetBisectionEdge(Vector3 point, Vector3 startDirection, Vector3 normal, float threshold)
#endif
		{
			CacheRayResult(point, startDirection, normal, true);

			if (m_Density <= 1)
			{
				Debug.Log(GetType().Name + " at least two Raycast are required.");
			}
			else if (m_Density > 2)
			{
				int pt = m_Density, nextPt;
				while (pt-- > 0)
				{
					nextPt = Mathf.CeilToInt(Mathf.Repeat(pt - 1, m_Density));
					if (!m_Sensors[pt].hitted)
					{
						m_Sensors[pt] = RaycastData.NONE;
					}
					else if (!RaycastData.IsNullOrEmpty(m_Sensors[nextPt]))
					{
						// let's find the edge in between
						TryLocateBisectionEdge(ref m_Sensors[pt], ref m_Sensors[nextPt], ref m_Finder, m_LayerMask, m_QueryTriggerInteraction, threshold);
					}

#if BULLET_TIME
					if (m_WaitSec > 0f)
						yield return new WaitForSeconds(m_WaitSec);
#endif
				}
			}
		}

		/// <summary>Locate edge within two ray. limit by angle threshold</summary>
		/// <param name="start">Start arc direction</param>
		/// <param name="end">End arc direction</param>
		/// <param name="finder">Cache Helper</param>
		/// <param name="layerMask"></param>
		/// <param name="queryTriggerInteraction"></param>
		/// <param name="threshold">min angle</param>
		private void TryLocateBisectionEdge(ref RaycastData start, ref RaycastData end, ref RaycastData finder,
			int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
			float threshold = float.Epsilon)
		{
			if (start.origin != end.origin)
			{
				Debug.LogWarning("Start origin should always equal.", this);
				start.origin = end.origin;
			}

			if (start.IsHittingSameObject(end.hitResult, includeNull: true))
			{
				// both hit same thing || nothing, may cause by m_Density too low, we don't care at this point.
				finder.Reset();
				return;
			}
			else
			{
				if (!end.hitted && start.hitted)
				{
					// hit something, and one of it are Empty.
					// to combine case, alway making start as Empty. (locate direction will change)
					finder.Update(start, hitReferences: false);
					start.Update(end, hitReferences: false);
					end.Update(finder, hitReferences: false);
				}
				// else hit different object, no change.

				// reach angle threshold.
				threshold = Mathf.Clamp(threshold, float.Epsilon, MAX_THRESHOLD);
				if (threshold > Vector3.Angle(start.direction, end.direction))
				{
					finder.Reset();
					return;
				}

				// Bisection
				// recursive logic : assume origin point of start & end are equal.
				finder.Update(start.origin, Vector3.LerpUnclamped(start.direction, end.direction, .5f), start.distance);
				if (finder.Raycast(layerMask, queryTriggerInteraction))
				{
					// found the object, Move "end" closer
					end.Update(finder, hitReferences: true);
				}
				else
				{
					// not found, Move "start" closer
					end.Update(finder, hitReferences: true);
				}
				TryLocateBisectionEdge(ref start, ref end, ref finder, layerMask, queryTriggerInteraction, threshold);
			}
		}
	}
}