using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public GameObject parts;
    public GameObject spark;   
    public AudioSource aS;
    private float ExplosionForse = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Knif")
        {
            aS.Play();
            GetComponentInParent<TimberController>().timberParts.Remove(GetComponent<Rigidbody2D>());
            GetComponentInParent<TimberController>().gameController.saveData.AppleCout += 2;
            GetComponentInParent<TimberController>().gameController.saveData.Save();
            transform.SetParent(null);
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            parts.SetActive(true);
            foreach (var item in parts.transform.GetComponentsInChildren<Rigidbody2D>())
            {
                item.isKinematic = false;
                item.AddRelativeForce(new Vector2(0, ExplosionForse), ForceMode2D.Impulse);
                item.freezeRotation = false;
                item.AddTorque(Random.Range(-300f, 300f), 0f);
            }           
            Destroy(Instantiate(spark, new Vector3(0,-1.5f,0), Quaternion.identity), 0.2f); //0.117 время анимации Spark
            Destroy(gameObject, 1f);
        }
    }
}
