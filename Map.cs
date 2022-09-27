using JewelCollector.Items;
using JewelCollector.Items.Jewel;
using JewelCollector.Items.Obstacle;
using JewelCollector.Exceptions;

namespace JewelCollector
{
    public class Map {

        private ItemMap[,] Matriz;
        private int h;
        private int w;
        private int level;

        private Random rand = new Random(1);  

        public int JewelsCount 
        {
            get {

                int Jewels = 0;

                for (int i = 0; i < this.w; i++)
                    for (int j = 0; j < this.h; j++)
                        if (Matriz[i, j] is Jewel) Jewels++;

                return Jewels;
            }
        }

        public Map(int level=1)
        {
            w = 10 + level-1;
            h = 10 + level-1;

            this.w = w <= 30 ? w : 30;
            this.h = h <= 30 ? h : 30;
            this.level = level;

            Matriz = new ItemMap[this.w, this.h];

            for (int i = 0; i < this.w; i++) 
                for (int j = 0; j < this.h; j++) 
                    Matriz[i, j] = new Empty();

            if (level == 1) 
                GenerateFixed();
            else 
                GenerateRandom();
        }

        public void Insert(ItemMap Item, int x, int y) => Matriz[x, y] = Item;

        public void Insert(ItemMap Item) => Matriz[rand.Next(w), rand.Next(h)] = Item;

        public Radioactive? Update(int x_old, int y_old, int x, int y)
        {
            if (x < 0 || y < 0 || x > this.w-1 || y > this.h-1)
                throw new OutOfMapException();

            if (IsAllowed(x, y))
            {
                Radioactive? radioactive = Matriz[x, y] is Radioactive r ? r : null;

                Matriz[x, y] = Matriz[x_old, y_old];
                Matriz[x_old, y_old] = new Empty(); 

                return radioactive;
            }
            else
                throw new OccupiedPositionException();
        }

        public List<Jewel> GetJewels(int x, int y)
        {
            List<Jewel> NearJewels = new List<Jewel>();

            int[,] Coords = GenerateCoord(x, y);

            for (int i = 0; i < Coords.GetLength(0); i++) {

                Jewel? jewel = GetJewel(Coords[i, 0], Coords[i, 1]);
                
                if (jewel is not null) NearJewels.Add(jewel);
            }

            return NearJewels;
        }

        private Jewel? GetJewel(int x, int y)
        {
            if (Matriz[x, y] is Jewel jewel)
            {
                Matriz[x, y] = new Empty();
                return jewel;
            }

            return null;
        }

        public T GetItem<T>(int x, int y)
        {
            int[,] Coords = GenerateCoord(x, y);

            for (int i = 0; i < Coords.GetLength(0); i++) 
                if (Matriz[Coords[i, 0], Coords[i, 1]] is T r) 
                    return r;

            return default(T);
        }

        private int[,] GenerateCoord(int x, int y)
        {
            return new int[4, 2] {
                {x, y+1 < w-1 ? y+1 : w-1},
                {x, y-1 > 0 ? y-1 : 0},
                {x+1 < h-1 ? x+1 : h-1, y},
                {x-1 > 0 ? x-1 : 0, y }
            };
        }

        private bool IsAllowed(int x, int y) => Matriz[x, y] is Empty or Radioactive;

        public void Print() {

            for (int i = 0; i < this.w; i++){
                for (int j = 0; j < this.h; j++)
                    Console.Write(Matriz[i, j] + " ");

                Console.Write("\n");
            }
        }

        private void GenerateFixed()
        {
            this.Insert(new JewelRed(), 1, 9);
            this.Insert(new JewelRed(), 8, 8);
            this.Insert(new JewelGreen(), 9, 1);
            this.Insert(new JewelGreen(), 7, 6);
            this.Insert(new JewelBlue(), 3, 4);
            this.Insert(new JewelBlue(), 2, 1);

            this.Insert(new Water(), 5, 0);
            this.Insert(new Water(), 5, 1);
            this.Insert(new Water(), 5, 2);
            this.Insert(new Water(), 5, 3);
            this.Insert(new Water(), 5, 4);
            this.Insert(new Water(), 5, 5);
            this.Insert(new Water(), 5, 6);
            this.Insert(new Tree(), 5, 9);
            this.Insert(new Tree(), 3, 9);
            this.Insert(new Tree(), 8, 3);
            this.Insert(new Tree(), 2, 5);
            this.Insert(new Tree(), 1, 4);
        }

        private void GenerateRandom()
        {
            for(int x = 0; x < this.level; x++)
                this.Insert(new JewelBlue());

            for(int x = 0; x < this.level; x++)
                this.Insert(new JewelRed());

            for(int x = 0; x < this.level; x++)
                this.Insert(new JewelGreen());

            for(int x = 0; x < 3+this.level; x++)
                this.Insert(new Water());

            for(int x = 0; x < 8+this.level; x++)
                this.Insert(new Tree());

            this.Insert(new Radioactive());
        }
    }
}