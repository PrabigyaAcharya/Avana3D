
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public Stat damage;
    public int maxHealth = 100;
    public int currentHelath { get; private set; }

    public event System.Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHelath = maxHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHelath -= damage;

        if(OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHelath);
        }

        if (currentHelath <= 0) 
        {
            Die();   
        }

    }

    public virtual void Respawn()
    {
        currentHelath = maxHealth;
    }

    public virtual void Die()
    {

    }
}
