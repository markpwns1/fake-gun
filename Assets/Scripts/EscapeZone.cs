using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    private CluesManager cm;
    private Health health;

    private GameObject winText;
    private GameObject dieText;
    private void Start()
    {
        cm = FindObjectOfType<CluesManager>();
        health = FindObjectOfType<Health>();

        winText = GameObject.FindGameObjectWithTag("Win");
        dieText = GameObject.FindGameObjectWithTag("Die");

        winText.SetActive(false);
        dieText.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && cm.CluesLeft() == 0)
        {
            Win();
        }
    }

    private void Update()
    {
        if (health.health <= 0)
        {
            Die();
        }
    }

    void Win() {
        winText.SetActive(true);
        winText.GetComponent<DieScene>().Die();
    }

    void Die() {
        dieText.SetActive(true);
        dieText.GetComponent<DieScene>().Die();
    }
}
