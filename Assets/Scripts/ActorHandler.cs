using Mirror;

namespace ContextIII
{
    public class ActorHandler : NetworkBehaviour
    {
        public bool IsActor { get; private set; }

        private void Awake()
        {
            ActorHandler[] actors = FindObjectsOfType<ActorHandler>();

            bool _actorFound = false;

            for (int i = 0; i < actors.Length; i++)
            {
                if (actors[i].IsActor)
                {
                    _actorFound = true;
                    break;
                }
            }

            if (!_actorFound)
            {
                IsActor = true;
            }
        }
    }
}