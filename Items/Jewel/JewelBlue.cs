namespace JewelCollector.Items.Jewel
{
    public class JewelBlue : Jewel, IRechargeable {

        public JewelBlue(string Symbol = "JB", int Points = 10) : base(Symbol, Points){}

        public void Recharge(Robot r) 
        {
            r.energy += 5;
        }
        
    }
}
