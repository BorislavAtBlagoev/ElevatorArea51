using System.Collections.Concurrent;

namespace Elevator
{
    public class Elevator
    {
        private ConcurrentQueue<Agent> floorRequests;
        private Agent? passenger;
        private Floor currentFloor;
        private bool isOccupied;

        public Elevator()
        {
            this.floorRequests = new ConcurrentQueue<Agent>();
            this.passenger = null;
            this.currentFloor = Floor.G;
            this.isOccupied = false;
        }

        public void Operate()
        {
            while (true)
            {
                if (this.floorRequests.TryDequeue(out var agent))
                {
                    this.MoveToFloor(agent.CurrentFloor);
                    this.passenger = agent;
                    this.passenger.IsInElevator = true;
                    this.isOccupied = true;
                    Console.WriteLine($"Agent {this.passenger.AgentId} ({this.passenger.SecurityLevel}) enter the elevator on floor {this.currentFloor} and wants to go to floor {this.passenger.DestinationFloor}");

                    this.MoveToFloor(this.passenger.DestinationFloor);

                    if (!this.CanAccessFloor(this.passenger.SecurityLevel))
                    {
                        Console.WriteLine($"Agent {this.passenger.AgentId} ({this.passenger.SecurityLevel}) don't have required Security Level to access this floor {this.currentFloor}");
                        this.passenger.ChoosePermittedFloor();
                        this.MoveToFloor(this.passenger.DestinationFloor);
                    }

                    Console.WriteLine($"Agent {this.passenger.AgentId} ({this.passenger.SecurityLevel}) exit the elevator on floor {this.currentFloor}");
                    this.passenger.IsInElevator = false;
                    this.isOccupied = false;
                    this.passenger = null;
                }

                Thread.Sleep(100);
            }
        }

        public void CallElevator(Agent agent)
        {
            this.floorRequests.Enqueue(agent);
        }

        private void MoveToFloor(Floor floor)
        {
            while (this.currentFloor != floor)
            {
                Thread.Sleep(1000);

                if (this.currentFloor < floor)
                    this.currentFloor++;
                else if (this.currentFloor > floor)
                    this.currentFloor--;

                Console.WriteLine($"The elevator is at floor {this.currentFloor}");
            }
        }

        private bool CanAccessFloor(SecurityLevel securityLevel)
        {
            if (securityLevel == SecurityLevel.Confidential && this.currentFloor == Floor.G)
                return true;
            if (securityLevel == SecurityLevel.Secret && (this.currentFloor == Floor.G || this.currentFloor == Floor.S))
                return true;
            if (securityLevel == SecurityLevel.TopSecret)
                return true;
            return false;
        }
    }
}
