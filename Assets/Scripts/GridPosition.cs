namespace DefaultNamespace
{
    public readonly struct GridPosition
    {
        public readonly int x;
        public readonly int z;

        public GridPosition(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return $"x: { x }; z: { z }";
        }
    }
}