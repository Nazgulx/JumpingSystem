using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody myRigidbody;
    Vector3 velocity;
    public Vector3 jump;
    public Vector3 startScale;
    public float speed= 6;
    public float jumpForce = 2f;
    public float timeToGround;
    public float charger;
    public bool isGrounded;
    public bool discharge;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        myRigidbody = GetComponent<Rigidbody>();
        jump = new Vector3(0, 2, 0);
    }

     void OnCollisionStay() {
       isGrounded = true;
    }

     void Jump(){
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.AddForce(jump * jumpForce, ForceMode.Impulse);
            myRigidbody.transform.localScale = startScale;
    }

     void Move(){
          Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        velocity = direction * speed;

     }

     void Raycasting(){
         // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
           // Debug.Log(hit.distance - 0.5);
            
        }

        if (hit.distance - 0.5 <= 0.1f){
            isGrounded = true;
        }
        else{
           isGrounded = false;
        }
     }

     

    // Update is called once per frame
    void Update()
    {
       Raycasting();


       if(Input.GetKey(KeyCode.Space) && isGrounded){
           charger += Time.deltaTime;
           speed = 3;
           if(charger >= 1){
               charger = 1f;
           }
           if(transform.localScale.y >= 0.75f){
               transform.localScale -= new Vector3(-0.001f,0.001f,-0.001f);
           }



       }
       if(Input.GetKeyUp(KeyCode.Space)){
           discharge = true;
           speed = 6;
       }

    }

    void FixedUpdate(){
       Move();
        myRigidbody.position += velocity * Time.deltaTime;
        if(discharge){

            //Set new jumpForce and then jumping
            jumpForce = 5 * charger;
            Jump();

            //Reset discharge and charger
            discharge= false;
            charger = 0f;
        }
    }
    

}
