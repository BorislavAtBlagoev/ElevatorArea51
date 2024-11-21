namespace Elevator
{
    public class Agent
    {
        private Random random;

        public long AgentId { get; private set; }
        public SecurityLevel SecurityLevel { get; private set; }
        public Floor CurrentFloor { get; private set; }
        public Floor DestinationFloor { get; private set; }
        public bool IsInElevator { get; set; }

        public Agent()
        {
            this.random = new Random();
            this.AgentId = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
            this.SecurityLevel = this.GetSecurityLevel();
            this.CurrentFloor = this.GetFloorBySecurityLevel();
            this.DestinationFloor = this.GetDestinationFloor();
            this.IsInElevator = false;
        }

        public void ChoosePermittedFloor()
        {
            Console.WriteLine($"Agent {this.AgentId} ({this.SecurityLevel}) choosing permitted floor");

            if (this.SecurityLevel == SecurityLevel.Confidential)
                this.DestinationFloor = Floor.G;
            if (this.SecurityLevel == SecurityLevel.Secret)
                this.DestinationFloor = (Floor)this.random.Next(2);

            Console.WriteLine($"Agent {this.AgentId} ({this.SecurityLevel}) new destination floor is {this.DestinationFloor}");
        }

        private SecurityLevel GetSecurityLevel()
        {
            int value = this.random.Next(3);
            if (value == 0)
                return SecurityLevel.Confidential;
            if (value == 1)
                return SecurityLevel.Secret;
            if (value == 2)
                return SecurityLevel.TopSecret;
            return SecurityLevel.Confidential;
        }

        private Floor GetFloorBySecurityLevel()
        {
            if (this.SecurityLevel == SecurityLevel.Confidential)
                return Floor.G;
            if (this.SecurityLevel == SecurityLevel.Secret)
                return (Floor)this.random.Next(2);
            if (this.SecurityLevel == SecurityLevel.TopSecret)
                return (Floor)this.random.Next(4);
            return Floor.G;
        }

        private Floor GetDestinationFloor()
        {
            int currentFloorNumber = (int)this.CurrentFloor;
            int destinationFloor = currentFloorNumber;

            while (currentFloorNumber == destinationFloor)
            {
                destinationFloor = this.random.Next(4);
            }

            return (Floor)destinationFloor;
        }
    }
}
