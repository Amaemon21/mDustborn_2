using UnityEngine;

namespace UniStorm.CharacterController
{
    public class Pause : MonoBehaviour
    {
        Example.DemoUIController demoUIController;

        void Start()
        {
            demoUIController = FindAnyObjectByType<Example.DemoUIController>();
        }

        void Update()
        {
            if (!demoUIController)
            {
                if (UniStormSystem.Instance != null && !UniStormSystem.Instance.m_MenuToggle)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                if (demoUIController.QualityDropdown.transform.parent.gameObject.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }
}