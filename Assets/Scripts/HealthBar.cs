using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health health;
    private GameObject healthBar;
    void Start()
    {
        health = FindObjectOfType<Health>();
        healthBar = GameObject.FindGameObjectWithTag("Health");
    }
    void Update()
    {
        healthBar.transform.localScale = new Vector3(Mathf.Max((float)health.health / (float)health.maxHealth, 0.0f), 0.1f, 1.0f);
    }
}
