using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TU.PerfNet
{
	public abstract class RpcPayload
	{
        public string RpcName => GetType().Name;

        // Override this to parse data to specific class variables.
        public virtual void Parse( object[] data )
		{

		}

        public abstract void HandleRpc();
    }

    /// <summary>
    /// Servers as an access point to call remote procedures on clients in a centralized way.
    /// </summary>
    public class ClientRpcController : NetworkBehaviour
    {
        public static ClientRpcController instance;

        static Dictionary<string, System.Type> payloadMap;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;

                // Rebuild payload map (part of the example of how it might be abstracted)\
                payloadMap = new Dictionary<string, System.Type>();

                Assembly assembly = Assembly.GetExecutingAssembly();
                System.Type baseType = typeof(RpcPayload);
                IEnumerable<System.Type> derivedTypes = assembly.GetTypes()
                    .Where(type => baseType.IsAssignableFrom(type) && type != baseType);

                foreach( System.Type t in derivedTypes )
				{
                    object o = System.Activator.CreateInstance(t);
                    string name = (string)o.GetType().GetProperty("RpcName").GetValue(o);
                    payloadMap.Add(name, t);
                    Debug.Log($"Added {name} to map");
                }
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// Creates an RpcPayload based on the rpcName argument, and performs it on the client-side.
        /// </summary>
        /// <param name="rpcName">Name of the class that implements RpcPayload.</param>
        /// <param name="reproduceLocal">True if this behaviour also needs to happen on the calling side.</param>
        /// <param name="data">Any arguments that need to be parsed by the implementing class.</param>
        public static void PerformRpc(string rpcName, bool reproduceLocal, params object[] data)
		{
            instance.RpcCall(rpcName, data);
            if ( reproduceLocal )
			{
                instance.Perform(rpcName, data);
            }
        }

        // DISCUSS: Here's an idea of how we could potentially make this more flexible:
        //  You can implement a function anywhere in the codebase (like the EventController) that calls this more complex function
        [ClientRpc]
        public void RpcCall( string rpcName, params object[] data )
		{
            if ( !payloadMap.ContainsKey(rpcName))
			{
                Debug.LogError($"RpcName not found in Payload Map: {rpcName}");
			}

            // Create payload object, parse & handle
            Perform(rpcName, data);
		}

        private void Perform( string rpcName, params object[] data)
		{
            System.Type t = payloadMap[rpcName];
            RpcPayload payload = (RpcPayload)System.Activator.CreateInstance(t);
            if (data != null)
            {
                payload.Parse(data);
            }
            payload.HandleRpc();
            // Debug.Log($"Handled: {rpcName}");
        }
    }
}