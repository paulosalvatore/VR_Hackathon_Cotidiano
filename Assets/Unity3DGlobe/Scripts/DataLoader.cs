using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoader : MonoBehaviour
{
	public DataVisualizer Visualizer;

	private List<float> data;

	// Use this for initialization
	private void Start()
	{
		/*
		TextAsset jsonData = Resources.Load<TextAsset>("population");
		string json = jsonData.text;
		SeriesArray data = JsonUtility.FromJson<SeriesArray>(json);
		*/

		TesteLoad();
	}

	private void TesteLoad()
	{
		// Posicionar NPCs no Globo
		data = new List<float>();

		List<NPC> npcs = Jogo.instancia.npcsMapeados;

		foreach (NPC npc in npcs)
		{
			float valor = npc.PegarValor();

			data.Add(npc.latitude);
			data.Add(npc.longitude);
			data.Add(valor);
		}

		Visualizer.CreateMeshes(data);
	}

	private void Update()
	{
	}
}

[System.Serializable]
public class SeriesArray
{
	public SeriesData[] AllData;
}
