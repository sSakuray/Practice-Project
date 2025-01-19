using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    [Header("References")]
    [Header("Cost")]
    [SerializeField] private int maxTiers = 4;
    [SerializeField] private int buildingCost = 100;
    [Header("Tier1")]
    [SerializeField] private string buildingNameTier1 = "test";
    [SerializeField] private int citizensProductionTier1 = 100;
    [SerializeField] private int energyProductionTier1 = 100;
    [SerializeField] private int energyConsumeTier1 = 100;
    [SerializeField] private int CitizenConsumeTier1 = 100;
    [SerializeField] private int passiveIncomeTier1 = 100;
    [SerializeField] private int upgradeToTier2Cost = 100;
    [Header("Tier2")]
    [SerializeField] private string buildingNameTier2 = "test";
    [SerializeField] private int citizensProductionTier2 = 100;
    [SerializeField] private int energyProductionTier2 = 100;
    [SerializeField] private int energyConsumeTier2 = 100;
    [SerializeField] private int citizenConsumeTier2 = 100;
    [SerializeField] private int passiveIncomeTier2 = 100;
    [SerializeField] private int upgradeToTier3Cost = 100;
    [Header("Tier3")]
    [SerializeField] private string buildingNameTier3 = "test";
    [SerializeField] private int citizensProductionTier3 = 100;
    [SerializeField] private int energyProductionTier3 = 100;
    [SerializeField] private int energyConsumeTier3 = 100;
    [SerializeField] private int citizenConsumeTier3 = 100;
    [SerializeField] private int passiveIncomeTier3 = 100;
    [SerializeField] private int upgradeToTier4Cost = 100;
    
    [Header("Tier4")]
    [SerializeField] private string buildingNameTier4 = "test";
    [SerializeField] private int citizensProductionTier4 = 100;
    [SerializeField] private int energyProductionTier4 = 100;
    [SerializeField] private int energyConsumeTier4 = 100;
    [SerializeField] private int citizenConsumeTier4 = 100;
    [SerializeField] private int passiveIncomeTier4 = 100;
}

