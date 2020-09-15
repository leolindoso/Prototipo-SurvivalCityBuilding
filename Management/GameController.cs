using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Resources")]
    public  int Wood;
    public int Metal;
    public int Food;
    public int Water;
    public int Electronics;
    public int Cloth;
    public int Weapons;
    public int Medicine;
    [Header("Indicators")]
    public int Housing;
    public int Population;
    public int Happiness;
    [Header("Loot")]
    public LootController currentLoot;
    [Header("Time")]
    public int Days;
    public int SeasonDaysCount;
    public int SeasonMaxDays;
    [Header("Skills")]
    public bool Agriculture;

    public static void UnitTakeDamage(UnitCombatController attacker, UnitCombatController victim)
    {
        float damage = attacker.unitAttackStats.attackDamage;
        victim.TakeDamage(attacker, damage);
    }
    public void AddWood(int amount)
    {
        Wood += amount;
    }
    public void AddMetal(int amount)
    {
        Metal += amount;
    }
    public void AddFood(int amount)
    {
        Food += amount;
    }
    public void AddWater(int amount)
    {
        Water += amount;
    }
    public void AddElectronics(int amount)
    {
        Electronics += amount;
    }
    public void AddCloth(int amount)
    {
        Cloth += amount;
    }
    public void AddWeapons(int amount)
    {
        Weapons += amount;
    }
    public void AddMedicine(int amount)
    {
        Medicine += amount;
    }
    public void AddHousing(int amount)
    {
        Housing += amount;
    }
    public void AddPopulation(int amount)
    {
        Population += amount;
    }
    public int GetWood()
    {
        return Wood;
    }
    public int GetMetal()
    {
        return Metal;
    }
    public int GetFood()
    {
        return Food;
    }
    public int GetWater()
    {
        return Water;
    }
    public int GetElectronics()
    {
        return Electronics;
    }
    public int GetCloth()
    {
        return Cloth;
    }
    public int GetWeapons()
    {
        return Weapons;
    }
    public int GetMedicine()
    {
        return Medicine;
    }
    public int GetHousing()
    {
        return Housing;
    }
    public int GetPopulation()
    {
        return Population;
    }
    public  void SetCurrentLoot(LootController loot)
    {
        currentLoot = loot;

    }

    public void Update()
    {
        
        GameObject[] population = GameObject.FindGameObjectsWithTag("PlayerUnit");
        Population = population.Length;
    }

    public  void LootWood()
    {
        Debug.Log("LootWood");
        Wood += currentLoot.WoodAmount;
        Debug.Log("Wood:" + Wood + "Metal:" + Metal + "Food:" + Food + "Water:" + Water + "Electronics:" + Electronics + "Cloth:" + Cloth + "Weapons:" + Weapons + "Medicine:" + Medicine);
    }
    public  void LootMetal()
    {
        Metal += currentLoot.MetalAmount;
    }
    public  void LootFood()
    {
        Food += currentLoot.FoodAmount;
    }
    public  void LootWater()
    {
        Water += currentLoot.WaterAmount;
    }
    public  void LootElectronics()
    {
        Electronics += currentLoot.ElectronicAmount;
    }
    public  void LootCloth()
    {
        Cloth += currentLoot.ClothAmount;
    }
    public  void LootWeapons()
    {
        Weapons += currentLoot.WeaponAmount;
    }
    public  void LootMedicine()
    {
        Medicine += currentLoot.MedicineAmount;
    }

    public void AddDay()
    {
        Days += 1;
        SeasonDaysCount += 1;
        CheckSeasonDaysCount();
    }
    public void NextSeason()
    {
        WeatherController weather = GetComponent<WeatherController>();

        switch (weather.actualSeason)
        {
            case WeatherController.Season.Spring:
                weather.actualSeason = WeatherController.Season.Summer;
                GetComponent<WeatherController>().SetActualMax(GetComponent<WeatherController>().summerMax);
                GetComponent<WeatherController>().SetActualMin(GetComponent<WeatherController>().summerMin);
                break;
            case WeatherController.Season.Summer:
                weather.actualSeason = WeatherController.Season.Fall;
                GetComponent<WeatherController>().SetActualMax(GetComponent<WeatherController>().fallMax);
                GetComponent<WeatherController>().SetActualMin(GetComponent<WeatherController>().fallMin);
                break;
            case WeatherController.Season.Fall:
                weather.actualSeason = WeatherController.Season.Winter;
                GetComponent<WeatherController>().SetActualMax(GetComponent<WeatherController>().winterMax);
                GetComponent<WeatherController>().SetActualMin(GetComponent<WeatherController>().winterMin);
                break;
            case WeatherController.Season.Winter:
                weather.actualSeason = WeatherController.Season.Spring;
                GetComponent<WeatherController>().SetActualMax(GetComponent<WeatherController>().springMax);
                GetComponent<WeatherController>().SetActualMin(GetComponent<WeatherController>().springMin);
                break;
        }
    }
    public void CheckSeasonDaysCount()
    {
        if(SeasonDaysCount >= SeasonMaxDays)
        {
            SeasonDaysCount = 0;
            NextSeason();
        }
    }

    public void UnlockAgriculture()
    {
        Agriculture = true;
    }
}
