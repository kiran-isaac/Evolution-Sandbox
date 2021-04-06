using UnityEngine;

public class Muscle : MonoBehaviour
{
    public float defaultLength;
    public float deviation = 1;

    private GameObject n1;
    private GameObject n2;

    private bool connected = false;

    private SpringJoint2D joint;

    private float currentScale;

    private float t;

    LineRenderer lr;

    public void Connect(GameObject node1, GameObject node2)
    {
        t = Random.Range(-100, 100);
        n1 = node1;
        n2 = node2;

        defaultLength = Vector3.Magnitude(n1.transform.position - n2.transform.position);

        Node n1Script = n1.GetComponent<Node>();
        Node n2Script = n2.GetComponent<Node>();
        n1Script.connections.Add(n2Script);
        n2Script.connections.Add(n1Script);

        joint = n1.AddComponent<SpringJoint2D>();
        joint.connectedBody = n2.GetComponent<Rigidbody2D>();
        joint.autoConfigureDistance = false;
        joint.frequency = 0;

        connected = true;

        Show();
    }

    private void UpdateXScale(float t)
    {
        float newScale = deviation * (Mathf.Sin(t) + 1) / 2 + defaultLength;
        currentScale = newScale;
    }

    private void UpdateJointLength()
    {
        if (joint)
        {
            joint.distance = currentScale;
        }
    }

    private void Show()
    {
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.1f;
        lr.startColor = lr.endColor = new Color(255, 0, 0);
        lr.sortingLayerName = "Foreground";
        lr.SetPosition(0, n1.transform.position);
        lr.SetPosition(1, n2.transform.position);
    }

    void Update()
    {
        if (connected)
        {
            t += 0.05f;
            UpdateXScale(t);
            transform.localScale = new Vector3(currentScale, transform.localScale.y, transform.localScale.z);
            UpdateJointLength();
            Show();
        }

        Creature parent = transform.parent.gameObject.GetComponent<Creature>();
        if (parent.isVisible)
        {
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }

    private void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }
}
