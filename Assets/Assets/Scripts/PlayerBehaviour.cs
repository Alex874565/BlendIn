using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private bool is2d;

    [SerializeField] float speed2d;
    [SerializeField] float speed3d;
    [SerializeField] float maxSpeed3d;
    [SerializeField] float jumpForce;

    [SerializeField] private BoxCollider collider3d;
    [SerializeField] private BoxCollider2D collider2d;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private ColorStatesDatabase colorStatesDatabase;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject transformParticles;
    [SerializeField] private GameObject moving3DParticles;
    [SerializeField] private GameObject jumpParticles;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GameObject eye1;
    [SerializeField] private GameObject eye2;

    [SerializeField] private Vector3 eye1Default;
    [SerializeField] private Vector3 eye2Default;

    private bool moving3D;
    private bool jumping;

    private int stars;
    public int Stars => stars;

    private ColorType color;

    // Start is called before the first frame update
    void Start()
    {
        stars = 0;
        if (!meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (!rb2d)
        {
            rb2d = spriteObject.GetComponent<Rigidbody2D>();
        }
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        if (is2d)
        {
            Enter2D();
        }
        else
        {
            Enter3D();
        }

        eye1Default = eye1.transform.localPosition;
        eye2Default = eye2.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapStates();
        }

        if (!is2d && Input.GetKeyDown(KeyCode.W))
        {
            if (Mathf.Abs(rb.velocity.y) <= 0.1f)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                moving3DParticles.GetComponent<ParticleSystem>().Stop();
                //Instantiate(jumpParticles, transform.position - new Vector3(0, 0.5f, 0), Quaternion.Euler(0, 75, 0));
            }
        }
        if (Mathf.Abs(rb.velocity.y) > 0)
        {
            jumping = true;
        }
        if (jumping && Mathf.Abs(rb.velocity.y) <= 0.05f)
        {
            //Instantiate(jumpParticles, transform.position - new Vector3(0, 0.5f, 0), Quaternion.Euler(0, 75, 0));
            jumping = false;
        }
        if (!is2d)
        {
            Vector2 velocityOffset = rb.velocity * 0.05f; // adjust sensitivity

            Vector3 targetOffset = new Vector3(velocityOffset.x, velocityOffset.y, 0);
            targetOffset = Vector3.ClampMagnitude(targetOffset, 0.02f); // max eye offset

            eye1.transform.localPosition = Vector3.Lerp(eye1.transform.localPosition, eye1Default + targetOffset, Time.deltaTime * 5f);
            eye2.transform.localPosition = Vector3.Lerp(eye2.transform.localPosition, eye2Default + targetOffset, Time.deltaTime * 5f);
        }
        else
        {
            Vector2 velocityOffset = rb2d.velocity * 0.05f; // adjust sensitivity

            Vector3 targetOffset = new Vector3(velocityOffset.x, velocityOffset.y, 0);
            targetOffset = Vector3.ClampMagnitude(targetOffset, 0.02f); // max eye offset

            eye1.transform.localPosition = Vector3.Lerp(eye1.transform.localPosition, eye1Default + targetOffset, Time.deltaTime * 5f);
            eye2.transform.localPosition = Vector3.Lerp(eye2.transform.localPosition, eye2Default + targetOffset, Time.deltaTime * 5f);

        }
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    void SwapStates()
    {
        if (is2d)
        {
            Enter3D();
        }
        else
        {
            Enter2D();
        }
    }


    void Enter2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward, 2f);
        Collider2D other = hit.collider;
        Debug.Log("Raycast hit: " + other?.gameObject.name);
        if (!other || !other.gameObject.CompareTag("Color"))
        {
            return;
        }

        color = other.GetComponent<ColorLayer>().color;

        rb.velocity = Vector3.zero;
        collider3d.enabled = false;

        is2d = true;
        rb.useGravity = false;
        collider2d.isTrigger = false;
        collider2d.excludeLayers = 1 << LayerMask.NameToLayer(color.ToString());

        StartCoroutine(Transition2D(color));
    }

    void Enter3D()
    {

        is2d = false;
        rb2d.velocity = Vector2.zero;
        rb.velocity = Vector3.zero;
        collider3d.enabled = true;

        animator.Play("PlayerReverse");

        spriteObject.GetComponent<BoxCollider2D>().enabled = false;
        meshRenderer.enabled = true;
        rb.useGravity = true;
        collider2d.isTrigger = true;

        rb.position = new Vector3(rb2d.position.x, rb2d.position.y, rb.position.z);
        spriteObject.transform.localPosition = new Vector3(0f, 0f, spriteObject.transform.localPosition.z);
    }

    IEnumerator Transition2D(ColorType color)
    {
        animator.Play("Player");

        yield return new WaitForSeconds(0.25f);

        Instantiate(transformParticles, transform.position, Quaternion.identity);

        is2d = true;

        spriteObject.GetComponent<BoxCollider2D>().enabled = true;
        meshRenderer.enabled = false;
    }

    void ProcessMovement()
    {
        if (is2d)
        {
            rb2d.velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized * speed2d;
        }
        else
        {
            rb.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f) * speed3d, ForceMode.Acceleration);
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed3d, maxSpeed3d), rb.velocity.y, 0);
            if(!moving3D && Mathf.Abs(rb.velocity.x) > 1f && !jumping)
            {
                Instantiate(moving3DParticles, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
            }
        }
    }

    public void CollectItem(GameObject item)
    {
        stars++;
        levelManager.AddStar();
    }

    public void LeaveButtonLayer(ColorType color)
    {
        if (is2d)
        {
            Enter3D();
        }
    }

    public void Die()
    {
        Debug.Log("Player died");
        rb.velocity = Vector3.zero;
        rb2d.velocity = Vector2.zero;
        rb.position = new Vector3(0, 0, 0);
        rb2d.position = new Vector2(0, 0);
        levelManager.Death();
    }
}
