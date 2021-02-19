using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifController : MonoBehaviour
{
    public GameObject spark;
    public GameObject opilki;
    private GameController gameController;
    private Rigidbody2D rB;
    private bool hit = false;
    public AudioSource aS;
    public AudioClip acHit;
    public AudioClip acMiss;
    private void Awake()
    {
        rB = GetComponent<Rigidbody2D>();
    }
    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }
    private void Hit(Collision2D collision)
    {
        aS.PlayOneShot(acHit);
        Instantiate(opilki, collision.contacts[0].point, Quaternion.identity);  
        transform.position = collision.transform.position;
        transform.SetParent(collision.transform);
        rB.velocity = Vector2.zero;
        rB.isKinematic = true;
        collision.gameObject.GetComponent<TimberController>().OnHit(rB);
        Destroy(GetComponent<KnifController>());
        Debug.Log("hit");
    }
    private void Miss(Collision2D collision)
    {
        aS.PlayOneShot(acMiss);
        rB.velocity = Vector2.zero;
        rB.freezeRotation = false;
        rB.AddTorque(-300f, 0f);
        rB.AddForce(new Vector2(1, -2), ForceMode2D.Impulse);
        Destroy(Instantiate(spark, collision.contacts[0].point, Quaternion.identity), 0.117f); //0.117 время анимации Spark
        GetComponent<PolygonCollider2D>().enabled = false;
        gameController.StartCoroutine("DefeatSlowMo");
        Destroy(gameObject, 1f);
        Debug.Log("miss");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Target" && !hit)
            Hit(collision);
        else if (!hit && collision.gameObject.tag != "Apple")  
            Miss(collision);
       if (collision.gameObject.tag != "Apple") hit = true;
    }
}
