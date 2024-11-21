namespace Elevator
{
    public class Base
    {
        private Elevator elevator;
        private Random random;

        public Base()
        {
            this.elevator = new Elevator();
            this.random = new Random();
        }

        public void StartSimulation()
        {
            Thread elevatorThread = new Thread(this.elevator.Operate);
            elevatorThread.Start();

            for (int i = 0; i < 6; i++)
            {
                Thread agentThread = new Thread(this.SimulateAgent);
                agentThread.Start();

                Thread.Sleep(random.Next(1000, 5000));
            }

            Console.ReadLine();
        }

        private void SimulateAgent()
        {
            Agent agent = new Agent();
            this.elevator.CallElevator(agent);
            Console.WriteLine($"Agent {agent.AgentId} ({agent.SecurityLevel}) call the elevator from floor {agent.CurrentFloor}");
        }
    }
}
