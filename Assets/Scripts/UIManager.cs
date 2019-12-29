using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider HealthBar;

    private void Start()
    {
        Debug.Log(HealthBar + " " + GameManager.Instance.Player);
        UpdateHealthBar();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void UpdateHealthBar()
    {
        HealthBar.value = CalculateLifePercent();
        HealthBar.GetComponentInChildren<Text>().text = GameManager.Instance.Player.GetVital((int)VitalName.Life).CurrentValue.ToString()
                                                        + " / " +
                                                        GameManager.Instance.Player.GetVital((int)VitalName.Life).AdjustedBaseValue.ToString();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private float CalculateLifePercent()
    {

        return (float)GameManager.Instance.Player.GetVital((int)VitalName.Life).CurrentValue /
                GameManager.Instance.Player.GetVital((int)VitalName.Life).AdjustedBaseValue;
    }
}
