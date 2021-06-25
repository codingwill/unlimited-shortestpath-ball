using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Vars and Objects q
    public float speed = 0f;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI pathText;
    public TextMeshProUGUI bestText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject continueTextObject;
    public GameObject diamondGeneratorObject;
    public AudioClip pickUp;
    public AudioClip gameOver;


    public InputAction nextButton;

    private static int count = 0;
    private static int path = 0;
    private Rigidbody rb;
    private float moveX, moveY;
    public float jumpPower;
    public InputActionMap control;
    private int diamondCount;
    private int lastCount;
    public bool win;
    public bool lose;

    public static int xCountLatest;
    public static int zCountLatest;
    public static int diamondCountLatest;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("xCount", 2);
        //PlayerPrefs.SetInt("zCount", 2);
        //PlayerPrefs.SetInt("diamondCount", 5);
        lastCount = count;
        rb = GetComponent<Rigidbody>();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        continueTextObject.SetActive(false);
        diamondCount = diamondGeneratorObject.GetComponent<GroundGeneratorScript>().collectibleCount;
        SetCountText();
    }

    private void Awake()
    {

        win = false;
        lose = false;
        nextButton.performed += ctx =>
        {
            Debug.Log("NextButton: " + ctx.ReadValueAsObject());
            if (win)
            {
                Debug.Log("Next: " + ctx.ReadValueAsObject());
                Debug.Log("Kok win?");
                win = false;
                xCountLatest = diamondGeneratorObject.GetComponent<GroundGeneratorScript>().getX() + 1;
                zCountLatest = diamondGeneratorObject.GetComponent<GroundGeneratorScript>().getZ() + 1;
                diamondCountLatest = diamondGeneratorObject.GetComponent<GroundGeneratorScript>().getDiamond() + 5;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1f;
            }
            if (lose)
            {
                //set highscore
                if (count == PlayerPrefs.GetInt("bestScore", 0) && path < PlayerPrefs.GetInt("bestPath", System.Int32.MaxValue))
                {
                    PlayerPrefs.SetInt("bestScore", count);
                    PlayerPrefs.SetInt("bestPath", path);
                }
                else if (count > PlayerPrefs.GetInt("bestScore", 0))
                {
                    PlayerPrefs.SetInt("bestScore", count);
                    PlayerPrefs.SetInt("bestPath", path);
                }
                
                Debug.Log("Kok lose?");
                xCountLatest = 0;
                zCountLatest = 0;
                diamondCountLatest = 0;
                path = 0;
                count = 0;
                Debug.Log("Path reset");
                Time.timeScale = 1f;
                lose = false;
                

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        };
    }

    private void OnEnable()
    {
        nextButton.Enable();
    }

    private void OnDisable()
    {
        nextButton.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(win);
        if (win || lose)
        {

            continueTextObject.SetActive(true);
            Time.timeScale = 0;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(pickUp);
            count++;
        }
        SetCountText();

        if (other.gameObject.CompareTag("Jurang"))
        {
            loseTextObject.SetActive(true);
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(gameOver);
            lose = true;
        }
    }

    public void addPath()
    {
        path += 1;
        Debug.Log("Paths: " + path);
    }
    void SetCountText()
    {
        countText.text = "" + count.ToString();
        pathText.text = "" + path.ToString();
        bestText.text = "" + PlayerPrefs.GetInt("bestScore", 0) + " points, " + PlayerPrefs.GetInt("bestPath", System.Int32.MaxValue) + " paths";
        if (count == diamondCount + lastCount)
        {
            Debug.Log("has reached the number.");
            winTextObject.SetActive(true);
            win = true;
        }
    }

    void OnMove(InputValue moveVal)
    {
        Vector2 moveVec = moveVal.Get<Vector2>();
        moveX = moveVec.x;
        moveY = moveVec.y;
    }

    void Jump()
    {
        Vector3 jumpVector = new Vector3(0f, jumpPower, 0f);
        rb.AddForce(jumpVector);
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(move * speed);
    }
}
