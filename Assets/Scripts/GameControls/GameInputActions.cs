//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/GameInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace PotatoFinch.TmgDotsJam.GameControls
{
    public partial class @GameInputActions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""a2f67adf-1fb4-4c3f-90e8-1a30437cd72b"",
            ""actions"": [
                {
                    ""name"": ""PlayerMovement"",
                    ""type"": ""Value"",
                    ""id"": ""c10483ed-ccbd-4f38-a67d-311942646a34"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""KillRandomEnemy"",
                    ""type"": ""Button"",
                    ""id"": ""325cdd55-f963-4f4c-9a01-3689e3668ffc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""178472dd-59e4-40c7-9f7f-8c2911b62683"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BuyUpgradeTest"",
                    ""type"": ""Button"",
                    ""id"": ""c2792e79-eec6-4ff4-aad8-dbe9c7ba748a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BuyUpgradeTest2"",
                    ""type"": ""Button"",
                    ""id"": ""e77732b3-21f2-4fa4-aceb-1546457b7f94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BuyUpgradeTest3"",
                    ""type"": ""Button"",
                    ""id"": ""6743b7c3-d78c-417a-9fab-1e40ec810596"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BuyUpgradeTest4"",
                    ""type"": ""Button"",
                    ""id"": ""bb6b61fb-a029-4052-994d-fe7f644713af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""644e7953-53c6-4af6-b7ce-5664fc6b5719"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fc0294af-3047-413d-9073-a8e2dc9754ed"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bf3ab8d5-362f-43d6-963a-14c9e1151fe4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d62025eb-a306-4215-aaa9-1249ea2c66bc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a8264b66-5588-4021-9a4e-1b71c9fb5f2e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""635882e0-4091-4927-8236-1e8bc12b43ee"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbe00384-e245-4a41-bae8-dca4c7b300c1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KillRandomEnemy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c27a173-149f-4c8f-b3ee-6791adb5279b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6405c6d-451d-4e8a-bb07-348c5f168adb"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c71371e-7e9b-493b-9a40-38e39ea15e66"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BuyUpgradeTest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93514e63-62e6-4b9b-b1e2-326277e580ba"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BuyUpgradeTest2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bb0a267-620f-4890-93a8-857e83da5ada"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BuyUpgradeTest3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""262317d3-a602-42f2-933f-7f05f1ed6191"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BuyUpgradeTest4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Gameplay
            m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
            m_Gameplay_PlayerMovement = m_Gameplay.FindAction("PlayerMovement", throwIfNotFound: true);
            m_Gameplay_KillRandomEnemy = m_Gameplay.FindAction("KillRandomEnemy", throwIfNotFound: true);
            m_Gameplay_PauseGame = m_Gameplay.FindAction("PauseGame", throwIfNotFound: true);
            m_Gameplay_BuyUpgradeTest = m_Gameplay.FindAction("BuyUpgradeTest", throwIfNotFound: true);
            m_Gameplay_BuyUpgradeTest2 = m_Gameplay.FindAction("BuyUpgradeTest2", throwIfNotFound: true);
            m_Gameplay_BuyUpgradeTest3 = m_Gameplay.FindAction("BuyUpgradeTest3", throwIfNotFound: true);
            m_Gameplay_BuyUpgradeTest4 = m_Gameplay.FindAction("BuyUpgradeTest4", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Gameplay
        private readonly InputActionMap m_Gameplay;
        private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
        private readonly InputAction m_Gameplay_PlayerMovement;
        private readonly InputAction m_Gameplay_KillRandomEnemy;
        private readonly InputAction m_Gameplay_PauseGame;
        private readonly InputAction m_Gameplay_BuyUpgradeTest;
        private readonly InputAction m_Gameplay_BuyUpgradeTest2;
        private readonly InputAction m_Gameplay_BuyUpgradeTest3;
        private readonly InputAction m_Gameplay_BuyUpgradeTest4;
        public struct GameplayActions
        {
            private @GameInputActions m_Wrapper;
            public GameplayActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @PlayerMovement => m_Wrapper.m_Gameplay_PlayerMovement;
            public InputAction @KillRandomEnemy => m_Wrapper.m_Gameplay_KillRandomEnemy;
            public InputAction @PauseGame => m_Wrapper.m_Gameplay_PauseGame;
            public InputAction @BuyUpgradeTest => m_Wrapper.m_Gameplay_BuyUpgradeTest;
            public InputAction @BuyUpgradeTest2 => m_Wrapper.m_Gameplay_BuyUpgradeTest2;
            public InputAction @BuyUpgradeTest3 => m_Wrapper.m_Gameplay_BuyUpgradeTest3;
            public InputAction @BuyUpgradeTest4 => m_Wrapper.m_Gameplay_BuyUpgradeTest4;
            public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
            public void AddCallbacks(IGameplayActions instance)
            {
                if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
                @PlayerMovement.started += instance.OnPlayerMovement;
                @PlayerMovement.performed += instance.OnPlayerMovement;
                @PlayerMovement.canceled += instance.OnPlayerMovement;
                @KillRandomEnemy.started += instance.OnKillRandomEnemy;
                @KillRandomEnemy.performed += instance.OnKillRandomEnemy;
                @KillRandomEnemy.canceled += instance.OnKillRandomEnemy;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @BuyUpgradeTest.started += instance.OnBuyUpgradeTest;
                @BuyUpgradeTest.performed += instance.OnBuyUpgradeTest;
                @BuyUpgradeTest.canceled += instance.OnBuyUpgradeTest;
                @BuyUpgradeTest2.started += instance.OnBuyUpgradeTest2;
                @BuyUpgradeTest2.performed += instance.OnBuyUpgradeTest2;
                @BuyUpgradeTest2.canceled += instance.OnBuyUpgradeTest2;
                @BuyUpgradeTest3.started += instance.OnBuyUpgradeTest3;
                @BuyUpgradeTest3.performed += instance.OnBuyUpgradeTest3;
                @BuyUpgradeTest3.canceled += instance.OnBuyUpgradeTest3;
                @BuyUpgradeTest4.started += instance.OnBuyUpgradeTest4;
                @BuyUpgradeTest4.performed += instance.OnBuyUpgradeTest4;
                @BuyUpgradeTest4.canceled += instance.OnBuyUpgradeTest4;
            }

            private void UnregisterCallbacks(IGameplayActions instance)
            {
                @PlayerMovement.started -= instance.OnPlayerMovement;
                @PlayerMovement.performed -= instance.OnPlayerMovement;
                @PlayerMovement.canceled -= instance.OnPlayerMovement;
                @KillRandomEnemy.started -= instance.OnKillRandomEnemy;
                @KillRandomEnemy.performed -= instance.OnKillRandomEnemy;
                @KillRandomEnemy.canceled -= instance.OnKillRandomEnemy;
                @PauseGame.started -= instance.OnPauseGame;
                @PauseGame.performed -= instance.OnPauseGame;
                @PauseGame.canceled -= instance.OnPauseGame;
                @BuyUpgradeTest.started -= instance.OnBuyUpgradeTest;
                @BuyUpgradeTest.performed -= instance.OnBuyUpgradeTest;
                @BuyUpgradeTest.canceled -= instance.OnBuyUpgradeTest;
                @BuyUpgradeTest2.started -= instance.OnBuyUpgradeTest2;
                @BuyUpgradeTest2.performed -= instance.OnBuyUpgradeTest2;
                @BuyUpgradeTest2.canceled -= instance.OnBuyUpgradeTest2;
                @BuyUpgradeTest3.started -= instance.OnBuyUpgradeTest3;
                @BuyUpgradeTest3.performed -= instance.OnBuyUpgradeTest3;
                @BuyUpgradeTest3.canceled -= instance.OnBuyUpgradeTest3;
                @BuyUpgradeTest4.started -= instance.OnBuyUpgradeTest4;
                @BuyUpgradeTest4.performed -= instance.OnBuyUpgradeTest4;
                @BuyUpgradeTest4.canceled -= instance.OnBuyUpgradeTest4;
            }

            public void RemoveCallbacks(IGameplayActions instance)
            {
                if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGameplayActions instance)
            {
                foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GameplayActions @Gameplay => new GameplayActions(this);
        public interface IGameplayActions
        {
            void OnPlayerMovement(InputAction.CallbackContext context);
            void OnKillRandomEnemy(InputAction.CallbackContext context);
            void OnPauseGame(InputAction.CallbackContext context);
            void OnBuyUpgradeTest(InputAction.CallbackContext context);
            void OnBuyUpgradeTest2(InputAction.CallbackContext context);
            void OnBuyUpgradeTest3(InputAction.CallbackContext context);
            void OnBuyUpgradeTest4(InputAction.CallbackContext context);
        }
    }
}
