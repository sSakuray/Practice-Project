using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRoot : MonoBehaviour {}

public class ShopManager : MonoBehaviour
{
    GameObject[] previews = new GameObject[4];
    bool placingPanel;
    int hovered;
    static readonly Color previewColor = Color.gray, originalColor = Color.white;

    [Header("Panel Shop")]
    public GameObject panelPrefab;
    public Button buyPanelButton;
    public int panelPrice = 1000;
    public List<GameObject> panels = new List<GameObject>();

    [System.Serializable]
    public class HouseData
    {
        public GameObject housePrefab;
        public Button buyButton;
        public int price;
    }
    [SerializeField] public List<HouseData> houses;

    public void Start()
    {
        foreach (var houseData in houses)
        {
            houseData.buyButton.onClick.AddListener(() => OnBuyClicked(houseData));
        }
        if (buyPanelButton != null)
            buyPanelButton.onClick.AddListener(OnBuyPanelClicked);
        if (panels.Count == 0 && panelPrefab != null)
        {
            var root = FindPanelRootOnScene();
            if (root != null)
                panels.Add(root.gameObject);
        }
    }

    private PanelRoot FindPanelRootOnScene()
    {
        foreach (var go in GameObject.FindObjectsOfType<GameObject>())
        {
            if (go.GetComponent<PanelRoot>() != null)
                return go.GetComponent<PanelRoot>();
        }
        return null;
    }

    // Покупка панели
    public void OnBuyPanelClicked()
    {
        if (panelPrefab == null || panels.Count == 0) 
        {
            if (GameManager.Instance.money < panelPrice)
            {
                StartCoroutine(MakeButtonFlash(buyPanelButton, Color.red));
                GameManager.Instance.ShowUpgradeWarning(false, false, true);
            }
            return;
        }
        
        if (GameManager.Instance.money < panelPrice)
        {
            StartCoroutine(MakeButtonFlash(buyPanelButton, Color.red));
            GameManager.Instance.ShowUpgradeWarning(false, false, true);
            return;
        }
        
        // Clear any existing previews
        foreach (var p in previews) 
        {
            if (p != null) 
                Destroy(p);
        }
        
        var bounds = GetBounds(panelPrefab);
        float dx = bounds.size.x;
        float dz = bounds.size.z;
        Vector3 origin = panels[0].transform.position;
        float y = origin.y;
        HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
        
        // Mark all existing panel positions as occupied
        foreach (var p in panels)
        {
            if (p == null) continue;
            Vector3 pos = p.transform.position - origin;
            int x = Mathf.RoundToInt(pos.x / dx);
            int z = Mathf.RoundToInt(pos.z / dz);
            occupied.Add(new Vector2Int(x, z));
        }
        
        // Find all possible positions for new panels
        List<Vector3> previewPositions = new List<Vector3>();
        Vector2Int[] dirs = { new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(-1,0) };
        
        foreach (var p in panels)
        {
            if (p == null) continue;
            Vector3 pos = p.transform.position - origin;
            int x = Mathf.RoundToInt(pos.x / dx);
            int z = Mathf.RoundToInt(pos.z / dz);
            
            foreach (var dir in dirs)
            {
                Vector2Int np = new Vector2Int(x + dir.x, z + dir.y);
                if (occupied.Contains(np)) continue;
                
                Vector3 previewPos = new Vector3(np.x * dx, 0, np.y * dz) + origin;
                previewPos.y = y;
                
                if (previewPositions.Exists(v => (v - previewPos).sqrMagnitude < 0.01f)) 
                    continue;
                    
                previewPositions.Add(previewPos);
            }
        }
        
        if (previewPositions.Count == 0)
        {
            StartCoroutine(MakeButtonFlash(buyPanelButton, Color.red));
            return;
        }
        
        // Create previews
        previews = new GameObject[previewPositions.Count];
        for (int i = 0; i < previewPositions.Count; i++)
        {
            if (previewPositions[i] == null) continue;
            
            previews[i] = Instantiate(panelPrefab, previewPositions[i], Quaternion.identity);
            if (previews[i] == null) continue;
            
            int previewLayer = LayerMask.NameToLayer("PreviewPanel");
            if (previewLayer >= 0) 
                SetLayerRecursively(previews[i], previewLayer);
                
            SetColor(previews[i], previewColor);
            SetFullCollider(previews[i]);
        }
        
        placingPanel = true;
        hovered = -1;
        StartCoroutine(MakeButtonFlash(buyPanelButton, Color.green));
    }

    Vector3 GetPanelAttachPosition(string side)
    {
        int panelSize = 4;
        float cellSize = 1f;
        float panelWorldSize = panelSize * cellSize;
        Vector3 attachPosition = Vector3.zero;
        if (panels.Count == 0) return Vector3.zero;
        GameObject targetPanel = panels[0];
        float extreme = 0;
        switch (side.ToLower())
        {
            case "a":
                extreme = panels[0].transform.position.y;
                foreach (var p in panels) if (p.transform.position.y > extreme) { extreme = p.transform.position.y; targetPanel = p; }
                attachPosition = targetPanel.transform.position + new Vector3(0, panelWorldSize, 0);
                break;
            case "b":
                extreme = panels[0].transform.position.y;
                foreach (var p in panels) if (p.transform.position.y < extreme) { extreme = p.transform.position.y; targetPanel = p; }
                attachPosition = targetPanel.transform.position + new Vector3(0, -panelWorldSize, 0);
                break;
            case "c":
                extreme = panels[0].transform.position.x;
                foreach (var p in panels) if (p.transform.position.x > extreme) { extreme = p.transform.position.x; targetPanel = p; }
                attachPosition = targetPanel.transform.position + new Vector3(panelWorldSize, 0, 0);
                break;
            case "d":
                extreme = panels[0].transform.position.x;
                foreach (var p in panels) if (p.transform.position.x < extreme) { extreme = p.transform.position.x; targetPanel = p; }
                attachPosition = targetPanel.transform.position + new Vector3(-panelWorldSize, 0, 0);
                break;
        }
        return attachPosition;
    }

    void Update()
    {
        if (!placingPanel || previews[0] == null) return;
        
        hovered = -1;
        for (int i = 0; i < previews.Length; i++)
        {
            if (previews[i] != null && IsMouseOver(previews[i]))
            {
                hovered = i;
                break;
            }
        }
        
        for (int i = 0; i < previews.Length; i++)
        {
            if (previews[i] != null)
            {
                SetColor(previews[i], i == hovered ? originalColor : previewColor);
            }
        }
        
        if (hovered != -1 && Input.GetMouseButtonDown(0))
        {
            var p = previews[hovered];
            if (p != null)
            {
                var newPanel = Instantiate(panelPrefab, p.transform.position, p.transform.rotation);
                panels.Add(newPanel);

                foreach (var cell in newPanel.GetComponentsInChildren<GridCell>())
                {
                    cell.SetOccupied(false);
                }

                SetFullCollider(newPanel);
                
                GameManager.Instance.money -= panelPrice;
                
                StartCoroutine(MakeButtonFlash(buyPanelButton, Color.green));
            }
            
            foreach (var x in previews) 
            {
                if (x != null) 
                    Destroy(x);
            }
            
            placingPanel = false;
            previews = new GameObject[4];
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            foreach (var x in previews) 
            {
                if (x != null) 
                    Destroy(x);
            }
            placingPanel = false; 
            previews = new GameObject[4];
        }
    }

    Vector3 GetPanelSnapPosition(Vector3 mouseWorldPos)
    {
        if (panels.Count == 0) return Vector3.zero;
        float half = 2f; 
        Vector3 best = panels[0].transform.position + Vector3.forward * half;
        float min = (mouseWorldPos - best).sqrMagnitude;
        foreach (var p in panels)
        {
            Vector3[] sides = {
                p.transform.position + Vector3.forward * half,
                p.transform.position - Vector3.forward * half,
                p.transform.position + Vector3.right * half,
                p.transform.position - Vector3.right * half
            };
            foreach (var s in sides)
            {
                float dist = (mouseWorldPos - s).sqrMagnitude;
                if (dist < min) { min = dist; best = s; }
            }
        }
        return best;
    }

    private bool CanAttachPanel(Vector3 pos)
    {
        float minDist = 0.1f;
        foreach (var p in panels)
        {
            if (Vector3.Distance(p.transform.position, pos) < minDist + 0.5f) 
                return false;
        }
        return true;
    }

    Bounds GetBounds(GameObject prefab) {
        var r = prefab.GetComponentsInChildren<MeshRenderer>();
        if (r.Length == 0) return new Bounds(prefab.transform.position, Vector3.one);
        var b = r[0].bounds; for (int i = 1; i < r.Length; i++) b.Encapsulate(r[i].bounds);
        return new Bounds(Vector3.zero, b.size);
    }
    void SetColor(GameObject g, Color c) { foreach (var r in g.GetComponentsInChildren<MeshRenderer>()) r.material.color = c; }
    void SetFullCollider(GameObject g)
    {
        foreach (var oldCollider in g.GetComponents<Collider>()) Destroy(oldCollider);

        var renderers = g.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0)
        {
            g.AddComponent<BoxCollider>();
            return;
        }

        Bounds worldBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) worldBounds.Encapsulate(renderers[i].bounds);

        var box = g.AddComponent<BoxCollider>();
        box.center = g.transform.InverseTransformPoint(worldBounds.center);

        Vector3 localScale = g.transform.localScale;
        Vector3 colliderSize;

        colliderSize.x = localScale.x == 0 ? 0 : worldBounds.size.x / localScale.x;
        colliderSize.y = localScale.y == 0 ? 0 : worldBounds.size.y / localScale.y;
        colliderSize.z = localScale.z == 0 ? 0 : worldBounds.size.z / localScale.z;
        
        box.size = new Vector3(Mathf.Abs(colliderSize.x), Mathf.Abs(colliderSize.y), Mathf.Abs(colliderSize.z));
    }

void SetLayerRecursively(GameObject obj, int layer)
{
    obj.layer = layer;
    foreach (Transform child in obj.transform)
        SetLayerRecursively(child.gameObject, layer);
}

    bool IsMouseOver(GameObject g)
    {
        int previewLayer = LayerMask.NameToLayer("PreviewPanel");
        int layerMask = previewLayer >= 0 ? (1 << previewLayer) : Physics.DefaultRaycastLayers;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, layerMask))
        {
            foreach (var col in g.GetComponentsInChildren<Collider>())
                if (hit.collider == col)
                    return true;
        }
        return false;
    }


    public int GetHousePrice(GameObject housePrefab)
    {
        foreach (var houseData in houses)
        {
            if (houseData.housePrefab == housePrefab)
            {
                return houseData.price;
            }
        }
        return 0;
    }
    private void OnBuyClicked(HouseData houseData)
    {
        if (GameManager.Instance.isHouseBeingPlaced)
        {
            return;
        }

        var houseManager = FindObjectOfType<HouseManager>();
        if (houseManager == null) return;

        var requirement = houseManager.GetHouseRequirement(houseData.housePrefab);
        if (requirement == null) return;

        bool hasCitizens = GameManager.Instance.totalCitizens >= requirement.requiredCitizens;
        bool hasEnergy = GameManager.Instance.totalEnergy >= requirement.requiredEnergy;
        bool hasMoney = GameManager.Instance.money >= houseData.price;

        if (hasCitizens && hasEnergy && hasMoney)
        {
            StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.green));
            GameManager.Instance.totalCitizens -= requirement.requiredCitizens;
            GameManager.Instance.totalEnergy -= requirement.requiredEnergy;
            GameManager.Instance.SpawnHouse(houseData.housePrefab);
        }
        else
        {
            StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.red));
            GameManager.Instance.ShowUpgradeWarning(!hasCitizens, !hasEnergy, !hasMoney);
        }
    }

    private IEnumerator MakeButtonFlash(Button button, Color flashColor)
    {
        button.targetGraphic.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        button.targetGraphic.color = Color.white;
    }
}