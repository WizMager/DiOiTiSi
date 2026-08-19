using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Lobby
{
    public class PlayerItem : MonoBehaviour
    {
        [SerializeField] private Image _readyImage;
        [SerializeField] private TMP_Text _nicknameText;
        
        public void SetReadyStatus(bool ready)
        {
            _readyImage.color = ready ? Color.green : Color.red;
        }
        
        public void SetNickname(string nickname)
        {
            _nicknameText.text = nickname;
        }
    }
}