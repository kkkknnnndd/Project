using UnityEngine;

#region Source And Purpose
// Nguồn: https://discussions.unity.com/t/inspector-field-for-scene-asset/40763/5
// Mục đích: định nghĩa một class SceneField để lưu trữ thông tin về một scene trong Unity project.
// Class này cũng bao gồm một Custom Property Drawer
// để tùy chỉnh cách hiển thị property SceneField trong Unity Inspector. 
// Có thể dễ dàng chọn và quản lý các scene trong project
#endregion

// Preprocessor Directives
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
#region Explain System.Serializable
// Thuộc tính này được áp dụng cho class SceneField.
// Nó cho biết class này có thể được serialize,
// nghĩa là thông tin của các biến bên trong class có thể được lưu và tải trong Unity Editor. 
#endregion
public class SceneField
{
    [SerializeField]
    private Object m_SceneAsset; // Lưu trữ object SceneAsset

    [SerializeField]
    private string m_SceneName = ""; // Lưu trữ tên scene.
    public string SceneName // Property lấy giá trị m_SceneName.
    {
        get { return m_SceneName; }
    }
    #region Explain method
    // Đây là toán tử chuyển đổi (conversion operator) được định nghĩa theo kiểu implicit (ngầm định).
    // Nó cho phép bạn chuyển đổi một object kiểu SceneField trực tiếp thành một chuỗi string.
    // Khi bạn thực hiện phép toán chuyển đổi này, hàm sẽ trả về giá trị của property SceneName.
    // Điều này giúp code của bạn gọn hơn khi cần sử dụng tên scene.
    #endregion
    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName; // huyển đổi SceneField sang string trả về m_SceneName.
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
#region Explain CustomPropertyDrawer
// Thuộc tính này được áp dụng cho class SceneFieldPropertyDrawer.
// Nó cho biết class này là một Custom Property Drawer,
// dùng để tùy chỉnh cách hiển thị property SceneField trong Unity Inspector.
#endregion
public class SceneFieldPropertyDrawer : PropertyDrawer // Định nghĩa class public SceneFieldPropertyDrawer kế thừa từ PropertyDrawer.
{
    #region Explain Method OnGUI
    // được sử dụng để thay đổi giao diện hiển thị của property SceneField trong Unity Inspector.
    // _position: Vị trí rectangle trên màn hình nơi cần hiển thị property.
    //_property: Tham chiếu đến property SceneField đang được hiển thị.
    // _label: Nhãn của property.
    #endregion
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        // Thay đổi giao diện hiển thị SceneField trong Unity Inspector.
        // Bắt đầu hiển thị property.
        EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        // Lấy tham chiếu đến property m_SceneAsset.
        SerializedProperty sceneAsset = _property.FindPropertyRelative("m_SceneAsset");
        // Lấy tham chiếu đến property m_SceneName.
        SerializedProperty sceneName = _property.FindPropertyRelative("m_SceneName");
        #region Explain Line 71
        // Dòng này tạo ra nhãn cho property SceneField tại vị trí _position. Nhãn này có ID duy nhất được tạo bởi GUIUtility.GetControlID
        // Tham số FocusType.Passive cho biết nhãn này không thể được chọn bằng chuột.
        #endregion
        _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (sceneAsset != null)
        {
            #region Explain Line 79.
            // Dòng này hiển thị một field trong Unity Inspector cho phép bạn chọn một SceneAsset từ project.
            // Field này được hiển thị tại vị trí _position và sử dụng tham chiếu hiện tại của sceneAsset làm giá trị mặc định.
            // Kiểu dữ liệu của field được giới hạn ở SceneAsset và false cho biết field không cho phép null.
            #endregion
            sceneAsset.objectReferenceValue = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

            // Nếu giá trị không null, nghĩa là bạn đã chọn một SceneAsset mới.
            if (sceneAsset.objectReferenceValue != null)
            {
                // Dòng này cập nhật giá trị của biến m_SceneName với tên của SceneAsset được chọn.
                // Việc ép kiểu sceneAsset.objectReferenceValue sang SceneAsset đảm bảo rằng bạn chỉ truy cập tên của SceneAsset hợp lệ.
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
            }
        }
        EditorGUI.EndProperty(); // Kết thúc hiển thị property
    }
}
#endif