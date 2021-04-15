using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class SimSave
{
    List<List<SaveNode>> creatures = new List<List<SaveNode>>();

    public void AddCreature(Creature creature)
    {
        creatures.Add((from node in creature.nodes select node.saveForm).ToList());
    }
}
