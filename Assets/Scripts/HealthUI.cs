using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterStat))]
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;
    Transform ui;
    Image healthSlider;

    float visibleTime = 10;
    float lastVisibleTime;

    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(false);
                break;
            }
        }
        GetComponent<CharacterStat>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if (ui == null)
        {
            return;
        } 

        ui.gameObject.SetActive(true);
        lastVisibleTime = Time.time;
        float healthPercent = (float)currentHealth/maxHealth;
        healthSlider.fillAmount= healthPercent;
        if(currentHealth <= 0)
        {
            Destroy(ui.gameObject);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ui == null)
        {
            return;
        }
        ui.position = target.position;
        ui.forward = -cam.forward;
        if (Time.time -lastVisibleTime > visibleTime)
        {
            ui.gameObject.SetActive(false);
        }
    }
}
