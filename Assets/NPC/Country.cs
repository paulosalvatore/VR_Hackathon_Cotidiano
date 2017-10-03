using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Country
{

    public CountryName officialName;
    public float latitude;
    public float longitude;
    public Mainland MainLand { get; set; }

    public void UpdateLocation()
    {
        switch (officialName)
        {
            case CountryName.Brazil:
                latitude = -15.8034988f;
                longitude = -48.0300896f;
                MainLand = Mainland.America;
                break;

            case CountryName.UnitedStates:
                latitude = 38.899265f;
                longitude = -77.1546526f;
                MainLand = Mainland.America;
                break;
                
            case CountryName.SouthAfrica:
                latitude = -25.870514f;
                longitude = 27.5003378f;
                MainLand = Mainland.Africa;
                break;
                
            case CountryName.Spain:
                latitude = 40.1207723f;
                longitude = -9.1148738f;
                MainLand = Mainland.Europe;
                break;
                
            case CountryName.UnitedKingdom:
                latitude = 52.6526765f;
                longitude = -1.4201357f;
                MainLand = Mainland.Europe;
                break;
                
            case CountryName.Russia:
                latitude = 57.023589f;
                longitude = 42.5680016f;
                MainLand = Mainland.Europe;
                break;
                
            case CountryName.Iraq:
                latitude = 33.5334827f;
                longitude = 41.5045714f;
                MainLand = Mainland.Asia;
                break;
                
            case CountryName.India:
                latitude = 2.4745381f;
                longitude = 93.9783168f;
                MainLand = Mainland.Asia;
                break;
                
            case CountryName.China:
                latitude = 39.9385466f;
                longitude = 116.1172666f;
                MainLand = Mainland.Asia;
                break;
                
            case CountryName.Japan:
                latitude = 35.6691088f;
                longitude = 139.6012941f;
                MainLand = Mainland.Asia;
                break;
                
            case CountryName.Australia:
                latitude = -35.313899f;
                longitude = 148.9896959f;
                MainLand = Mainland.Oceania;
                break;
                       
            //case CountryName.:
            //    latitude = f;
            //    longitude = f;
            //    MainLand = Mainland.;
            //    break;
                
        }
    }

}

public enum Mainland
{
    Africa,
    America,
    Antarctica,
    Asia,
    Europe,
    Oceania
}

public enum CountryName
{
    Brazil,
    UnitedStates,
    SouthAfrica,
    Spain,
    UnitedKingdom,
    Russia,
    Iraq,
    India,
    China,
    Japan,
    Australia
}