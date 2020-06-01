using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    #region Health
    public Image healthBarFilled;
    public Text healthText;

    private float fillValue;
    #endregion

    void Start()
    {

    }

    void Update()
    {
        #region Health
        fillValue = GameController.Health;
        fillValue = fillValue / GameController.MaxHealth;

        healthBarFilled.fillAmount = fillValue;
        healthText.text = "Health: " + GameController.Health;
        #endregion
    }
}
