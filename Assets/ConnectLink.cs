using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLink : MonoBehaviour
{
	public List<NPCLink> npcs;
	public List<GameObject> lista = new List<GameObject>();
	public Vector3 origin;
	public Vector3 target;

	private void Start()
	{
		int totalnpc = npcs.Count;
		origin = transform.position;

		for (int i = 0; i < totalnpc; i++)
		{
			if (Random.Range(0f, 1f) > 0.5f)
			{
				GameObject objeto = new GameObject();
				LineRenderer linha = objeto.AddComponent<LineRenderer>();
				linha.material = gameObject.GetComponent<LineRenderer>().material;
				linha.SetColors(Color.blue, Color.blue);
				linha.SetWidth(0.05f, 0.05f);
				target = npcs[i].origin;
				linha.SetPositions(new Vector3[] { origin, target });
				objeto.transform.parent = transform;
				lista.Add(objeto);
			}
		}
	}

	// Update is called once per frame
	private void Update()
	{
	}
}
