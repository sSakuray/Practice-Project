using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI citizensText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI incomeText;

    public void UpdateStats(int citizens, int energy, int income) {
        citizensText.text = $"{citizens}";
        energyText.text = $"{energy}";
        incomeText.text = $"{income}";
    }
}
