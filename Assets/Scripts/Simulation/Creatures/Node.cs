using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;

    public float mass;

    public List<Node> connections = new List<Node>();

    Rigidbody2D rb;
    CircleCollider2D col;

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
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        parent = transform.parent.gameObject.GetComponent<Creature>();

        mass = Random.Range(0.01f, 0.05f);
        float scale = Mathf.Sqrt((mass*100) / (Mathf.PI));
        transform.localScale = new Vector3(scale, scale, 1);
        rb.mass = mass;
        //col.sharedMaterial.bounciness = Random.Range(0f, 5f);
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
