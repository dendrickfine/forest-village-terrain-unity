using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float kecepatan;
    public float loncat;
    private Rigidbody rb;
    private bool isGrounded;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Inisialisasi animator
    }

    void Update()
    {
        gerak();
        lompat();
    }

    void gerak()
    {
        float gerakX = Input.GetAxis("Horizontal");
        float gerakZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(gerakX, 0, gerakZ) * kecepatan;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        if (move.magnitude > 0)
        {
            transform.forward = move;
            animator.SetBool("iswalking", true);
        }
        else
        {
            animator.SetBool("iswalking", false);
        }
    }

    void lompat()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * loncat, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isjump", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tanah")) // Menghapus titik koma yang salah
        {
            isGrounded = true;
            animator.SetBool("isjump", false);
        }
    }
}
