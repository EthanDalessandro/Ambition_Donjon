using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : MonoBehaviour
{
    public PlayerController player;
    public Slider mainSlider;
    public void Start()
    {
        mainSlider.value = player.health / player.maxHealth;
    }
    public void UpdateHealth()
    {
        mainSlider.value = player.health / player.maxHealth;
    }
}
