using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor.IMGUI.Controls;
namespace DhafinFawwaz.ActionExtension {

public class StaticActionFieldDropdown : AdvancedDropdown {
    private Action<FieldInfo> _onSelected;
    private Dictionary<Type, List<FieldInfo>> _actionFields;

    public StaticActionFieldDropdown(AdvancedDropdownState state, Dictionary<Type, List<FieldInfo>> actionFields, Action<FieldInfo> onSelected) 
        : base(state) {
        _actionFields = actionFields;
        _onSelected = onSelected;
    }

    // if its Action<int, string> OnHurt, show OnHurt<int, string>
    public static string GetConvertedName(FieldInfo fieldInfo) {
        Type fieldType = fieldInfo.FieldType;
        string genericArgs = string.Join(", ", fieldType.GetGenericArguments().Select(t => t.Name));
        return $"{fieldInfo.Name}({genericArgs})";
    }

    private class FieldDropdownItem : AdvancedDropdownItem {
        public FieldInfo FieldInfo { get; }
        public FieldDropdownItem(FieldInfo field) : base(GetConvertedName(field)) {
            FieldInfo = field;
        }
    }

    protected override AdvancedDropdownItem BuildRoot() {
        var root = new AdvancedDropdownItem("Select Action");

        foreach (var kvp in _actionFields) {
            Type classType = kvp.Key;
            List<FieldInfo> fields = kvp.Value;

            var classItem = new AdvancedDropdownItem(classType.Name);

            foreach (var field in fields) {
                var item = new FieldDropdownItem(field);
                classItem.AddChild(item);
            }

            root.AddChild(classItem);
        }

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item) {
        if (item is FieldDropdownItem fieldItem) {
            _onSelected?.Invoke(fieldItem.FieldInfo);
        }
    }
}

}