using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnPlayButtonClick);
    }

    void OnPlayButtonClick()
    {
        SceneManager.LoadScene(1);
    }
}


