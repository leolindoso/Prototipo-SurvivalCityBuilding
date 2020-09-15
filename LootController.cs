using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootController : MonoBehaviour
{
    public LootStats lootStats;
    public int WoodAmount;
    public int MetalAmount;
    public int FoodAmount;
    public int WaterAmount;
    public int ElectronicAmount;
    public int ClothAmount;
    public int WeaponAmount;
    public int MedicineAmount;
    public bool Lootable;
    public GameObject canvas;
    private void Start()
    {
        if(Random.Range(0,100) <= lootStats.WoodChance)
        {
            WoodAmount = Random.Range(1, lootStats.WoodMax);
        }
        if (Random.Range(0, 100) <= lootStats.MetalChance)
        {
            MetalAmount = Random.Range(1, lootStats.MetalMax);
        }
        if (Random.Range(0, 100) <= lootStats.FoodChance)
        {
            FoodAmount = Random.Range(1, lootStats.FoodMax);
        }
        if (Random.Range(0, 100) <= lootStats.WaterChance)
        {
            WaterAmount = Random.Range(1, lootStats.WaterMax);
        }
        if (Random.Range(0, 100) <= lootStats.ElectronicChance)
        {
            ElectronicAmount = Random.Range(1, lootStats.ElectronicMax);
        }
        if (Random.Range(0, 100) <= lootStats.ClothChance)
        {
            ClothAmount = Random.Range(1, lootStats.ClothMax);
        }
        if (Random.Range(0, 100) <= lootStats.WeaponChance)
        {
            WeaponAmount = Random.Range(1, lootStats.WeaponMax);
        }
        if (Random.Range(0, 100) <= lootStats.MedicineChance)
        {
            MedicineAmount = Random.Range(1, lootStats.MedicineMax);
        }
    }
    private void Update()
    {
        
    }
    public void GenerateLoot()
    {
        Lootable = false;
        HUDController hud = GameObject.Find("GameManager").GetComponent<HUDController>();
        hud.GenerateLoot(this);
        DesactivateCanvas();
    }
    public void ActivateCanvas()
    {
        canvas.SetActive(true);
        
    }
    public void DesactivateCanvas()
    {
        canvas.SetActive(false);

    }
}
