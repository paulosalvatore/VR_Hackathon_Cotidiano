using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
	#region Custom
	internal bool checado;
	#endregion

	#region Basic Attributes
	[Space(5)]
    [Header("+ Basic Attributes")]
    [Space(4)]
    public bool isBot;
    public string personName;
    public string profession;
    public Gender gender;
    public int age;

    [TextArea(4, 7)]
    public string description;

    public enum Gender
    {
        Male,
        Female
    }
    #endregion

    #region Feelings
    [Space(5)]
    [Header("+ Feelings")]
    [Space(4)]

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float temper;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float happiness;

    #region Auxiliaries
    private float lastTemper;
    private float lastHappiness;
    #endregion

    public Feeling feelings = new Feeling();
    #endregion

    #region Social Media
    [Space(5)]
    [Header("+ Social Media")]
    [Space(4)]
    public List<Post> posts = new List<Post>();
    #endregion

    #region Network Attributes
    [Space(5)]
    [Header("+ Network Attributes")]
    [Space(4)]
    [Tooltip("Index of Room")]
    [Range(0, 10)]
    public int roomIndex;
    public float latitude;
    public float longitude;
    public Country country;
    public float traffic;

    private int lastRoomIndex = -1;
    CountryName lastCoutryName;
    #endregion

    #region Messages Methods of MonoBehaviour
    private void Start()
	{
	}

    private void Update()
    {
        UpdateFeelings();
        UpdateLocation();
    }
    #endregion

    #region private void UpdateFeelings()
    private void UpdateFeelings()
    {
        //Temper
        if (temper != lastTemper && temper != feelings.Temper)
        {
            feelings.Temper = temper;
            lastTemper = temper;
        }

        if (temper == lastTemper && feelings.Temper != lastTemper)
        {
            temper = feelings.Temper;
            lastTemper = feelings.Temper;
        }

        //Happiness
        if (happiness != lastHappiness && happiness != feelings.Happiness)
        {
            feelings.Happiness = happiness;
            lastHappiness = happiness;
        }

        if (happiness == lastHappiness && feelings.Happiness != lastHappiness)
        {
            happiness = feelings.Happiness;
            lastHappiness = feelings.Happiness;
        }
    }
    #endregion

    #region private void UpdateLocation()
    private void UpdateLocation()
    {
        bool hasChanged = false;

        country.UpdateLocation();

        if (lastCoutryName != country.officialName)
        {
            roomIndex = (int)country.officialName;
            lastCoutryName = country.officialName;
            hasChanged = true;
        }

        if (lastRoomIndex != roomIndex)
        {
            #region atualiza "country.officialName" de acordo com "roomIndex"
            switch (roomIndex)
            {
                //case -1:
                //    roomIndex = 0;
                //    country.officialName = CountryName.Brazil;
                //    break;

                case (int)CountryName.Brazil:
                    country.officialName = CountryName.Brazil;
                    break;

                case (int)CountryName.UnitedStates:
                    country.officialName = CountryName.UnitedStates;
                    break;

                case (int)CountryName.SouthAfrica:
                    country.officialName = CountryName.SouthAfrica;
                    break;

                case (int)CountryName.Spain:
                    country.officialName = CountryName.Spain;
                    break;

                case (int)CountryName.UnitedKingdom:
                    country.officialName = CountryName.UnitedKingdom;
                    break;

                case (int)CountryName.Russia:
                    country.officialName = CountryName.Russia;
                    break;

                case (int)CountryName.Iraq:
                    country.officialName = CountryName.Iraq;
                    break;

                case (int)CountryName.India:
                    country.officialName = CountryName.India;
                    break;

                case (int)CountryName.China:
                    country.officialName = CountryName.China;
                    break;

                case (int)CountryName.Japan:
                    country.officialName = CountryName.Japan;
                    break;

                case (int)CountryName.Australia:
                    country.officialName = CountryName.Australia;
                    break;                    
            }
            #endregion

            lastRoomIndex = roomIndex;
            hasChanged = true;
        }

        if (hasChanged)
        {
            GeneratePosition();
        }

	}
	#endregion

	internal List<NPC> npcsLigados = new List<NPC>();

	public float PegarValor()
	{
		return Mathf.Max(npcsLigados.Count / 20f, traffic);
	}

	public void GeneratePosition()
    {
        float minX = country.longitude - NPCManager.MaxDistanceBetweenNPCs.x;
        float maxX = country.longitude + NPCManager.MaxDistanceBetweenNPCs.x;

        float minZ = country.latitude - NPCManager.MaxDistanceBetweenNPCs.y;
        float maxZ = country.latitude + NPCManager.MaxDistanceBetweenNPCs.y;

        latitude = Random.Range(minZ, maxZ);
        longitude = Random.Range(minX, maxX);

        transform.localPosition = new Vector3(
			longitude,
            transform.position.y,
			latitude
        );
    }

}
