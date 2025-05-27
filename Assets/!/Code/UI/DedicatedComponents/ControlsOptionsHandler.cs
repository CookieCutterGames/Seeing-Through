// using System;
// using TMPro;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class ControlsOptionsHandler : MonoBehaviour
// {
//     [SerializeField]
//     private Transform Container;

//     [SerializeField]
//     private GameObject RecordPrefab;
//     private bool IsInitialized = false;

//     void OnEnable()
//     {
//         if (IsInitialized)
//             return;
//         Initialization();
//     }

//     void Initialization()
//     {
//         if (RecordPrefab == null)
//         {
//             Debug.LogError("Prefab not setted up - AudioOptionsHandler");
//             return;
//         }
//         if (Container == null)
//         {
//             Container = transform;
//         }
//         CreateRecord(
//             RecordPrefab,
//             InputControlPath.ToHumanReadableString(
//                 InputSystem.actions.FindAction("Attack").bindings[0].effectivePath,
//                 InputControlPath.HumanReadableStringOptions.OmitDevice
//             ),
//             "użyj misia".ToUpper(),
//             value => InputSystem.actions.FindAction("Attack").ApplyBindingOverride(value)
//         );

//         CreateRecord(
//             RecordPrefab,
//             InputControlPath.ToHumanReadableString(
//                 InputSystem.actions.FindAction("Attack2").bindings[0].effectivePath,
//                 InputControlPath.HumanReadableStringOptions.OmitDevice
//             ),
//             "rzut przedmiotem".ToUpper(),
//             value => InputSystem.actions.FindAction("Attack2").ApplyBindingOverride(value)
//         );
//         CreateRecord(
//             RecordPrefab,
//             InputControlPath.ToHumanReadableString(
//                 InputSystem.actions.FindAction("Interact").bindings[0].effectivePath,
//                 InputControlPath.HumanReadableStringOptions.OmitDevice
//             ),
//             "Interakcja".ToUpper(),
//             value => InputSystem.actions.FindAction("Interact").ApplyBindingOverride(value)
//         );
//         CreateRecord(
//             RecordPrefab,
//             InputControlPath.ToHumanReadableString(
//                 InputSystem.actions.FindAction("Sprint").bindings[0].effectivePath,
//                 InputControlPath.HumanReadableStringOptions.OmitDevice
//             ),
//             "Sprint".ToUpper(),
//             value => InputSystem.actions.FindAction("Sprint").ApplyBindingOverride(value)
//         );
//         CreateRecord(
//             RecordPrefab,
//             InputControlPath.ToHumanReadableString(
//                 InputSystem.actions.FindAction("TurnPauseMenu").bindings[0].effectivePath,
//                 InputControlPath.HumanReadableStringOptions.OmitDevice
//             ),
//             "Menu Pauzy".ToUpper(),
//             value => InputSystem.actions.FindAction("TurnPauseMenu").ApplyBindingOverride(value)
//         );
//         IsInitialized = true;
//     }

//     void CreateRecord(
//         GameObject prefab,
//         string currentKeybind,
//         string name,
//         Action<string> onValueChanged,
//         bool disabled = false
//     )
//     {
//         var obj = Instantiate(prefab, Container);
//         if (obj.TryGetComponent(out RebindActionUI component))
//         {
//             component.key.text = name;
//             component.value.GetComponentInChildren<TextMeshProUGUI>().text = currentKeybind;
//             component.value.enabled = !disabled;
//             component.value.GetComponentInChildren<TextMeshProUGUI>().color = disabled
//                 ? Color.grey
//                 : Color.white;
//         }
//     }
// }
