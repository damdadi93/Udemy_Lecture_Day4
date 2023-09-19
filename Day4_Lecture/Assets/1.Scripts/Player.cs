using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f;

    CharacterController charCtrl;
    Animator anim;


    //중력주기 ? 컨트롤러 자체에 중력이 있는것인가? 아니면 없는것인가?
    [SerializeField]
    private float gravity = -9.81f; //중력계수-9.81f
    private Vector3 moveDirector;
    


    //[SerializeField]
    bool clearGame = false;

    [SerializeField]
    string sceneName;

    



    // Start is called before the first frame update
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); //자식오브젝트의 컴포넌트를 가져오겠다.
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(
            Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (dir.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(transform.forward, dir,
            rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, dir));
            transform.LookAt(transform.position + forward);
        }
        charCtrl.Move(dir * moveSpeed * Time.deltaTime);
        anim.SetFloat("Speed", charCtrl.velocity.magnitude);

        if (GameObject.FindGameObjectsWithTag("Dot").Length < 1)
        {
            clearGame = true;
            SceneManager.LoadScene("Win");
        }

        //중력
        if(charCtrl.isGrounded == false)
        {
            moveDirector.y += gravity * Time.deltaTime;
        }
        charCtrl.Move(moveDirector *moveSpeed * Time.deltaTime);
        ClearScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("dot!");

        switch (other.tag)
        {
            case "Dot":
                Destroy(other.gameObject);
                break;
            case "Enemy":
                SceneManager.LoadScene("Lose");
                break;
        }


        //if (other.CompareTag("Enemy"))
        //{
        //    Destroy(other.gameObject);
        //}
        
        
    }


    private void ClearScene()
    {
        if (GameObject.FindGameObjectsWithTag("Dot").Length < 1)
        {                                    
            sceneName = SceneManager.GetActiveScene().name;

            Debug.Log(sceneName);

            switch (sceneName)
            {
                case "Stage1":
                    SceneManager.LoadScene("Stage2");
                    break;
            }
        }

    }

    private void MoveTo(Vector3 direction)
    {
        moveDirector = new Vector3(direction.x, moveDirector.y, direction.z);
    }
    

}
