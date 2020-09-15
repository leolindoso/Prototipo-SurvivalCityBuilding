using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Resources")]
    public Text WoodText;
    public Text MetalText;
    public Text FoodText;
    public Text WaterText;
    public Text ElectronicsText;
    public Text ClothText;
    public Text WeaponsText;
    public Text MedicineText;
    [Header("Central")]
    public Text HoursText;
    public Text TemperatureText;
    public Text SeasonText;
    [Header("Indicators")]
    public Text HousingText;
    public Text HappinessText;
    [Header("Loot")]
    public Text WoodLootText;
    public GameObject WoodLootBtn;
    public Text MetalLootText;
    public GameObject MetalLootBtn;
    public Text FoodLootText;
    public GameObject FoodLootBtn;
    public Text WaterLootText;
    public GameObject WaterLootBtn;
    public Text ElectronicsLootText;
    public GameObject ElectronicsLootBtn;
    public Text ClothLootText;
    public GameObject ClothLootBtn;
    public Text WeaponsLootText;
    public GameObject WeaponsLootBtn;
    public Text MedicineLootText;
    public GameObject MedicineLootBtn;
    public GameObject LootPanel;
    [Header("Expeditions")]
    public GameObject ExplorersPanel;
    public GameObject ExplorerBtn;
    [Header("Manage")]
    public GameObject ManagePanel;
    public GameObject PopulationBtn;
    public Text PersonName;
    private void Start()
    {
        //PROVISORIAMENTE AQUI, TEM Q ATUALIZAR SEMPRE QUE ABRE O EXPEDITIONS
        //GetAvaliableExplorers();
    }
    public void Update()
    {
        HUD_Update();

    }

    private void HUD_Update()
    {
        var GC = GameObject.Find("GameManager").GetComponent<GameController>();
        WoodText.text = GC.GetWood().ToString();
        MetalText.text = GC.GetMetal().ToString();
        FoodText.text = GC.GetFood().ToString();
        WaterText.text = GC.GetWater().ToString();
        ElectronicsText.text = GC.GetElectronics().ToString();
        ClothText.text = GC.GetCloth().ToString();
        WeaponsText.text = GC.GetWeapons().ToString();
        MedicineText.text = GC.GetMedicine().ToString();

        HoursText.text = LightingController.timeDay.ToString("0") + ":00";
        TemperatureText.text = GetComponent<WeatherController>().temperature.ToString("0°");
        SeasonText.text = GetComponent<WeatherController>().actualSeason.ToString();

        HousingText.text = GC.GetPopulation().ToString()+ "/" +GC.GetHousing().ToString();
        
        if(GetComponent<ManageController>().selectedPerson != null)
        {
            PersonName.text = GetComponent<ManageController>().selectedPerson.name;
        }
    }

    public void ToggleActivationOf(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }
    public void GenerateLoot(LootController loot)
    {
        if(loot.WoodAmount == 0)
        {
            WoodLootBtn.SetActive(false);
        }
        else
        {
            WoodLootBtn.SetActive(true);
            WoodLootText.text = loot.WoodAmount.ToString();
        }
        
        if (loot.MetalAmount == 0)
        {
            MetalLootBtn.SetActive(false);
        }
        else
        {
            MetalLootBtn.SetActive(true);
            MetalLootText.text = loot.MetalAmount.ToString();
        }
        if (loot.FoodAmount == 0)
        {
            FoodLootBtn.SetActive(false);
        }
        else
        {
            FoodLootBtn.SetActive(true);
            FoodLootText.text = loot.FoodAmount.ToString();
        }
        if (loot.WaterAmount == 0)
        {
            WaterLootBtn.SetActive(false);
        }
        else
        {
            WaterLootBtn.SetActive(true);
            WaterLootText.text = loot.WaterAmount.ToString();
        }
        if (loot.ElectronicAmount == 0)
        {
            ElectronicsLootBtn.SetActive(false);
        }
        else
        {
            ElectronicsLootBtn.SetActive(true);
            ElectronicsLootText.text = loot.ElectronicAmount.ToString();
        }
        if (loot.ClothAmount == 0)
        {
            ClothLootBtn.SetActive(false);
        }
        else
        {
            ClothLootBtn.SetActive(true);
            ClothLootText.text = loot.ClothAmount.ToString();
        }
        if (loot.WeaponAmount == 0)
        {
            WeaponsLootBtn.SetActive(false);
        }
        else
        {
            WeaponsLootBtn.SetActive(true);
            WeaponsLootText.text = loot.WeaponAmount.ToString();
        }
        if (loot.MedicineAmount == 0)
        {
            MedicineLootBtn.SetActive(false);
        }
        else
        {
            MedicineLootText.text = loot.MedicineAmount.ToString();
            MedicineLootBtn.SetActive(true);
        }
        LootPanel.SetActive(true);
    }

    public void GetAvaliableExplorers()
    {
        foreach(GameObject explorer in GetComponent<ExpeditionController>().avaliableExplorers)
        {
            var aux = Instantiate(ExplorerBtn, ExplorersPanel.transform);
            aux.transform.Find("Name").GetComponent<Text>().text = explorer.name;
        }
        
    }
    public void GetPopulation()
    {
        foreach (GameObject person in GetComponent<ManageController>().population)
        {
            var aux = Instantiate(PopulationBtn, ManagePanel.transform);
            aux.transform.Find("Name").GetComponent<Text>().text = person.name;
        }

    }
}
