using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FishTools
{
    public class PlayerEntity : BaseSingletonMono<PlayerEntity>
    {
        public Rigidbody2D rig2d;
        public Animator animator;
        public Color oringalColor;

        public TextMeshProUGUI healthText;

        protected override void Awake()
        {
            base.Awake();
            rig2d = this.GetComponent<Rigidbody2D>();
            oringalColor = this.GetComponent<SpriteRenderer>().color;
        }

        private void Update()
        {
            healthText.text = "当前生命值" + datas.CurHP+"/最大生命值"+ datas.MaxHP;
        }

        [SerializeField]
        public PlayerData datas = new PlayerData();

    }
}