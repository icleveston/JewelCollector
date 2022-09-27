namespace JewelCollector.Items
{
    public class Radioactive : ItemMap {

        private int damage;

        public Radioactive(string Symbol = "!!", int Damage = 10) : base(Symbol)
        {
            this.damage = Damage;
        }

        public Radioactive Damage(Robot r)
        {
            r.energy -= this.damage;

            return this;
        }

    }
}