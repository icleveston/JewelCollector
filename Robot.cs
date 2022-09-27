using JewelCollector.Items;
using JewelCollector.Items.Jewel;
using JewelCollector.Exceptions;

namespace JewelCollector
{
        
    public class Robot : ItemMap {

        private Map map;
        private int x, y;
        private List<Jewel> Bag = new List<Jewel>();
        public int energy {get; set;}

        public bool HasEnergy => this.energy > 0;
        public bool IsDone => this.map.JewelsCount == 0;

        public Robot(Map map, int x=0, int y=0, int energy=10, string Symbol = "ME") : base(Symbol){
            this.map = map;
            this.x = x;
            this.y = y;
            this.energy = energy;

            this.map.Insert(this, x, y);
        }

        public void Move(Direction d)
        {
            int xNew = 0; int yNew = 0;

            try
            {
                (xNew, yNew) = d switch
                {
                    Direction.NORTH => (this.x-1, this.y),
                    Direction.SOUTH => (this.x+1, this.y),
                    Direction.EAST => (this.x, this.y+1),
                    Direction.WEST => (this.x, this.y-1),
                    _ => throw new Exception()
                };

                Radioactive? r = map.Update(this.x, this.y, xNew, yNew);

                r?.Damage(this).Damage(this);
        
                map.GetItem<Radioactive>(xNew, yNew)?.Damage(this);      
                
                this.x = xNew;
                this.y = yNew;
                this.energy--;
            
            } 
            catch (OccupiedPositionException)
            {
                Console.WriteLine($"Position {xNew}, {yNew} is occupied");
            }
            catch (OutOfMapException)
            {
                Console.WriteLine($"Position {xNew}, {yNew} is out of map");
            }       
        }

        public void Get(){

            Bag = Bag.Concat(map.GetJewels(this.x, this.y)).ToList();

            map.GetItem<IRechargeable>(this.x, this.y)?.Recharge(this);
        }

        public void Print()
        {
            map.Print();

            (int ItensBag, int TotalPoints) = this.GetBagInfo();
            Console.WriteLine($"Itens Bag: {ItensBag} - Total Points: {TotalPoints} - Energy: {this.energy}");
        }
        
        private (int, int) GetBagInfo()
        {
            int Points = 0;

            foreach (Jewel j in this.Bag) Points += j.Points;

            return (this.Bag.Count, Points);
        }
    }
}