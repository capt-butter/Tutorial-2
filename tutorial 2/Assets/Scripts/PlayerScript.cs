using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    private int scoreValue = 0;

    public int lives = 3;
    public Text livestxt;

    public Text winmsg;
    public Text losemsg;
    //audio stuff
    public AudioSource musicSource;
    public AudioClip background;
    public AudioClip win;
    //animation stuff
    Animator anim;
    private bool isright = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public bool won = false;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        livestxt.text = "lives: " + lives.ToString();
        winmsg.text = "";
        losemsg.text = "";
        anim = GetComponent<Animator>();

        //starts up audio
        musicSource.clip = background;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
        
        else if (isright == true && hozMovement < 0)
        {
            Flip();
        }
        if (isright == false && hozMovement > 0)
        {
            Flip();
        }
        if (hozMovement != 0)
        {
            anim.SetInteger("state", 1);
        }
        else if (hozMovement == 0 && vertMovement == 0)
        {
            anim.SetInteger("state", 0);
        }
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);      

    }
    //collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player hits a coin this happens
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            //checks if at score to move to 2nd lvl
            if (scoreValue == 4)
            {
                transform.position = new Vector2(85.0f, 5.0f);
                lives = 3;
                livestxt.text = "lives: " + lives.ToString();
            }
            if (scoreValue == 8 && won == false)
            {
                won = true;
                winmsg.text = "YOU WON, MADE BY ETHAN SYMONDS";
                musicSource.Stop();
                musicSource.PlayOneShot(win, 1.0f);
                musicSource.clip = background;
                musicSource.Play();

            }
        }
        //if player hits baddie this happens
        if (collision.collider.tag == "enemy")
        {
            lives = lives - 1;
            livestxt.text = "lives: " + lives.ToString();
            Destroy(collision.collider.gameObject);
            if (lives == 0)
            {
                losemsg.text = "you lost, try again";
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("state", 2);
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                anim.SetInteger("state", 2);
            }
        }
    }
    void Flip()
    {
        isright = !isright;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}