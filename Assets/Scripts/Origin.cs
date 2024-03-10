using UnityEngine;

namespace ContextIII
{
    public struct Origin
    {
        public Vector3 Position;
        public Vector3 Eulers;

        public Origin(Vector3 position, Vector3 eulers)
        {
            Position = position;
            Eulers = eulers;
        }
    }
}