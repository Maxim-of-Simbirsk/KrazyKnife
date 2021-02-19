using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimberController : MonoBehaviour
{
    public GameObject deathTimber;
    public GameObject BossParts;
    private Quaternion Rotation;
    private SpriteRenderer sR;
    public AudioSource aS;
    [HideInInspector] public List<Rigidbody2D> timberParts = new List<Rigidbody2D>();
    private float ExplosionForse = 2f;
    private bool alive = true;
    public bool defet = false;
    [HideInInspector] public GameController gameController;
    private void Awake()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        if(BossParts != null) timberParts.AddRange(BossParts.transform.GetComponentsInChildren<Rigidbody2D>());
        timberParts.AddRange(deathTimber.transform.GetComponentsInChildren<Rigidbody2D>());
    }
    void Update()
    {
        if (alive && !defet) RotationTimber();
    }
    void RotationTimber()
    {
        Rotation.eulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * gameController.gameSetings.SpeedRotation;
        transform.rotation = Rotation;
    }
    public void OnHit(Rigidbody2D rigidbody2D)
    {
        StartCoroutine("HitenAnim");
        timberParts.Add(rigidbody2D);
        gameController.hitDetected();
    }
   public void Death()
    {
        aS.Play();
        alive = false;      
        sR.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        deathTimber.SetActive(true);
        foreach (var item in timberParts)
        {
            Destroy(item.GetComponent<PolygonCollider2D>());
            item.isKinematic = false;
            item.AddTorque(Random.Range(-300f, 300f), 0f);
            item.AddRelativeForce(new Vector2(0, -ExplosionForse), ForceMode2D.Impulse);
            item.freezeRotation = false; 
        }
        Destroy(gameObject, 1f);
    }
    private IEnumerator HitenAnim()
    {
        transform.position += new Vector3(0, 0.1f, 0);
        sR.color = new Color(1, 0.8f, 0.8f, 1);
        yield return new WaitForSeconds(0.03f);
        transform.position -= new Vector3(0, 0.1f, 0);
        sR.color = Color.white;
    }
    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }
}
