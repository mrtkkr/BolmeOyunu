using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{

    [SerializeField] private GameObject karePrefab;

    [SerializeField] private Transform karelerPaneli;

    [SerializeField]
    private TextMeshProUGUI soruText;

    private GameObject[] karelerDizisi= new GameObject[25];

    [SerializeField] private Transform soruPaneli;

    [SerializeField] private Sprite[] kareSprites;

    List<int> bolumDegerleriListesi = new List<int>();

    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int dogruSonuc;
    int butonDegeri;

    bool butonaBasilsinmi;
    int kalanHak;
    string sorununZorlukDerecesi;

    kalanHaklarManager kalanHaklarManager;
    PuanManager puanManager;

    GameObject gecerliKare;

    [SerializeField] private GameObject sonucPaneli;

    [SerializeField] AudioSource audioSource;

    public AudioClip butonSesi;


    private void Awake()
    {
        kalanHak = 3;

        audioSource= GetComponent<AudioSource>();

        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

        kalanHaklarManager = Object.FindObjectOfType<kalanHaklarManager>();
        puanManager=Object.FindObjectOfType<PuanManager>();


        kalanHaklarManager.KalanHaklariKotnrolEt(kalanHak);
    }

    // Start is called before the first frame update
    void Start()
    {
        butonaBasilsinmi = false;
        soruPaneli.GetComponent<RectTransform>().localScale=Vector3.zero;
        kareleriOlustur();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void kareleriOlustur()
    {
        for(int i = 0;i<25;i++)
        {
            GameObject kare=Instantiate(karePrefab,karelerPaneli);
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0,kareSprites.Length)];
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            karelerDizisi[i] = kare;
        }
        BolumDegerleriniTexteYazdir();
        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc", 2f);
    }
    void ButonaBasildi()
    {
        if(butonaBasilsinmi == true)
        {
            audioSource.PlayOneShot(butonSesi);
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            SonucuKontrolEt();

        }

    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled=true;
            gecerliKare.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text= "";
            gecerliKare.transform.GetComponent<Button>().interactable=false;
            puanManager.puaniArtir(sorununZorlukDerecesi);

            bolumDegerleriListesi.RemoveAt(kacinciSoru);
            if(bolumDegerleriListesi.Count > 0)
            {
                SoruPaneliniAc();
            }
            else
            {
                OyunBitti();
            }

           
        }
        else
        {
            kalanHak--;
            kalanHaklarManager.KalanHaklariKotnrolEt(kalanHak);
        }

        if (kalanHak <= 0)
        {
            OyunBitti();
        }

    }

    void OyunBitti()
    {
        butonaBasilsinmi = false;
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }


    IEnumerator DoFadeRoutine()
    {
        foreach(var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
            yield return new WaitForSeconds(0.07f);
        }
    }

    void BolumDegerleriniTexteYazdir()
    {
        foreach (var kare in karelerDizisi)
        {
            if (kare.transform.childCount == 0)
            {
                Debug.LogWarning("No child object found for kare: " + kare.name);
                continue;
            }

            TextMeshProUGUI textMeshProComponent = kare.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (textMeshProComponent != null)
            {
                int rastgeleDeger = Random.Range(1, 13);
                bolumDegerleriListesi.Add(rastgeleDeger);
                textMeshProComponent.text = rastgeleDeger.ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshPro-Text component not found in child object of kare: " + kare.name);
            }
        }
    }


    void SoruPaneliniAc()
    {
        SoruyuSor();
        butonaBasilsinmi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);

        kacinciSoru=Random.Range(0,bolumDegerleriListesi.Count);

        dogruSonuc = bolumDegerleriListesi[kacinciSoru];

        bolunenSayi = bolenSayi * dogruSonuc;
        if (bolunenSayi <= 40)
        {
            sorununZorlukDerecesi = "kolay";
        }else if(bolunenSayi>40 && bolunenSayi <= 80)
        {
            sorununZorlukDerecesi = "orta";
        }
        else
        {
            sorununZorlukDerecesi = "zor";
        }
        soruText.text=bolunenSayi.ToString()+ " : " + bolenSayi.ToString();
    }

}
