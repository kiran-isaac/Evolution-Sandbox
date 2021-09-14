using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulation.Creatures
{
    public class Node : MonoBehaviour
    {
        public SaveNode saveForm;

        public int id;

        public float mass;

        public List<Node> connections = new List<Node>();

        Rigidbody2D rb;

        CircleCollider2D col;

        Creature parent;

        private float RandRange = 2;

        new SpriteRenderer renderer;

        public GameObject CreateRandomChildNode(GameObject nodePrefab)
        {
            GameObject node = Instantiate(nodePrefab, new Vector3(UnityEngine.Random.Range(-RandRange, RandRange),
                UnityEngine.Random.Range(-RandRange, RandRange), 0), Quaternion.identity, transform.parent.transform);

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
        }

        public void GenerateSaveFormat()
        {
            saveForm = new SaveNode(new float[2] { transform.position.x, transform.position.y }, mass, id, connections);
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

    [System.Serializable]
    public class SaveNode
    {
        float[] pos = new float[2];
        float mass;
        int id;
        int[] connections;

        public SaveNode(float[] pos, float mass, int id, List<Node> connections)
        {
            this.pos = pos;
            this.mass = mass;
            this.id = id;

            this.connections = (from connection in connections select connection.id).ToArray();
        }
    }
}