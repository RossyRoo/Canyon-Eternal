// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""6d4be5dc-e3d6-481f-bb51-a243a2d1d410"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""7309bd78-0486-4472-864d-cdeee9629d9c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b5f9b8ba-21ae-41cd-8dff-d4dd66683ecd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cb7caaca-b36a-4af6-b54a-f1e97d76af18"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0f7170e3-e57c-43dc-886d-2aa60c900a79"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7b737998-9e1a-4cea-95ec-5d4b096b12af"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1fe24d31-fe65-4712-9928-73c54a17593b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""d5fbaf1a-7f2c-4cdd-bdc3-9db57fce47a3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cce74c2e-c57b-46a5-9306-c7be8b41fb75"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ca94ab0c-1cb1-47df-bf71-dd457b1eaa06"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""46bb8c0d-2836-4a48-84cf-4020fb4a0725"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3d025745-306e-4265-a08c-ea8e24f18b0a"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""a2dfe137-9624-4d27-a258-35965fb0c3a0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""04938cce-9cd1-46b0-9c36-bc1123483373"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ef150f70-78b9-4c78-bd04-1ae935ae350d"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""62e965ce-c124-4203-b7fb-2f72b47d4fff"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""507d4ff7-983b-4a5e-88ce-0dfa1ab94ee7"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""PlayerActions"",
            ""id"": ""a884f13e-ef30-4c0b-843a-6f93baa30d4a"",
            ""actions"": [
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""2d2cbcfb-f1db-437a-b229-015599f680c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""ef7cc54e-41c3-4027-a7c1-95e93727aca5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heal"",
                    ""type"": ""Button"",
                    ""id"": ""8e03f7c5-8070-4440-8d54-ce90aca3a36d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""db0b053b-677c-4256-9c2a-f1b5e022b6c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""a72f6c03-d39f-4ae1-b59d-e9b43224d22b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spell"",
                    ""type"": ""Button"",
                    ""id"": ""c48c0885-279b-4413-b6c0-f35b91f8955c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickSlot"",
                    ""type"": ""Button"",
                    ""id"": ""302dd86c-ca2a-409d-a2da-da84accab130"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""96aad6bd-3b68-4424-b200-0a29c04537fb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f3c8c2e0-ebe6-4926-b250-3336a9571864"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b2e1e2a-9c8c-4f78-a4b7-61e48f62cc07"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d13e980f-a294-4fa6-ace9-0c9af8068190"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88ac3f2c-89c3-49df-ae2e-cdf0b947446d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4009cad3-7d46-48ab-a5df-a124b7a35440"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e46bf096-595a-4743-b7e1-646873ae66c1"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d605ebf5-bbb6-44fe-a8b0-721b8e6bb04e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2744532-4fc8-4ebe-b41c-7b8dddff2a80"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2ed56d6-33b3-47a5-bcd8-8979fa2918bc"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""759c1545-3b1c-438c-ba47-601daa90f061"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Spell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2aa45452-ecec-4ed2-9a32-0b7cecbbb534"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Spell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""336b3c5f-1f1a-466f-a88f-082f9a5e7aef"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""a978b114-f827-4dd5-b351-a1f74deb7095"",
            ""actions"": [
                {
                    ""name"": ""OpenMenu"",
                    ""type"": ""Button"",
                    ""id"": ""8be32cc1-12fe-4eaa-8f00-157d7aa00d89"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleMenuRight"",
                    ""type"": ""Button"",
                    ""id"": ""237c9419-45ec-4c64-b654-118fc9c646f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleMenuLeft"",
                    ""type"": ""Button"",
                    ""id"": ""aefd054d-0fff-4ce6-93a0-5d52d31085ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleSubmenuRight"",
                    ""type"": ""Button"",
                    ""id"": ""104d2247-05d1-4317-b0f6-bd0d8894669a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleSubmenuLeft"",
                    ""type"": ""Button"",
                    ""id"": ""6ab6231f-9dc2-4f98-9b7e-d97ffc7484f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""57d506a5-09cc-4bb3-bcd9-5302a2e29431"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50639509-d240-47f4-a825-0795aba0ea79"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b0f9b33-b05c-4245-ac9c-1a58ac61c70a"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CycleMenuRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38cf850a-ee3b-40fd-83d7-e468a67f48c5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CycleMenuRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1aa23511-e783-4c30-a9a7-bd89fae6c4c4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CycleMenuLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f989704c-eab6-483d-afee-785889b8a2d0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CycleMenuLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7aeb0400-125e-4a20-84fc-aa73b114628e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CycleSubmenuRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""665dc720-13cf-49c0-b6c2-bed0f1932643"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CycleSubmenuRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d41872d4-b05f-4f36-8965-b2a7acc0d2c7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CycleSubmenuLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c43decf5-d599-4975-b791-9f09484f0e23"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CycleSubmenuLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Move = m_PlayerMovement.FindAction("Move", throwIfNotFound: true);
        // PlayerActions
        m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
        m_PlayerActions_Melee = m_PlayerActions.FindAction("Melee", throwIfNotFound: true);
        m_PlayerActions_Dash = m_PlayerActions.FindAction("Dash", throwIfNotFound: true);
        m_PlayerActions_Heal = m_PlayerActions.FindAction("Heal", throwIfNotFound: true);
        m_PlayerActions_Interact = m_PlayerActions.FindAction("Interact", throwIfNotFound: true);
        m_PlayerActions_Block = m_PlayerActions.FindAction("Block", throwIfNotFound: true);
        m_PlayerActions_Spell = m_PlayerActions.FindAction("Spell", throwIfNotFound: true);
        m_PlayerActions_QuickSlot = m_PlayerActions.FindAction("QuickSlot", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_OpenMenu = m_UI.FindAction("OpenMenu", throwIfNotFound: true);
        m_UI_CycleMenuRight = m_UI.FindAction("CycleMenuRight", throwIfNotFound: true);
        m_UI_CycleMenuLeft = m_UI.FindAction("CycleMenuLeft", throwIfNotFound: true);
        m_UI_CycleSubmenuRight = m_UI.FindAction("CycleSubmenuRight", throwIfNotFound: true);
        m_UI_CycleSubmenuLeft = m_UI.FindAction("CycleSubmenuLeft", throwIfNotFound: true);
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

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Move;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerMovement_Move;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // PlayerActions
    private readonly InputActionMap m_PlayerActions;
    private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_PlayerActions_Melee;
    private readonly InputAction m_PlayerActions_Dash;
    private readonly InputAction m_PlayerActions_Heal;
    private readonly InputAction m_PlayerActions_Interact;
    private readonly InputAction m_PlayerActions_Block;
    private readonly InputAction m_PlayerActions_Spell;
    private readonly InputAction m_PlayerActions_QuickSlot;
    public struct PlayerActionsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Melee => m_Wrapper.m_PlayerActions_Melee;
        public InputAction @Dash => m_Wrapper.m_PlayerActions_Dash;
        public InputAction @Heal => m_Wrapper.m_PlayerActions_Heal;
        public InputAction @Interact => m_Wrapper.m_PlayerActions_Interact;
        public InputAction @Block => m_Wrapper.m_PlayerActions_Block;
        public InputAction @Spell => m_Wrapper.m_PlayerActions_Spell;
        public InputAction @QuickSlot => m_Wrapper.m_PlayerActions_QuickSlot;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
            {
                @Melee.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMelee;
                @Melee.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMelee;
                @Melee.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMelee;
                @Dash.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDash;
                @Heal.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnHeal;
                @Heal.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnHeal;
                @Heal.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnHeal;
                @Interact.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnInteract;
                @Block.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnBlock;
                @Spell.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSpell;
                @Spell.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSpell;
                @Spell.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSpell;
                @QuickSlot.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnQuickSlot;
                @QuickSlot.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnQuickSlot;
                @QuickSlot.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnQuickSlot;
            }
            m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Melee.started += instance.OnMelee;
                @Melee.performed += instance.OnMelee;
                @Melee.canceled += instance.OnMelee;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Heal.started += instance.OnHeal;
                @Heal.performed += instance.OnHeal;
                @Heal.canceled += instance.OnHeal;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @Spell.started += instance.OnSpell;
                @Spell.performed += instance.OnSpell;
                @Spell.canceled += instance.OnSpell;
                @QuickSlot.started += instance.OnQuickSlot;
                @QuickSlot.performed += instance.OnQuickSlot;
                @QuickSlot.canceled += instance.OnQuickSlot;
            }
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_OpenMenu;
    private readonly InputAction m_UI_CycleMenuRight;
    private readonly InputAction m_UI_CycleMenuLeft;
    private readonly InputAction m_UI_CycleSubmenuRight;
    private readonly InputAction m_UI_CycleSubmenuLeft;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenMenu => m_Wrapper.m_UI_OpenMenu;
        public InputAction @CycleMenuRight => m_Wrapper.m_UI_CycleMenuRight;
        public InputAction @CycleMenuLeft => m_Wrapper.m_UI_CycleMenuLeft;
        public InputAction @CycleSubmenuRight => m_Wrapper.m_UI_CycleSubmenuRight;
        public InputAction @CycleSubmenuLeft => m_Wrapper.m_UI_CycleSubmenuLeft;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @OpenMenu.started -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                @OpenMenu.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                @OpenMenu.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                @CycleMenuRight.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleMenuRight;
                @CycleMenuRight.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleMenuRight;
                @CycleMenuRight.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleMenuRight;
                @CycleMenuLeft.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleMenuLeft;
                @CycleMenuLeft.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleMenuLeft;
                @CycleMenuLeft.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleMenuLeft;
                @CycleSubmenuRight.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleSubmenuRight;
                @CycleSubmenuRight.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleSubmenuRight;
                @CycleSubmenuRight.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleSubmenuRight;
                @CycleSubmenuLeft.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleSubmenuLeft;
                @CycleSubmenuLeft.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleSubmenuLeft;
                @CycleSubmenuLeft.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCycleSubmenuLeft;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenMenu.started += instance.OnOpenMenu;
                @OpenMenu.performed += instance.OnOpenMenu;
                @OpenMenu.canceled += instance.OnOpenMenu;
                @CycleMenuRight.started += instance.OnCycleMenuRight;
                @CycleMenuRight.performed += instance.OnCycleMenuRight;
                @CycleMenuRight.canceled += instance.OnCycleMenuRight;
                @CycleMenuLeft.started += instance.OnCycleMenuLeft;
                @CycleMenuLeft.performed += instance.OnCycleMenuLeft;
                @CycleMenuLeft.canceled += instance.OnCycleMenuLeft;
                @CycleSubmenuRight.started += instance.OnCycleSubmenuRight;
                @CycleSubmenuRight.performed += instance.OnCycleSubmenuRight;
                @CycleSubmenuRight.canceled += instance.OnCycleSubmenuRight;
                @CycleSubmenuLeft.started += instance.OnCycleSubmenuLeft;
                @CycleSubmenuLeft.performed += instance.OnCycleSubmenuLeft;
                @CycleSubmenuLeft.canceled += instance.OnCycleSubmenuLeft;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnMelee(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnHeal(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnSpell(InputAction.CallbackContext context);
        void OnQuickSlot(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnOpenMenu(InputAction.CallbackContext context);
        void OnCycleMenuRight(InputAction.CallbackContext context);
        void OnCycleMenuLeft(InputAction.CallbackContext context);
        void OnCycleSubmenuRight(InputAction.CallbackContext context);
        void OnCycleSubmenuLeft(InputAction.CallbackContext context);
    }
}
