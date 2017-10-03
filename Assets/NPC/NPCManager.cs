using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
	internal static NPCManager instancia;

    #region NPC Options
    [Space(5)]
    [Header("+ NPC Options")]
    [Space(4)]
    [Tooltip("NPC Model For Instantiate")]
    public GameObject NPCModelForInstantiate;
    [Tooltip("Number of Bots NPC")]
    [Range(1, 10)]
    public int numberBotsNPC = 3;
    [Tooltip("Number of People (NPCs) per Room")]
    [Range(3, 100)]
    public int numberOfNPCsPerRoom = 10;

    public float geolocationCorrection = 80f;
	public float geolocationCorrectionNpc = 80f;
	public static float GeolocationCorrection { get; set; }
	public static float GeolocationCorrectionNpc { get; set; }

	[Tooltip("Distance Between People (NPCs)")]
    public Vector2 maxDistanceBetweenNPCs = new Vector2(0.5f, 0.5f);
    public static Vector2 MaxDistanceBetweenNPCs { get; set; }

    public List<GameObject> npcList = new List<GameObject>();

    private int countBotNPC;
    #endregion

    #region Room Options
    [Space(5)]
    [Header("+ Room Options")]
    [Space(4)]
    [Tooltip("Number of Rooms")]
    [Range(1, 11)]
    public int numberOfRooms = 3;
    public Vector2 roomSizes = new Vector2(20, 20);
	public GameObject floorPrefab;
    #endregion

    private RandomNumber uniqueRoomNumbers;

    #region Messages Methods of MonoBehaviour
    private void Start()
    {
		instancia = this;

		npcList = new List<GameObject>(numberOfRooms * numberOfNPCsPerRoom);
        GenerateRandomNPC();

		ConstruirLigacoes();
	}

    private void Update()
    {
        GeolocationCorrection = geolocationCorrection;
        MaxDistanceBetweenNPCs = maxDistanceBetweenNPCs;
    }
	#endregion

	public void GenerateRandomNPC()
	{
		int numberOfNPCs = numberOfRooms * numberOfNPCsPerRoom;
		uniqueRoomNumbers = new RandomNumber(0, 11);

		int indexNPCList = 0;

		for (int countRoom = 0; countRoom < numberOfRooms; countRoom++)
		{
			//sorteia a sala (país)            
			int currentRoomIndex = 0/*uniqueRoomNumbers.GetUniqueInt()*/;

			GameObject chao = Instantiate(floorPrefab);
			chao.transform.position = Vector3.zero;
			chao.transform.parent = Jogo.instancia.chao.transform;

			if (countRoom == 0)
				GameObject.Find("[VRTK_SDKManager]").transform.position = chao.transform.position;

			for (int countNPC = 0; countNPC < numberOfNPCsPerRoom; countNPC++)
            {
                //adiciona 1 pessoa na sala

                GameObject currentNpcObject = Instantiate(NPCModelForInstantiate);
                npcList.Add(currentNpcObject);
                currentNpcObject.name = "npc" + indexNPCList;
                indexNPCList++;

                NPC currentNPC = currentNpcObject.GetComponent<NPC>();

                //print("CurrentRoomIndex: " + currentRoomIndex + ", currentNPC.roomIndex: " + currentNPC.roomIndex);
                //adiciona a pessoa dentro de 1 sala
                currentNPC.roomIndex = currentRoomIndex;

                //sorteia um bot
                if (countBotNPC < numberBotsNPC)
                {
                    currentNPC.isBot = Random.Range(0, 2) == 0 ? false : true;

                    if (currentNPC.isBot)
                    {
                        countBotNPC++;
                    }
                }

                //sorteia os Feelings
                currentNPC.feelings.Temper = Random.Range(-1f, 1.1f);
				currentNPC.feelings.Happiness = Random.Range(-1f, 1.1f);

				currentNPC.traffic = Random.Range(0f, 1f);

				//sorteia a posição
				//currentNPC.GeneratePosition();

				//altera transform
				currentNPC.transform.parent = transform;
            }
        }
    }

	private void ConstruirLigacoes()
	{
		foreach (GameObject npc in npcList)
		{
			int ligacoes = Random.Range(5, 10);

			NPC npcScript = npc.GetComponent<NPC>();

			if (npcScript.isBot)
				ligacoes += Random.Range(3, 20);

			RandomNumber uniqueRoomNumbers = new RandomNumber(0, npcList.Count);
			
			for (int i = 0; i < ligacoes; i++)
			{
				int npcLigarIndex = uniqueRoomNumbers.GetUniqueInt();

				NPC npcLigar = npcList[npcLigarIndex].GetComponent<NPC>();

				npcScript.npcsLigados.Add(npcLigar);
			}
		}
	}
}