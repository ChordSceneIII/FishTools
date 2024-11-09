using System.IO;
using UnityEngine;

namespace FishToolsDEMO
{
    public class GameInitializer : MonoBehaviour
    {
        void Awake()
        {
            Initializer();
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void Initializer()
        {
            PlayerControl.Instance.Initializer();
        }
    }
}