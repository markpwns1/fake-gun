using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hurtFade = 1.0f;
    public int maxHealth = 100;
    public int health = 100;

    public Image hurt;

    void Update() {
        hurt.color = new Color(hurt.color.r, hurt.color.g, hurt.color.b, Mathf.Lerp(hurt.color.a, 0.0f, hurtFade * Time.deltaTime));
    }

    public void Hurt() {
        hurt.color = new Color(hurt.color.r, hurt.color.g, hurt.color.b, 1.0f);
    }

}
