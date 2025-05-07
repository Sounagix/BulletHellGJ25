using UnityEngine;

public class ServedFood : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private FoodType _foodType;

    public void SetUp(ThroweableFood throweableFood,Vector2 dir, float throwSpeed)
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        GetComponent<Rigidbody2D>().AddForce(dir * throwSpeed);
        _foodType = throweableFood._fOOD_tYPE;
        _spriteRenderer.sprite = throweableFood._sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CustomerController cC = collision.GetComponent<CustomerController>();
        if (cC != null)
        {
            cC.GetServed(_foodType);
            Destroy(gameObject);
        }
    }
}
