using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;

    private void Start()
    {


        int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);


        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelsUnlocked)
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void SelectLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void ResetUnlockedLevels()
    {
        PlayerPrefs.SetInt("levelsUnlocked", 1);
        PlayerPrefs.Save();

    }
    // Level geçildiğinde bu fonksiyonu çağır
    public static void UnlockNextLevel()
    {
        Debug.Log("Sonraki bölüm açıldı");
        int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);
        levelsUnlocked++;
        PlayerPrefs.SetInt("levelsUnlocked", levelsUnlocked);
        PlayerPrefs.Save();
    }
}
