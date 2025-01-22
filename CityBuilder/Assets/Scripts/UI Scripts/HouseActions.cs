using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseActions : MonoBehaviour
{
    [SerializeField] private Button upgradeButton; 
    [SerializeField] private Button rotateButton;  
    [SerializeField] private Button deleteButton;   
    [SerializeField] public GameObject statPanel; 


    [System.Serializable]
    public class UpgradeOption
    {
        public GameObject currentPrefab;  
        public GameObject upgradedPrefab; 
        public float upgradeCost;  
    }

    [SerializeField] private List<UpgradeOption> upgradeOptions; 

    private GameObject currentHouse; 
    private float houseCost; 

    private void Start()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(UpgradeHouse);
        }

        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(RotateHouse);
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(DeleteHouse);
        }

    }
    

    public void SetCurrentHouse(GameObject house, float cost)
    {
        currentHouse = house;
        houseCost = cost;
    }

    public void UpgradeHouse()
    {
        if (currentHouse == null)
        {
            return;
        }

        foreach (var option in upgradeOptions)
        {
            if (option.currentPrefab.name == currentHouse.name.Replace("(Clone)", "").Trim())
            {
                if (GameManager.Instance.money >= option.upgradeCost)
                {
                    Vector3 position = currentHouse.transform.position;
                    Quaternion rotation = currentHouse.transform.rotation;

                    Destroy(currentHouse);
                    GameObject upgradedHouse = Instantiate(option.upgradedPrefab, position, rotation);

                    GameManager.Instance.money -= option.upgradeCost; 
                    currentHouse = upgradedHouse; 

                }

                return;
            }
        }

    }

    public void RotateHouse()
    {
        if (currentHouse == null)
        {
            return;
        }

        currentHouse.transform.RotateAround(currentHouse.transform.position, Vector3.up, 90);
    }

    public void DeleteHouse()
    {
        GridCell cell = currentHouse.GetComponent<GridCell>();
        if (currentHouse == null)
        {
            return;
        }
        if (statPanel != null)
        {
            Destroy(statPanel); 
        }

        Destroy(currentHouse);
        currentHouse = null; 

        GameManager.Instance.money += houseCost;
    }
}
