using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject breakEffect;

    public int health = 1;
    public bool destroy;

    [Space(10)]
    public UnityEvent onHit;
    public UnityEvent onBreak;

    public bool addsLife; // This controls whether the block adds a life

    private int currentHealth;

    private void Start()
    {
        currentHealth = health;
    }

    public void Hit()
    {
        if (hitEffect)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
        }

        currentHealth -= 1;
        onHit.Invoke();

        if (currentHealth <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        if (breakEffect)
        {
            Instantiate(breakEffect, transform.position, transform.rotation);
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

        // Add a life if 'addsLife' is true
        if (addsLife)
        {
            GameManager.instance.AddLife(1); // Add a life if this block is set to do so
           
        }
        else
        {
            GameManager.instance.AddBlock();
        }

        onBreak.Invoke();
    }

    private void Repair()
    {
        currentHealth = health;
        gameObject.SetActive(true);
    }

    public void AddCoin(int amount)
    {
        GameManager.instance.AddCoin(amount);
    }
}
