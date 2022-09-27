namespace JewelCollector
{
  
  public class JewelCollector {

    delegate void Move(Direction d);
    static event Move? OnMove; 

    public static void Main() {

        int level = 1;

        while(true)
        {

          Map map = new Map(level);
          Robot robot = new Robot(map);

          Console.WriteLine($"------------- Level: {level} ------------- ");

          if(Run(robot)) level++;

        }
          
      }

      private static bool Run(Robot robot)
      {

          OnMove += robot.Move; 
      
          while (true) {

              robot.Print();
              
              Console.WriteLine("Enter the command: ");
              ConsoleKeyInfo command = Console.ReadKey(true);
      
              switch (command.Key.ToString())
              {
                  case "W" : OnMove(Direction.NORTH); break;
                  case "S" : OnMove(Direction.SOUTH); break;
                  case "D" : OnMove(Direction.EAST); break;
                  case "A" : OnMove(Direction.WEST); break;
                  case "G" : robot.Get(); break;
                  case "Escape" : Environment.Exit(0); break;
                  default: Console.WriteLine(command.Key.ToString()); break;
              }

              if (robot.IsDone) return true;
              else if (!robot.HasEnergy) return false;

          }

      }

  }
}