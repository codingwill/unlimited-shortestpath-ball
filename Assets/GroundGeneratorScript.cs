using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundGeneratorScript : MonoBehaviour
{
    //Vars and Objects
    public GameObject groundPrefab;
    public GameObject diamondPrefab;
    public TextMeshProUGUI levelText;
    public int collectibleCount;
    public int xCount;
    public int zCount;
    public float cubeLength;
    bool[] hasDiamond;
    int deckCount;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (PlayerPrefs.GetInt("xCount", 0) != 0)
        {
            xCount = PlayerPrefs.GetInt("xCount");
        }
        if (PlayerPrefs.GetInt("zCount", 0) != 0)
        {
            zCount = PlayerPrefs.GetInt("zCount");
        }
        if (PlayerPrefs.GetInt("diamondCount", 1) != 1)
        {
            collectibleCount = PlayerPrefs.GetInt("diamondCount");
        }
        */
        if (PlayerController.xCountLatest > xCount)
        {
            xCount = PlayerController.xCountLatest;
        }
        if (PlayerController.zCountLatest > zCount)
        {
            zCount = PlayerController.zCountLatest;
        }
        if (PlayerController.diamondCountLatest > collectibleCount)
        {
            collectibleCount = PlayerController.diamondCountLatest;
        }
        
        levelText.text = "" + (xCount - 1);
        deckCount = (2 * xCount - 1) * (2 * zCount - 1);
        hasDiamond = new bool[deckCount];
        //Inisialisasi collectibleCount diamond jadi true
        for (int i = 0; i < collectibleCount; ++i)
        {
            hasDiamond[i] = true;
        }
        //shuffle diamond location
        shuffle();


        //Generate block dari tengah, mengisi block x dan z beserta cerminannya
        for (int x = 0; x < xCount; ++x)
        {
            for (int z = 0; z < zCount; ++z)
            {
                //position deck
                float posX = cubeLength * x;
                float posY = 0f;
                float posZ = cubeLength * z;

                //instansiasi deck/ground
                if (x == 0 && z == 0)
                {
                    Instantiate(groundPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                }
                else if (x == 0 && z > 0)
                {
                    Instantiate(groundPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                    Instantiate(groundPrefab, new Vector3(posX, posY, -posZ), Quaternion.identity);
                }
                else if (x > 0 && z == 0)
                {
                    Instantiate(groundPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                    Instantiate(groundPrefab, new Vector3(-posX, posY, posZ), Quaternion.identity);
                }
                else if (x > 0 && z > 0)
                {
                    Instantiate(groundPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                    Instantiate(groundPrefab, new Vector3(-posX, posY, posZ), Quaternion.identity);
                    Instantiate(groundPrefab, new Vector3(posX, posY, -posZ), Quaternion.identity);
                    Instantiate(groundPrefab, new Vector3(-posX, posY, -posZ), Quaternion.identity);
                }

            }
        }

        for (int x = -(xCount-1); x < xCount; ++x)
        {
            for (int z = -(zCount-1); z < zCount; ++z)
            {
                //nomor urut petak
                int row = x + (xCount-1);
                int col = z + (zCount-1) + 1;
                int deckNumber = (row * (2*zCount - 1) + col) - 1;
                Debug.Log("Deck Number: " + deckNumber);

                float posX = cubeLength * x;
                float posY = 0.5f;
                float posZ = cubeLength * z;

                //instansiasi diamond
                if (hasDiamond[deckNumber])
                {
                    Instantiate(diamondPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                }
            }
        }
    }

    //Fisher and Yates Algorithm
    void shuffle()
    {
        for (int i = 0; i < deckCount-1; ++i)
        {
            int j = Random.Range(i, deckCount);
            bool temp = hasDiamond[i];
            hasDiamond[i] = hasDiamond[j];
            hasDiamond[j] = temp;
        }
    }

    public void setX(int value)
    {
        xCount = value;
    }
    public void setZ(int value)
    {
        zCount = value;
    }
    public void setDiamond(int value)
    {
        collectibleCount = value;
    }
    public int getX()
    {
        return xCount;
    }
    public int getZ()
    {
        return zCount;
    }
    public int getDiamond()
    {
        return collectibleCount;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
