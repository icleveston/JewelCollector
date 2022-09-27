namespace JewelCollector.Items.Obstacle
{
    public class Tree : Obstacle, IRechargeable {

        public Tree(string Symbol = "$$") : base(Symbol) {}

        public void Recharge(Robot r) 
        {
            r.energy += 3;
        }

    }
}