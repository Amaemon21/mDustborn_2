using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Transform _uiSceneContainer;
    
    private void Awake()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        _loadingScreen.gameObject.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        _loadingScreen.gameObject.SetActive(false);
    }

    public void AttachSceneUI(GameObject sceneUi)
    {
        ClearSceneUI();
        
        sceneUi.transform.SetParent(_uiSceneContainer, false);
    }

    private void ClearSceneUI()
    {
        var childCount = _uiSceneContainer.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(_uiSceneContainer.GetChild(i).gameObject);
        }
    }
}