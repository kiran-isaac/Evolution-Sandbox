using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public List<Node> nodes;
    public List<Muscle> muscles;

    public bool isVisible = true;

    int n;
    int m;

    public float fitness;

    public void Setup(int n, int m)
    {
        this.n = n;
        this.m = m;
    }

    public float AvgX()
    {
        float xSum = 0;
        foreach (Node node in nodes)
        {
            xSum += node.gameObject.transform.position.x;
        }
        return xSum / nodes.Count;
    }

    private void Update()
    {
        fitness = AvgX();
    }

    public void GenerateNodes(GameObject nodePrefab)
    {
        nodes = new List<Node>();

        GameObject root = Instantiate(nodePrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        Node rootScript = root.GetComponent<Node>();
        nodes.Add(rootScript);
        rootScript.id = 0;

        for (int i = 0; i < n - 1; i++)
        {
            root = rootScript.CreateRandomChildNode(nodePrefab);
            rootScript = root.GetComponent<Node>();
            nodes.Add(rootScript);
            rootScript.id = i + 1;
        }
    }

    public void GenerateMuscles(GameObject musclePrefab)
    {
        muscles = new List<Muscle>();

        List<Node> nodeTree = new List<Node>();

        List<Node> unselected = new List<Node>(); ;

        foreach (Node n in nodes)
        {
            unselected.Add(n);
        }

        Node node = unselected[0];
        nodeTree.Add(node);
        unselected.Remove(node);

        int musclesToCreate = m;

        do
        {
            int randomNodePos = Random.Range(0, nodeTree.Count);
            Node n1 = nodeTree[randomNodePos];

            randomNodePos = Random.Range(0, unselected.Count);
            Node n2 = unselected[randomNodePos];
            unselected.Remove(n2);
            nodeTree.Add(n2);

            GameObject muscle = Instantiate(musclePrefab, transform);
            Muscle muscleScript = muscle.GetComponent<Muscle>();
            muscleScript.Connect(n1.gameObject, n2.gameObject);

            musclesToCreate -= 1;
        }
        while (unselected.Count > 0);


        for (int i = 0; i < musclesToCreate; i++)
        {
            Node n1, n2;
            int rnd1 = Random.Range(0, nodes.Count);
            int rnd2 = Random.Range(0, nodes.Count);
            while (rnd1 == rnd2)
            {
                rnd1 = Random.Range(0, nodes.Count);
                rnd2 = Random.Range(0, nodes.Count);
            }

            n1 = nodes[rnd1];
            n2 = nodes[rnd2];

            while (n1.connections.Contains(n2))
            {
                rnd1 = Random.Range(0, nodes.Count);
                rnd2 = Random.Range(0, nodes.Count);
                while (rnd1 == rnd2)
                {
                    rnd1 = Random.Range(0, nodes.Count);
                    rnd2 = Random.Range(0, nodes.Count);
                }

                n1 = nodes[rnd1];
                n2 = nodes[rnd2];
            }

            GameObject muscle = Instantiate(musclePrefab, transform);
            Muscle muscleScript = muscle.GetComponent<Muscle>();
            muscleScript.Connect(n1.gameObject, n2.gameObject);
        }
    }

    public static void Generate(int n, int m, GameObject nodePrefab, GameObject musclePrefab)
    {
        GameObject newCreature = new GameObject("Creature");
        newCreature.transform.position = newCreature.transform.position;
        newCreature.transform.parent = GameObject.Find("SimulationManager").transform;

        Creature creature = newCreature.AddComponent<Creature>();

        creature.Setup(n, m);

        creature.GenerateNodes(nodePrefab);

        creature.GenerateMuscles(musclePrefab);    
    }
}
