using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameController : MonoBehaviour
{
    public GameObject timber;
    public GameObject timberBoss;
    public GameObject knif;
    public GameObject Iconknif;
    public GameObject apple;
    public Setings gameSetings;
    public Interface interfaceUI;
    public AudioSource aS;
    public SaveData saveData;
    private GameObject carrentKnif;
    private GameObject carrentIconKnif;
    float knifImpulsForse = 5f;
    private Rigidbody2D carrentKnif_rB;
    [HideInInspector] public int starnKnifCount = 5;
    [HideInInspector] public int carrentKnifCount;
    [HideInInspector] public int hitCount = 0;
    [HideInInspector] public int progresStage = 1; // 1 стартовое значение
    private GameObject carrentTimber;
    private float timer;
    private bool canShot;

    private void Awake()
    {

    }
    void Start()
    {
        carrentIconKnif = Instantiate(Iconknif, new Vector2(0, -3f), Quaternion.identity);
        carrentIconKnif.SetActive(false);
        Vibration.Init();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) ShotKnife();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) ShotKnife();
        }
    }
    public void StartStage()
    {
        canShot = true;
        carrentIconKnif.SetActive(true);
        carrentKnifCount = starnKnifCount;
        if (progresStage % 5 == 0)
        {
            StartCoroutine("BossStart");
        }
        else
        {
            carrentTimber = Instantiate(timber, new Vector2(0, 0), Quaternion.identity);
            carrentTimber.GetComponent<TimberController>().SetGameController(this);
            AddRendomParts();
        }

        interfaceUI.SetGameUI();
    }
    public IEnumerator BossStart()
    {
        canShot = false;
        interfaceUI.bossFaght.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        interfaceUI.bossFaght.SetActive(false);
        carrentTimber = Instantiate(timberBoss, new Vector2(0, 0), Quaternion.identity);
        carrentTimber.GetComponent<TimberController>().SetGameController(this);
        canShot = true;
    }
    public IEnumerator ChendeStage()
    {
        yield return new WaitForSeconds(1f);
        if (starnKnifCount < 20) starnKnifCount += 1;
        progresStage += 1;
        StartStage();
    }
    public IEnumerator DefeatSlowMo()
    {
        canShot = false;
        if (saveData.vbration) Vibration.VibratePeek();
        carrentTimber.GetComponent<TimberController>().defet = true;
       // Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1f;
        Defeat();
        VibroOn();
    }
    public void ShotKnife()
    {
        if (carrentKnifCount > 0 && CanShot() && carrentTimber != null && canShot)
        {
            carrentIconKnif.SetActive(false);
            aS.Play();
            timer = Time.time + gameSetings.ShotDely;
            AddKnif();
            carrentKnif_rB.gravityScale = 1;
            carrentKnif_rB.AddForce(new Vector2(0, knifImpulsForse), ForceMode2D.Impulse);
            carrentKnifCount -= 1;
            interfaceUI.knifCountBar.SpendKnif();
            Debug.Log("------------------");
        }
    }
    private bool CanShot()
    {
        return Time.time > timer;
    }
    void AddKnif()
    {
        if (canShot)
        {
            carrentKnif = Instantiate(knif, new Vector2(0, -3f), Quaternion.identity);
            carrentKnif_rB = carrentKnif.GetComponent<Rigidbody2D>();
            carrentKnif.GetComponent<KnifController>().SetGameController(this);
        }
    }
    void AddRendomParts()
    {
        int sectorCount = 12; // количество секторов в окружности, выше 12 возможны пересечения с яблоком выше 36 между ножами
        int cauntRendomKnif = 0;
        List<GameObject> randomParts = new List<GameObject>();
        if (progresStage > 2) cauntRendomKnif = Random.Range(1, gameSetings.maxSpawnKnif);
        else if (progresStage == 2) cauntRendomKnif = 1;
        for (int i = 0; i < cauntRendomKnif; i++)
            randomParts.Add(knif);
        if (Random.Range(0, 100) <= gameSetings.chanceSpawnApple) randomParts.Add(apple);

        List<int> tekenSector = new List<int>();
        foreach (var item in randomParts)
        {
            int sector;
            while (true) // генерациюслучайного сектора на окружности
            {
                sector = Random.Range(0, sectorCount);
                if (tekenSector.Where(x => x == sector).Any()) continue;
                tekenSector.Add(sector);
                break;
            }
            GameObject carrentpart = Instantiate(item, carrentTimber.transform.position, Quaternion.Euler(0f, 0f, (float)sector * (360 / sectorCount)));
            carrentpart.transform.SetParent(carrentTimber.transform);
            Rigidbody2D carrentpart_rB = carrentpart.GetComponent<Rigidbody2D>();
            carrentTimber.GetComponent<TimberController>().timberParts.Add(carrentpart_rB);
            if (carrentpart.tag == "Knif")
            {
                carrentpart_rB.velocity = Vector2.zero;
                carrentpart_rB.isKinematic = true;
                Destroy(carrentpart.GetComponent<KnifController>());
            }
        }
    }
    public void hitDetected()
    {
        if (saveData.vbration) Vibration.VibratePop();
        if (carrentKnifCount <= 0)
        {
            carrentTimber.GetComponent<TimberController>().Death();
            StartCoroutine("ChendeStage");
        }
        else carrentIconKnif.SetActive(true);
        hitCount++;
        interfaceUI.hitCountText.text = System.Convert.ToString(hitCount);
        interfaceUI.appleCountText.text = System.Convert.ToString(saveData.AppleCout);
    }
    void Defeat()
    {
        if (hitCount > saveData.maxHit && progresStage > saveData.maxStage)
        {
            saveData.maxHit = hitCount;
            saveData.maxStage = progresStage;
            saveData.Save();
        }
        interfaceUI.SetDefetMenu();
        Destroy(carrentTimber);
        Destroy(carrentKnif);
        hitCount = 0;
        progresStage = 1;
        starnKnifCount = 5;
        Debug.Log("DEAD");
    }
    void VibroOn()
    {
       if(saveData.vbration) Handheld.Vibrate();
    }

}
