using System.Collections.Generic;
using System.Linq;
using Simulation.Creatures;

namespace Simulation
{
    [System.Serializable]
    public class SimSave
    {
        List<List<SaveNode>> creatures = new List<List<SaveNode>>();

        public void AddCreature(Creature creature)
        {
            creatures.Add((from node in creature.nodes select node.saveForm).ToList());
        }
    }
}
