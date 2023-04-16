using UnityEngine;

public class LoadAndSaveData : MonoBehaviour
{
    public static LoadAndSaveData instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("no LoadAndSaveData instance in the scene.");
            return;
        }

        instance = this;
    }

    void Start()
    {
        this.LoadData();
    }

    private void LoadData()
    {
        PlayerPrefs.SetInt("level", 0);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("level", 1);
    }
}
