using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public GameObject scoreboardButton;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.G))
        {
            UnlockScoreboard();
        }
    }
    public void OpenGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenScoreboard()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void UnlockScoreboard()
    {
        scoreboardButton.SetActive(true);
    }
}
