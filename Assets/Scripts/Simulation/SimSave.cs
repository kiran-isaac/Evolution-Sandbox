using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SimSave
{
    // Stores the starting positions of the nodes
    List<List<float[]>> nodes = new List<List<float[]>>();

    // Stores which nodes are connected to which other nodes
    List<List<int>> connections = new List<List<int>>();

    public void AddCreature(Creature creature)
    {
        List<float[]> tempNodes = new List<float[]>();

        foreach (Node node in creature.nodes)
        {
            tempNodes.Add(new float[2] { node.gameObject.transform.position.x, node.gameObject.transform.position.y });

            List<int> tempConnections = new List<int>();

            foreach (Node connection in node.connections)
            {
                tempConnections.Add(connection.id);
            }

            connections.Add(tempConnections);
        }

        nodes.Add(tempNodes);
    }
}
