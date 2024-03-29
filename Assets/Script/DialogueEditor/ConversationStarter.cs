using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation nPCConversation;
    [SerializeField] private GameObject Loading_Text;
    private bool _loadingTime = false;
    private bool _isTalking = false;
    private void Awake()
    {
        if (!_loadingTime)
        {
            Loading_Text.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _loadingTime)
        {
            _isTalking = true;
            Loading_Text.SetActive(false);
            ConversationManager.Instance.StartConversation(nPCConversation);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !_isTalking)
        {
            _loadingTime = true;
            if (_loadingTime)
            {
                Loading_Text.SetActive(true);
            }
            else
            {
                Loading_Text.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isTalking = false;
        _loadingTime = false;
        Loading_Text.SetActive(false);
        ConversationManager.Instance.EndConversation();
    }
}
