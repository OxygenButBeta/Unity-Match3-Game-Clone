using System.Collections.Generic;

namespace Match3{
    public class CandyEqualityComparer : IEqualityComparer<Candy>{
        public bool Equals(Candy x, Candy y) => x != null && y != null && x.scriptableCandy.Equals(y.scriptableCandy);
        public int GetHashCode(Candy obj) => obj.scriptableCandy.GetHashCode();
    }
}