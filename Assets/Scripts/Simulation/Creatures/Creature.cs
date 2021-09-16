using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Creatures
{
    public class Creature : MonoBehaviour
    {
        public Genome genome;

        static SimSave simSave = new SimSave();

        public List<Node> nodes;
        public List<Muscle> muscles;

        public bool isVisible = true;

        public float Fitness {
            get {
                float xSum = 0;
                foreach (Node node in nodes)
                {
                    xSum += node.gameObject.transform.position.x;
                }
                return xSum / nodes.Count;
            }
        }

        void GenerateNodes(int n, GameObject nodePrefab)
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

        void GenerateMuscles(int m, GameObject musclePrefab)
        {
            muscles = new List<Muscle>();
            List<Node> nodeTree = new List<Node>();

            List<Node> unconnected = new List<Node>(); ;

            foreach (Node n in nodes)
            {
                unconnected.Add(n);
            }

            Node node = unconnected[0];
            nodeTree.Add(node);
            unconnected.Remove(node);

            int musclesToCreate = m;

            do
            {
                int randomNodePos = Random.Range(0, nodeTree.Count);
                Node n1 = nodeTree[randomNodePos];

                randomNodePos = Random.Range(0, unconnected.Count);
                Node n2 = unconnected[randomNodePos];
                unconnected.Remove(n2);
                nodeTree.Add(n2);

                Muscle muscle = Instantiate(musclePrefab, transform).GetComponent<Muscle>();
                muscle.Connect(n1.gameObject, n2.gameObject);

                musclesToCreate -= 1;
            }
            while (unconnected.Count > 0);


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

                Muscle muscle = Instantiate(musclePrefab, transform).GetComponent<Muscle>();
                muscle.Connect(n1.gameObject, n2.gameObject);
            }
        }

        public static Creature Generate(int n, int m, GameObject nodePrefab, GameObject musclePrefab, Vector3 pos)
        {
            Creature creature = new GameObject("Creature").AddComponent<Creature>();
            creature.gameObject.transform.parent = GameObject.Find("SimulationManager").transform;

            creature.GenerateNodes(n, nodePrefab);

            creature.GenerateMuscles(m, musclePrefab);

            foreach (Node node in creature.nodes)
            {
                node.GenerateSaveFormat();
            }

            creature.transform.position = pos;

            return creature;
        }
    }

    public struct Genome
    {
        public int n;
        public int m;

        public 
    }
}