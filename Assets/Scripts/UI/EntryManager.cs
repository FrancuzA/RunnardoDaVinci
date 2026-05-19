using TMPro;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;
    public void SetData(string name, int score)
    {
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
