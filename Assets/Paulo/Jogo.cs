using NewtonVR.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogo : MonoBehaviour
{
	public FollowPlayer watson;
	private Transform watsonFollowInicial;

	public NVRExampleLaserPointer left_nvr;
	public NVRExampleLaserPointer right_nvr;

	internal static Jogo instancia;

	private bool selecionarNpcLiberado = false;

	internal NPC npcSelecionado;
	internal List<NPC> npcsMapeados = new List<NPC>();

	private bool exibindoGeoLocalizacao = false;

	public GameObject chao;
	public GameObject npcs;
	public GameObject terra;

	public List<AudioClip> clips;

	public List<AudioClip> watsonClips;

	public GameObject logo;

	private AudioSource audioSource;

	private AudioSource audioFX;

	private void Awake()
	{
		instancia = this;

		selecionarNpcLiberado = true;

		watsonFollowInicial = watson.target;

		audioSource = GetComponent<AudioSource>();

		audioFX = gameObject.AddComponent<AudioSource>();
		audioFX.playOnAwake = false;

		Invoke("IniciarPreparacao", 0.2f);
		//preparacaoEmAndamento = false;
	}

	private bool preparacaoEmAndamento = true;

	private void IniciarPreparacao()
	{
		chao.SetActive(false);
		npcs.SetActive(false);

		watson.rotacionarDesabilitado = true;
		watson.moverDesabilitado = true;
		watson.modelUpDown = true;
		watson.minimum = -0.1f;
		watson.maximum = 0.1f;

		audioSource.clip = clips[0];
		audioSource.Play();

		Invoke("TocarAudio2", audioSource.clip.length);
	}

	private void TocarAudio2()
	{
		audioSource.clip = clips[1];
		audioSource.Play();

		Invoke("ExibirChao", audioSource.clip.length + 1);
	}

	private void ExibirChao()
	{
		chao.SetActive(true);
		npcs.SetActive(false);

		watson.rotacionarDesabilitado = false;
		watson.moverDesabilitado = false;
		watson.modelUpDown = false;

		audioSource.clip = clips[4];
		audioSource.Play();

		Invoke("TocarAudio3", audioSource.clip.length + 1);
	}

	private void TocarAudio3()
	{
		audioSource.clip = clips[7];
		audioSource.Play();

		Invoke("TocarAudio4", audioSource.clip.length + 3f);
	}

	private void TocarAudio4()
	{
		audioSource.clip = clips[5];
		audioSource.Play();

		Invoke("CarregarNpcs", audioSource.clip.length + 3f);
	}

	private void CarregarNpcs()
	{
		npcs.SetActive(true);

		preparacaoEmAndamento = false;
	}

	private bool gripLiberado = true;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
			Checar();
		else if (Input.GetKeyDown(KeyCode.X))
			Localizar();
		else if (Input.GetKeyDown(KeyCode.C))
			SelecionarNpcAleatorio();
		else if (Input.GetKeyDown(KeyCode.V))
			MapearConexoes();
		else if (Input.GetKeyDown(KeyCode.B))
			Eliminar();
		else if (Input.GetKeyDown(KeyCode.Q))
			ExibirGeoLocalizacao();
		else if (Input.GetKeyDown(KeyCode.W))
			OcultarGeoLocalizacao();
		else if (Input.GetKeyDown(KeyCode.E))
			Vitoria();
		else if (Input.GetKeyDown(KeyCode.R))
			Derrota();

		if (preparacaoEmAndamento)
			return;

		if (gripLiberado && (left_nvr.grip > 0 || right_nvr.grip > 0))
		{
			if (!exibindoGeoLocalizacao)
				Localizar();
			else
				OcultarGeoLocalizacao();

			gripLiberado = false;
		}

		if (left_nvr.grip == 0 && right_nvr.grip == 0)
			gripLiberado = true;
	}

	public void SelecionarNpc(NPC npc)
	{
		if (!selecionarNpcLiberado)
			return;

		DesselecionarNpc();

		npcSelecionado = npc;

		audioFX.clip = efeitosSonoros[0];
		audioFX.Play();

		WatsonFollow(npcSelecionado.transform);
	}

	public void DesselecionarNpc(bool destruirConexoesAtuais = false)
	{
		if (destruirConexoesAtuais && npcSelecionado)
			foreach (Transform child in npcSelecionado.transform)
				Destroy(child.gameObject);

		npcSelecionado = null;

		WatsonFollow(watsonFollowInicial);
	}

	private void WatsonFollow(Transform follow)
	{
		watson.target = follow;
	}

	public Material materialLinha;

	public void Checar()
	{
		if (npcSelecionado == null || npcSelecionado.checado)
			return;

		npcSelecionado.checado = true;

		/*
		 * Watson vai até o usuário checar suas conexões
		 */

		watson.upDownWhenRotating = true;

		audioFX.clip = efeitosSonoros[1];
		audioFX.Play();

		Invoke("CompletarChecar", audioFX.clip.length);
	}

	void CompletarChecar()
	{
		audioFX.clip = efeitosSonoros[2];
		audioFX.Play();

		watson.modelUpDown = false;

		foreach (NPC npcLigacao in npcSelecionado.npcsLigados)
		{
			if (!npcLigacao)
				continue;

			if (!npcsMapeados.Contains(npcLigacao))
				npcsMapeados.Add(npcLigacao);

			GameObject objeto = new GameObject();
			LineRenderer linha = objeto.AddComponent<LineRenderer>();
			linha.material = materialLinha;

			linha.material.SetColor("_Color", new Color(0, 1, 1));
			linha.startWidth = 0.03f;
			linha.endWidth = 0.03f;

			Vector3 target = npcLigacao.transform.position;
			linha.SetPositions(new Vector3[] { npcSelecionado.transform.position, target });
			objeto.transform.parent = npcSelecionado.transform;
			objeto.name = npcLigacao.name;
		}
	}

	public void Localizar()
	{
		if (npcSelecionado == null || !npcSelecionado.checado || exibindoGeoLocalizacao)
			return;

		/*
		 * Watson abre a geolocalização do usuário
		 */

		Debug.Log("Localizar Usuário");

		ExibirGeoLocalizacao();
	}

	public Gradient Colors;

	public void MapearConexoes()
	{
		if (npcSelecionado == null || !npcSelecionado.checado || exibindoGeoLocalizacao)
			return;

		foreach (NPC npc in npcSelecionado.npcsLigados)
		{
			float valor = npc.PegarValor();

			Color valueColor = Colors.Evaluate(valor);

			npcSelecionado.transform.Find(npc.name).GetComponent<LineRenderer>().material.SetColor("_Color", valueColor);
		}
	}

	public int vitoria = 3;
	public int derrota = 3;
	private int botsEliminados;
	private int usuariosEliminados;

	public List<AudioClip> efeitosSonoros;

	public void Eliminar()
	{
		if (npcSelecionado == null || !npcSelecionado.checado || exibindoGeoLocalizacao)
			return;

		audioFX.clip = efeitosSonoros[3];
		audioFX.Play();

		if (npcSelecionado.isBot)
		{
			botsEliminados++;

			if (botsEliminados == 1)
			{
				audioSource.clip = clips[8];
				audioSource.Play();
			}
			else if (botsEliminados == 2)
			{
				audioSource.clip = clips[9];
				audioSource.Play();
			}
			else if (botsEliminados == 3)
			{
				audioSource.clip = clips[10];
				audioSource.Play();
			}
		}
		else
		{
			usuariosEliminados++;

			if (usuariosEliminados == 1)
			{
				audioSource.clip = clips[13];
				audioSource.Play();
			}
			else if (usuariosEliminados == 2)
			{
				audioSource.clip = clips[14];
				audioSource.Play();
			}
			else if (usuariosEliminados == 3)
			{
				audioSource.clip = clips[15];
				audioSource.Play();
			}
		}

		float delayDestruir = 0;
		
		foreach (NPC npc in npcsMapeados)
		{
			foreach (Transform child in npc.transform)
				if (child.name == npcSelecionado.name)
					child.GetComponent<LineRenderer>().enabled = false;

			foreach (NPC npcLigado in npc.npcsLigados)
				foreach (Transform child in npcLigado.transform)
					if (child.name == npcSelecionado.name)
						child.GetComponent<LineRenderer>().enabled = false;
		}

		Destruir(npcSelecionado, delayDestruir);

		foreach (Transform child in npcSelecionado.transform)
			child.GetComponent<LineRenderer>().enabled = false;

		WatsonFollow(watsonFollowInicial);

		npcSelecionado = null;

		Invoke("ValidarFimJogo", audioSource.clip.length);
	}

	private void ValidarFimJogo()
	{
		if (botsEliminados == vitoria)
		{
			Vitoria();
		}
		else if (usuariosEliminados == derrota)
		{
			Derrota();
		}
	}

	private void Vitoria()
	{
		Debug.Log("Vitoria");

		audioSource.clip = clips[12];
		audioSource.Play();
	}

	private void Derrota()
	{
		Debug.Log("Derrota");

		StartCoroutine(DestruirNpcs());
	}

	private IEnumerator DestruirNpcs()
	{
		foreach (GameObject npc in NPCManager.instancia.npcList)
		{
			Destruir(npc.GetComponent<NPC>());

			yield return new WaitForSeconds(Random.Range(0.001f, 0.01f));
		}
	}

	public GameObject explosao;

	private void Destruir(NPC npc, float delay = 0)
	{
		if (npc.GetComponent<MeshRenderer>())
			npc.GetComponent<MeshRenderer>().enabled = false;
		else if (npc.GetComponent<LineRenderer>())
			npc.GetComponent<LineRenderer>().enabled = false;

		Instantiate(explosao, npc.transform.position, explosao.transform.rotation);

		npcsMapeados.Remove(npc);
	}

	public void ExibirGeoLocalizacao()
	{
		audioFX.clip = efeitosSonoros[4];
		audioFX.Play();

		exibindoGeoLocalizacao = true;

		terra.SetActive(true);

		chao.SetActive(false);
		npcs.SetActive(false);
		watson.gameObject.SetActive(false);
	}

	public void OcultarGeoLocalizacao()
	{
		audioFX.clip = efeitosSonoros[5];
		audioFX.Play();

		exibindoGeoLocalizacao = false;

		terra.SetActive(false);

		chao.SetActive(true);
		npcs.SetActive(true);
		watson.gameObject.SetActive(true);
	}

	private void SelecionarNpcAleatorio()
	{
		int npcIndex = Random.Range(0, NPCManager.instancia.npcList.Count);

		NPC npc = NPCManager.instancia.npcList[npcIndex].GetComponent<NPC>();

		SelecionarNpc(npc);

		Checar();
	}
}
