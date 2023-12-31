using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f;

    bool clearGame = false;
    Scene sceneName;

    CharacterController charCtrl;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); //자식오브젝트의 컴포넌트를 가져오겠다.
        sceneName = GetComponentInChildren<Scene>();
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
            clearGame = true;

            Debug.Log(sceneName);

            //switch(SceneManager.GetSceneByName("Stage1"))
            //{
            //    case "Dot":
            //}
        }
            
    }


}
