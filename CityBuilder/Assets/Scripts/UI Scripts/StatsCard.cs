using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI houseNameText;
    [SerializeField] private TextMeshProUGUI citizensText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI incomeText;
    public void UpdateStats(string houseName, int citizens, int energy, int income) 
    {
        houseNameText.text = $"{houseName}";
        citizensText.text = $"{citizens}";
        energyText.text = $"{energy}";
        incomeText.text = $"{income}";
    }
}
