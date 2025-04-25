using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public int poin = 100;
    public int nyawa = 100;

    public TextMeshProUGUI nyawaText;
    public TextMeshProUGUI poinText;
    public TextMeshProUGUI timerText;

    public GameObject gameOverPanel;
    public TextMeshProUGUI skorAkhirText;
    public TextMeshProUGUI nyawaAkhirText;

    public float waktuTersisa = 60f;
    private bool gameBerakhir = false;

    private bool bisaTrigger = false;
    private Light[] pointLights; // hanya menyimpan light yang tipe-nya Point


    IEnumerator DelayTrigger()
    {
        yield return new WaitForSeconds(1f);
        bisaTrigger = true;
    }

    void Start()
    {
        poin = 0;
        nyawa = 100;
        UpdateUI();
        gameOverPanel.SetActive(false);
        StartCoroutine(DelayTrigger());

        // Ambil semua lampu di scene
        Light[] semuaLampu = FindObjectsOfType<Light>();

        // Filter: hanya Point Light
        List<Light> hanyaPoint = new List<Light>();
        foreach (Light lampu in semuaLampu)
        {
            if (lampu.type == LightType.Point)
            {
                lampu.enabled = false; // matikan
                hanyaPoint.Add(lampu);
            }
        }
        pointLights = hanyaPoint.ToArray();
    }

    void Update()
    {
        if (!gameBerakhir)
        {
            waktuTersisa -= Time.deltaTime;
            timerText.text = "Waktu: " + Mathf.CeilToInt(waktuTersisa).ToString();

            if (waktuTersisa <= 0)
            {
                waktuTersisa = 0;
                GameOver();
            }
        }

        // Debug key (opsional)
        if (Input.GetKeyDown(KeyCode.N)) KurangiNyawa(10);
        if (Input.GetKeyDown(KeyCode.P)) TambahPoin(5);
    }

    public void KurangiNyawa(int jumlah)
    {
        nyawa -= jumlah;
        nyawa = Mathf.Clamp(nyawa, 0, 100);
        UpdateUI();
    }

    public void TambahPoin(int jumlah)
    {
        poin += jumlah;
        UpdateUI();
    }

    void NyalakanSemuaPointLight()
    {
        foreach (Light lampu in pointLights)
        {
            lampu.enabled = true;
        }
    }

    void MatikanSemuaPointLight()
    {
        foreach (Light lampu in pointLights)
        {
            lampu.enabled = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (!bisaTrigger) return;

        if (other.CompareTag("Bahaya"))
        {
            KurangiNyawa(20);
            MatikanSemuaPointLight();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Health"))
        {
            TambahNyawa(10);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("merah"))
        {
            TambahPoin(20);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("kuning"))
        {
            TambahPoin(30);
            NyalakanSemuaPointLight();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("biru"))
        {
            TambahPoin(10);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("hijau"))
        {
            TambahPoin(100);
            Destroy(other.gameObject);
        }
    }

    public void TambahNyawa(int jumlah)
    {
        nyawa += jumlah;
        nyawa = Mathf.Clamp(nyawa, 0, 100);
        UpdateUI();
    }

    void UpdateUI()
    {
        nyawaText.text = "Nyawa: " + nyawa;
        poinText.text = "Poin: " + poin;
    }

    void GameOver()
    {
        gameBerakhir = true;
        Time.timeScale = 0f; // Pause game
        gameOverPanel.SetActive(true);
        skorAkhirText.text = "Skor: " + poin;
        nyawaAkhirText.text = "Nyawa: " + nyawa;
    }

    // Fungsi tombol
    public void MainLagi()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KeluarGame()
    {
        Application.Quit();
    }
}
