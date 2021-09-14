using System;
using Simulation.Creatures;

namespace Simulation
{
    [Serializable]
    public class Generation
    {
        Creature[] creatures = new Creature[10];

        int n = 10;
    }
}
