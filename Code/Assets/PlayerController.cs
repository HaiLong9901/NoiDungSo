/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    private Rigidbody2D myRigidbody;

    private Animator anim;
    private float moveHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponentInParent<Rigidbody2D>();
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else if (CrossPlatformInputManager.GetAxisRaw("Horizontal") > 0)
        {
            anim.SetBool("isRunning", false);
            transform.localScale = new Vector3(1, 1, 1);
            myRigidbody.velocity = new Vector3(moveHorizontal * speed, myRigidbody.velocity.y);
        }
        else if (CrossPlatformInputManager.GetAxisRaw("Horizontal")  0)
        {
            anim.SetBool("isRunning", false);
            transform.localScale = new Vector3(-1, 1, 1);
            myRigidbody.velocity = new Vector3(moveHorizontal * speed, myRigidbody.velocity.y);
        }
    }

}*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            animator.SetBool("isRunning", true);
        }
        if (Input.GetKey("s"))
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKey("j"))
        {
            animator.SetBool("isJumping", true);
        }
        if (Input.GetKey("k"))
        {
            animator.SetBool("isJumping", false);
        }
    }
}
*/


/*  Version1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Lấy input từ bàn phím hoặc các nguồn khác để xác định trạng thái của nhân vật
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool jumpInput = Input.GetKey("k");

        // Xác định giá trị của biến trạng thái
        isJumping = jumpInput;
        isRunning = Mathf.Abs(horizontalInput) > 0f || Mathf.Abs(verticalInput) > 0f;

        // Kiểm tra xem nhân vật có đang chạm đất không
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // Cập nhật trạng thái của Animator dựa trên biến trạng thái
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);

        if (isJumping)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
        }
        else if(isRunning)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
        }
        // Xoay mặt đối tượng theo hướng di chuyển
        if (verticalInput != 0f)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(verticalInput, 0f, 0f));
        }
        if (horizontalInput != 0f)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, horizontalInput));
        }

        // Di chuyển nhân vật dựa trên input ngang
        //Vector3 movement = new Vector3(verticalInput * moveSpeed * Time.deltaTime, 0f, horizontalInput * moveSpeed * Time.deltaTime);
        //transform.Translate(movement);
        if(verticalInput == 1)
        {
            Vector3 movement = new Vector3(0f, 0f, verticalInput * moveSpeed * Time.deltaTime);
            transform.Translate(movement);
        }
        else if (verticalInput == -1)
        {
            Vector3 movement = new Vector3(0f, 0f, -verticalInput * moveSpeed * Time.deltaTime);
            transform.Translate(movement);
        }
        else if (horizontalInput == 1)
        {
            Vector3 movement = new Vector3(0f, 0f, horizontalInput * moveSpeed * Time.deltaTime);
            transform.Translate(movement);
        }
        else if (horizontalInput == -1)
        {
            Vector3 movement = new Vector3(0f, 0f, -horizontalInput * moveSpeed * Time.deltaTime);
            transform.Translate(movement);
        }
    }
}
*/


//Code OK
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private bool isRunning;
    private bool isJumping;
    private Animator animator;

    public string[] keywords = new string[] { "forward", "down", "left", "right", "jump", "stop" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    protected PhraseRecognizer recognizer;
    protected string word = "";

    public AudioSource source;
    public AudioClip clip;
    public AudioClip clip2;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log(recognizer.IsRunning);
        }

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        Debug.Log(args.text);
    }

    private void Update()
    {

        // Lấy input từ bàn phím hoặc các nguồn khác để xác định trạng thái của nhân vật
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool jumpInput = Input.GetKey("j");

        // Xác định giá trị của biến trạng thái
        isJumping = jumpInput;
        isRunning = Mathf.Abs(horizontalInput) > 0f || Mathf.Abs(verticalInput) > 0f;

        if (isJumping) {
            source.PlayOneShot(clip);
        }

        if ((isJumping != false) || (isRunning != false))
        {
            word = "";
            // Cập nhật trạng thái của Animator dựa trên biến trạng thái
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);

            if (isJumping)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isRunning", false);
            }
            else if (isRunning)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isRunning", false);
            }
            // Xoay mặt đối tượng theo hướng di chuyển
            if (verticalInput != 0f)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(verticalInput, 0f, 0f));
                source.PlayOneShot(clip2);
            }
            if (horizontalInput != 0f)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, -horizontalInput));
                source.PlayOneShot(clip2);
            }

            if (verticalInput == 1)
            {
                Vector3 movement = new Vector3(0f, 0f, verticalInput * moveSpeed * Time.deltaTime);
                transform.Translate(movement);
                source.PlayOneShot(clip2);
            }
            else if (verticalInput == -1)
            {
                Vector3 movement = new Vector3(0f, 0f, -verticalInput * moveSpeed * Time.deltaTime);
                transform.Translate(movement);
                source.PlayOneShot(clip2);
            }
            else if (horizontalInput == 1)
            {
                Vector3 movement = new Vector3(0f, 0f, horizontalInput * moveSpeed * Time.deltaTime);
                transform.Translate(movement);
                source.PlayOneShot(clip2);
            }
            else if (horizontalInput == -1)
            {
                Vector3 movement = new Vector3(0f, 0f, -horizontalInput * moveSpeed * Time.deltaTime);
                transform.Translate(movement);
                source.PlayOneShot(clip2);
            }
        }
        else
        {
            Vector3 movement;
            switch (word)
            {
                case "forward":
                    transform.rotation = Quaternion.LookRotation(new Vector3(1, 0f, 0f));
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isRunning", true);
                    movement = new Vector3(0f, 0f, 1 * moveSpeed * Time.deltaTime);
                    transform.Translate(movement);
                    source.PlayOneShot(clip);
                    break;
                case "down":
                    transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0f, 0f));
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isRunning", true);
                    movement = new Vector3(0f, 0f, 1 * moveSpeed * Time.deltaTime);
                    transform.Translate(movement);
                    source.PlayOneShot(clip);
                    break;
                case "left":
                    transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f));
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isRunning", true);
                    movement = new Vector3(0f, 0f, 1 * moveSpeed * Time.deltaTime);
                    transform.Translate(movement);
                    source.PlayOneShot(clip);
                    break;
                case "right":
                    transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, -1f));
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isRunning", true);
                    movement = new Vector3(0f, 0f, 1 * moveSpeed * Time.deltaTime);
                    transform.Translate(movement);
                    source.PlayOneShot(clip);
                    break;
                case "jump":
                    animator.SetBool("isJumping", true);
                    animator.SetBool("isRunning", false);
                    break;
                case "stop":
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isRunning", false);
                    break;
                default:
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isRunning", false);
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
            if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
        Debug.Log("Stop");
    }
}


