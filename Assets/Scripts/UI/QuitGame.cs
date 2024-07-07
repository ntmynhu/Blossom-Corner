using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        DataPersistenceManager.Instance.SaveGame();

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
