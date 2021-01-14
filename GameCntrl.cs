using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class GameCntrl : MonoBehaviour
{
    public GameObject pLost;
    public GameObject colBlock;
    public Vector3[] positions;
    private GameObject block;
    private GameObject[] blocks = new GameObject[4];

    private int rand, count;
    private float rCol, gCol, bCol;
    public Text score;
    private static Color aColor;

    [HideInInspector]
    public bool next, lose;
    private static int advCount = 0;

    void Start()
    {
       
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("3973649", false);
        }
        else
            Debug.Log("Platform is not supported");

        count = 0;
        next = false;
        lose = false;
        rand = Random.Range(0, positions.Length);
        for (int i = 0; i < positions.Length; i++)
        {
            blocks[i] = Instantiate(colBlock, positions[i], Quaternion.identity) as GameObject;
            if (rand == i)
                block = blocks[i];
        }
        block.GetComponent<RandCol>().right = true;
    }

    void Update()
    {
        if (lose)
        {
            if (!pLost.activeSelf)
            {
                playerLose();
            }
        }
        else if (next)
        {
            nextColors();
        }
    }

    void nextColors()
    {
        if (PlayerPrefs.GetString("Music") != "no")
            GetComponent<AudioSource>().Play();
        count++;
        score.text = count.ToString();
        aColor = new Vector4(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), 1);
        GetComponent<Renderer>().material.color = aColor;
        next = false;

        if (count < 3)
        {
            rCol = 0.2f;
            gCol = 0.2f;
            bCol = 0.2f;
        }
        else if (count >= 3 && count < 5)
        {
            rCol = 0.1f;
            gCol = 0.1f;
            bCol = 0f;
        }
        else if (count >= 5)
        {
            rCol = 0f;
            gCol = 0f;
            bCol = 0.05f;
        }

        // New colors for blocks
        rand = Random.Range(0, positions.Length);
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == rand)
                blocks[i].GetComponent<Renderer>().material.color = aColor;
            else
            {
                float r = aColor.r + Random.Range(-rCol, rCol) > 1f ? 1f : aColor.r + Random.Range(-rCol, rCol);
                float g = aColor.g + Random.Range(-gCol, gCol) > 1f ? 1f : aColor.g + Random.Range(-gCol, gCol);
                float b = aColor.b + Random.Range(-bCol, bCol) > 1f ? 1f : aColor.b + Random.Range(-bCol, bCol);
                blocks[i].GetComponent<Renderer>().material.color = new Vector4(r, g, b, aColor.a);
            }
        }
    }

    void playerLose()
    {
        
 if (Advertisement.IsReady() && advCount %3 ==0)
 Advertisement.Show();
        advCount++;
if (PlayerPrefs.GetInt ("Score") < count)
        PlayerPrefs.SetInt("Score", count);
        pLost.SetActive(true);
        if (PlayerPrefs.GetString("Music") == "no")
            pLost.GetComponent<AudioSource>().mute = true;
        print("Player lose");
    }
}