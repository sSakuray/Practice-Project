using UnityEngine;

public class HouseArrowClick : MonoBehaviour
{
    public static GameObject currentArrow;
    public GameObject arrowPrefab; 
    public Vector3 arrowOffset = new Vector3(0, 3f, 0);

    private void OnMouseDown()
    {
        var spawnStats = GetComponent<SpawnStats>();
        if (spawnStats != null && !spawnStats.IsPlaced)
            return;
        if (currentArrow != null)
            Destroy(currentArrow);

        if (arrowPrefab != null && SpawnStats.currentActiveStatPanel != null)
        {
            currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.position = transform.position + arrowOffset;
            currentArrow.transform.SetParent(null);
        }
    }
}
