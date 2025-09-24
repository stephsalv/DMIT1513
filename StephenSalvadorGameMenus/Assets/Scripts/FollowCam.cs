using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public GameObject target  = null;
    public Transform pos1, pos2, pos3;
    public GameObject t = null;
    public float speed = 1.5f;
    public int index = 0;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        t = GameObject.FindGameObjectWithTag("Target");

        pos1 = GameObject.FindGameObjectWithTag("Pos1").GetComponent<Transform>();
        pos2 = GameObject.FindGameObjectWithTag("Pos2").GetComponent<Transform>();
        pos3 = GameObject.FindGameObjectWithTag("Pos3").GetComponent<Transform>();

        index = PlayerPrefs.GetInt("save");
    }

    void Update()
    {
        if (index > 2)
        {
            index = 0;
        }
        if (index < 0)
        {
            index = 2;
        }
    }

    void FixedUpdate()
    {
        this.transform.LookAt(target.transform);
        float carMove = Mathf.Abs(Vector3.Distance(this.transform.position, t.transform.position) * speed);
        this.transform.position = Vector3.MoveTowards(this.transform.position, t.transform.position, carMove * Time.deltaTime);

        if (index == 0)
        {
            t.transform.position = pos1.position;
        }
        if (index == 1)
        {
            t.transform.position = pos2.position;
        }
        if (index == 2)
        {
            t.transform.position = pos3.position;
        }
    }

    public void Next()
    {
        index++;
        PlayerPrefs.SetInt("save", index);
        PlayerPrefs.Save();
    }

}
