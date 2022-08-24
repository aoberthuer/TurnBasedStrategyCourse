namespace tbs.grid
{
    public class PathNode
    {
        private GridPosition _gridPosition;
        
        private int _gCost;
        private int _hCost;
        private int _fCost;
        
        private PathNode _cameFromPathNode;

        public PathNode(GridPosition gridPosition)
        {
            this._gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return _gridPosition.ToString();
        }

        public int GetGCost()
        {
            return _gCost;
        }
        
        public void SetGCost(int gCost)
        {
            this._gCost = gCost;
        }

        public int GetHCost()
        {
            return _hCost;
        }
        
        public void SetHCost(int hCost)
        {
            this._hCost = hCost;
        }

        public int GetFCost()
        {
            return _fCost;
        }
        
        public void CalculateFCost()
        {
            _fCost = _gCost + _hCost;
        }

        public void ResetCameFromPathNode()
        {
            _cameFromPathNode = null;
        }

        public void SetCameFromPathNode(PathNode pathNode)
        {
            _cameFromPathNode = pathNode;
        }

        public PathNode GetCameFromPathNode()
        {
            return _cameFromPathNode;
        }

        public GridPosition GetGridPosition()
        {
            return _gridPosition;
        }


    }
}