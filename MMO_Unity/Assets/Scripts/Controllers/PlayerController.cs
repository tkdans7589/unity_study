using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float _speed = 10.0f;

    Vector3 _destPos;
    void Start()
    {
        // Managers.Input.KeyAction -= OnKeyboard;
        // Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    // GameObject (Player)
        // Transform
        // PlayerController (*)
    // Update is called once per frame
    float _yAngle = 0.0f;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }
    PlayerState _state = PlayerState.Idle;

    void UpdateDie()
    {

    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        //animation
        Animator anim = GetComponent<Animator>();
        //현재 게임 상태에 대한 정보를 넘겨준다.
        anim.SetFloat("speed", _speed);
    }

    void UpdateIdle()
    {
        //animation
        Animator anim = GetComponent<Animator>();
        
        anim.SetFloat("speed", 0);
    }


    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }
    // void OnKeyboard()
    // {
    //    _yAngle += Time.deltaTime * 100.0f;

    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //         transform.position += Vector3.forward * Time.deltaTime * _speed;
    //     }
            
    //     if (Input.GetKey(KeyCode.S))
    //     {
    //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //         transform.position += Vector3.back * Time.deltaTime * _speed;
    //     }
            
    //     if (Input.GetKey(KeyCode.A))
    //     {
    //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //         transform.position += Vector3.left * Time.deltaTime * _speed;
    //     }
            
    //     if (Input.GetKey(KeyCode.D))
    //     {
    //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //         transform.position += Vector3.right * Time.deltaTime * _speed;
    //     }

    //     _moveToDest = false;
    // }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Debug.Log("OnMouseClicked");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
            // Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
        }
    }
}
