using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2_ostov
{
    public class Set
    {
        private int[] set;
        private int[] rank;

        public Set(int size)
        {
            set = new int[size];
            rank = new int[size];

            for (int i = 0; i < size; i++)
            {
                set[i] = i;
                rank[i] = 0;
            }
        }
        
        public int FindSet(int vert)
        {
            if (vert == set[vert])
            {
                return vert;
            }

            set[vert] = FindSet(set[vert]);
            return set[vert];
        }

        public void Union(int vert1, int vert2)
        {
            int set1 = FindSet(vert1);
            int set2 = FindSet(vert2);

            if (set1 != set2)
            {
                if (rank[set1] < rank[set2])
                {
                    set[set1] = set2;
                }
                else
                {
                    set[set2] = set1;
                    if (rank[set1] == rank[set2])
                    {
                        rank[set1]++;
                    }
                }
            }
        }
    }
}
