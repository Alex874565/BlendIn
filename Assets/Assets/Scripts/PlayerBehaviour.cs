using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private bool is2d;

    [SerializeField] float speed2d;
    [SerializeField] float speed3d;
    [SerializeField] float maxSpeed3d;
    [SerializeField] float jumpForce;

    [SerializeField] private BoxCollider collider3d;
    [SerializeField] private BoxCollider collider2d;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private ColorStatesDatabase colorStatesDatabase;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject particles;

    private ColorType color;

    // Start is called before the first frame update
    void Start()
    {
        if (!meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapStates();
        }

        if(!is2d && Input.GetKeyDown(KeyCode.W))
        {
            if(rb.velocity.y == 0f)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
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
        Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 2f);
        var other = hit.collider;
        if(!other || !other.gameObject.CompareTag("Color"))
        {
            return;
        }

        color = other.GetComponentInParent<ColorLayer>().color;
        Debug.Log("other color: " + other.GetComponentInParent<ColorLayer>().color);

        rb.velocity = Vector3.zero;
        is2d = true;
        rb.useGravity = false;
        collider2d.isTrigger = false;
        collider2d.excludeLayers = 1 << LayerMask.NameToLayer(color.ToString());

        StartCoroutine(Transition2D());
    }

    void Enter3D()
    {
        rb.velocity = Vector3.zero;
        is2d = false;
        rb.useGravity = true;
        collider2d.isTrigger = true;

        animator.Play("PlayerReverse");

        spriteObject.SetActive(false);
        meshRenderer.enabled = true;
    }

    IEnumerator Transition2D()
    {
        animator.Play("Player");

        yield return new WaitForSeconds(0.25f);

        Instantiate(particles, transform.position, Quaternion.identity);

        spriteObject.GetComponent<SpriteRenderer>().sprite = colorStatesDatabase.GetColorSprite(color);
        spriteObject.SetActive(true);
        meshRenderer.enabled = false;
    }

    void ProcessMovement()
    {
        if (is2d)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized * speed2d;
        }
        else
        {
            rb.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f) * speed3d, ForceMode.Acceleration);
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed3d, maxSpeed3d), rb.velocity.y, 0);
        }
    }
}
