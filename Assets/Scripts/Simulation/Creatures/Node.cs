using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;

    public List<Node> connections = new List<Node>();

    Creature parent;

    public float start_x;
    public float start_y;

    private float RandRange = 2;

    new SpriteRenderer renderer;

    public GameObject CreateRandomChildNode(GameObject nodePrefab)
    {
        GameObject node = Instantiate(nodePrefab, new Vector3(Random.Range(-RandRange, RandRange),
            Random.Range(-RandRange, RandRange), 0), Quaternion.identity, transform.parent.transform);

        return node;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Destroy(parent.gameObject);
        }
    }

    private void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        parent = transform.parent.gameObject.GetComponent<Creature>();
    }

    private void Update()
    {
        if (parent.isVisible)
        {
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }
    }
}
