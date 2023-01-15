using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text rings;
    public Text lives;
    public PlayerCharacter player;

    public void Update()
    {
        rings.text = player.ringCount.ToString();
        lives.text = player.lifeCount.ToString();
    }
}
