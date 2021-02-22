using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PM.BuildingClass
{
    public class BuildingWorld
    {
        public Dictionary<int, BuildingChunk> Chunks = new Dictionary<int, BuildingChunk>();
        
        public int this[int count]
        {
            get
            {
                var chunk = Chunks[count];
                return chunk[count];
            }
            set
            {
                var chunk = Chunks[count];
                chunk[count] =value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
